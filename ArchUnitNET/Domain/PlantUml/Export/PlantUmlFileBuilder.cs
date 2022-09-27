//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlFileBuilder
    {
        private readonly PlantUmlDiagram _diagram = new PlantUmlDiagram();
        
        private readonly List<PlantUmlDependency> _dependencies = new List<PlantUmlDependency>();

        private List<Slice> _sliceList = new List<Slice>();

        public string AsString(RenderOptions renderOptions = null)
        {
            if (renderOptions == null)
            {
                renderOptions = new RenderOptions();
            }

            return _diagram.GetPlantUmlString(renderOptions);
        }

        public void WriteToFile(string path, RenderOptions renderOptions = null, bool overwrite = true)
        {
            if (!overwrite && File.Exists(path))
            {
                throw new FileAlreadyExistsException("File already exists and overwriting is disabled.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path."));

            using (var sw = File.CreateText(path))
            {
                sw.Write(AsString(renderOptions));
            }
        }

        public PlantUmlFileBuilder WithElements(IEnumerable<IPlantUmlElement> elements)
        {
            _diagram.AddElements(elements);
            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<IType> types, GenerationOptions generationOptions = null)
        {
            var typeList = types.Distinct().ToList();
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            var nodes = new Dictionary<IType, IPlantUmlElement>();

            foreach (var type in typeList)
            {
                var filteredTypeDependencies =
                    type.Dependencies.Where(dep =>
                        (generationOptions.DependencyFilter?.Invoke(dep) ?? true) && !Equals(dep.Origin, dep.Target));

                if (!generationOptions.IncludeDependenciesToOther)
                {
                    filteredTypeDependencies = filteredTypeDependencies.Where(dep => typeList.Contains(dep.Target));
                }

                var plantUmlDependencies = filteredTypeDependencies.Select(dep =>
                        new PlantUmlDependency(dep.Origin.FullName, dep.Target.FullName, DependencyType.OneToOne))
                    .Distinct();
                _dependencies.AddRange(plantUmlDependencies);

                if (type is Interface)
                {
                    nodes.Add(type, new PlantUmlInterface(type.FullName));
                }
                else
                {
                    var plantUmlClass = new PlantUmlClass(type.FullName);
                    foreach (var member in type.Members)
                    {
                        plantUmlClass.AddField(member.Name);
                    }

                    nodes.Add(type, plantUmlClass);
                }
            }

            var nodeElements =
                generationOptions.IncludeNodesWithoutDependencies
                    ? nodes.Values
                    : nodes.Where(node =>
                            _dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target}).Contains(node.Key.FullName))
                        .Select(node => node.Value);

            _diagram.AddElements(nodeElements);
            _diagram.AddElements(_dependencies);

            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<Slice> slices, GenerationOptions generationOptions = null)
        {
            _sliceList = slices.Distinct().ToList();
            RemovePatternInappropriateSlices();

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            foreach (var slice in _sliceList)
            {
                var sliceIsPackage = IsPackage(slice);
                var dependencyTargets = SelectDependencies(slice, generationOptions);

                if (slice is Namespace) //This throws errors if namespaces and slices are used in the same diagram, the syntax can't be mixed in PlantUML
                {
                    nodes.Add(slice, new PlantUmlNamespace(slice.Description));
                }
                else if (!sliceIsPackage)
                {
                    nodes.Add(slice, new PlantUmlSlice(slice.Description, slice.NameSpace));
                }

                if (!generationOptions.CompactVersion)
                {
                    if (sliceIsPackage)
                    {
                        _dependencies.AddRange(dependencyTargets.Select(target =>
                            new PlantUmlDependency(slice.Description, target.Description, DependencyType.PackageToOne)));
                    }
                    else
                    {
                        _dependencies.AddRange(dependencyTargets.Select(target =>
                            new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOne)));
                    }
                }
                else if (slice.ContainsNamespace())
                {
                    if (sliceIsPackage)
                    {
                        _dependencies.AddRange(dependencyTargets.Select(target =>
                            new PlantUmlDependency(slice.Description, target.Description, DependencyType.PackageToPackageIfSameParentNamespace)));
                    }
                    else
                    {
                        var dt = dependencyTargets.Select(
                                target => new PlantUmlDependency(slice.Description, target.Description,
                                    DependencyType.OneToOneIfSameParentNamespace)
                            )
                            .Where(dependency => dependency.OriginCountOfDots() < dependency.TargetCountOfDots() ||
                                                 _sliceList.All(slc =>
                                                     slc.Description == dependency.Target ||
                                                     !slc.Description.Contains(dependency.Target)));
                        _dependencies.AddRange(dt);
                    }
                }
                else
                {
                    _dependencies.AddRange(dependencyTargets.Select(target =>
                        new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOneCompact)));
                }
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                nodes = RemoveNodesNotContainedInDependencies(nodes);
            }

            var nodeElements = HandleNodes(nodes);

            if (!generationOptions.CompactVersion)
            {
                RemoveDuplicateDependenciesWhenShowingPackages();
                ReplaceCirclesWithAppropriateDependencyType();                
            }
            RemoveDuplicatedArrowsIfExist();

            _diagram.AddElements(nodeElements.OrderBy(element =>
                element is PlantUmlNamespace @namespace ? @namespace.Name.Length : -1));
            _diagram.AddElements(_dependencies);

            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFromFocusOn(IEnumerable<Slice> slices, string package)
        {
            _sliceList = slices.Distinct().ToList();

            switch (package)
            {
                case "":
                    throw new ArgumentException("Package can't be empty");
                case ".":
                    throw new ArgumentException("Package can't contain a single dot only");
            }

            if (package[package.Length-1] == '.')
            {
                package = package.Remove(package.Length - 1);
            }

            RemovePatternInappropriateSlices(package);

            var existPackage = _sliceList.Any(slice => (slice.NameSpace + slice.Description).Contains(package));
            if (!existPackage)
            {
                throw new ArgumentException("The package [" + package + "] is not contained in this slice");
            }

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            var generationOptions = new GenerationOptions();

            foreach (var slice in _sliceList)
            {
                var sliceIsPackage = IsPackage(slice);
                var dependencyTargets = SelectDependencies(slice, generationOptions);

                if (sliceIsPackage)
                {
                    _dependencies.AddRange(dependencyTargets.Select(target =>
                        new PlantUmlDependency(slice.Description, target.Description, DependencyType.PackageToOne)));
                }
                else
                {
                    _dependencies.AddRange(dependencyTargets.Select(target =>
                        new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOne)));
                }

                if (slice is Namespace) //This throws errors if namespaces and slices are used in the same diagram, the syntax can't be mixed in PlantUML
                {
                    nodes.Add(slice, new PlantUmlNamespace(slice.Description));
                }
                else if (!sliceIsPackage)
                {
                    nodes.Add(slice,
                        slice.Description.Contains(package)
                            ? new PlantUmlSlice(slice.Description, slice.NameSpace, "99ffd1")
                            : new PlantUmlSlice(slice.Description, slice.NameSpace));
                }
                else if (!slice.Description.Contains(package))
                { 
                    nodes.Add(slice, new PlantUmlSlice(slice.Description + ".", slice.NameSpace));
                }
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                nodes = RemoveNodesNotContainedInDependencies(nodes);
            }

            RemoveDuplicateDependenciesWhenShowingPackages();
            ReplaceCirclesWithAppropriateDependencyType();
            RemoveDuplicatedArrowsIfExist();

            for (var j = _dependencies.Count - 1; j >= 0; j--)
            {
                if (!_dependencies[j].Origin.Contains(package) && !_dependencies[j].Target.Contains(package))
                {
                    _dependencies.RemoveAt(j);
                }
            }

            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var nodeExist = _dependencies.Any(dep => nodes.ElementAt(i).Key.Description == dep.Origin || nodes.ElementAt(i).Key.Description == dep.Target);
                if (!nodeExist)
                {
                    nodes.Remove(nodes.ElementAt(i).Key);
                }
            }

            var nodeElements = HandleNodes(nodes);

            _diagram.AddElements(nodeElements.OrderBy(element =>
                element is PlantUmlNamespace @namespace ? @namespace.Name.Length : -1));

            var tempList = _sliceList.Where(slice => IsPackage(slice) && slice.Description.Contains(package))
                .Select(slice => new PlantUmlSlice(slice.Description + ".", slice.NameSpace, "99ffd1"))
                .ToList();
            if (tempList.Count > 0)
            {
                _diagram.AddElements(tempList.OrderByDescending(element => element.ToString()));
            }
            _diagram.AddElements(_dependencies);

            return this;
        }

        private Dictionary<Slice, IPlantUmlElement> RemoveNodesNotContainedInDependencies(Dictionary<Slice, IPlantUmlElement> nodes)
        {
            foreach (var entry in nodes.Where(node =>
                         !_dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target})
                             .Contains(node.Key.Description)))
            {
                nodes.Remove(entry.Key);
            }
            return nodes;
        }

        private static IEnumerable<IPlantUmlElement> HandleNodes(Dictionary<Slice, IPlantUmlElement> nodes)
        {
            var nodeElements = nodes.Values.ToList();

            foreach (var node in nodes)
            {
                if (node.Key is Namespace)
                {
                    var namespaceName = node.Key.Description;
                    var dotIndex = namespaceName.Length;
                    var parentNamespaces = new List<string>();
                    while (dotIndex != -1)
                    {
                        namespaceName = namespaceName.Substring(0, dotIndex);
                        parentNamespaces.Add(namespaceName);
                        dotIndex = namespaceName.LastIndexOf('.');
                    }

                    parentNamespaces.Reverse();
                    foreach (var namespc in parentNamespaces.Where(namespcName => nodeElements
                                 .OfType<PlantUmlNamespace>()
                                 .All(element => element.Name != namespcName)))
                    {
                        nodeElements.Add(new PlantUmlNamespace(namespc));
                    }
                }
            }

            return nodeElements;
        }

        private IEnumerable<Slice> SelectDependencies(Slice slice, GenerationOptions generationOptions) => 
            _sliceList.Where(targetSlice =>
                targetSlice.Description != slice.Description &&
                slice.Dependencies.Where(dep => generationOptions.DependencyFilter?.Invoke(dep) ?? true)
                    .Any(dep => targetSlice.Types.Contains(dep.Target)));

        private void RemovePatternInappropriateSlices(string thatContainsThisString = null)
        {
            if (_sliceList.Any(slice => slice.CountOfAsteriskInPattern == null))
            {
                return;
            }
            if (thatContainsThisString != null && !_sliceList.Any(slice => (slice.NameSpace+slice.Description).Contains(thatContainsThisString)))
            {
                return;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                var dots = 0;

                if (thatContainsThisString != null && !(_sliceList[i].Description).Contains(thatContainsThisString))
                {
                    continue;
                }

                dots += _sliceList[i].Description.Count(c => c == '.');
                dots -= (_sliceList[i].NameSpace ?? string.Empty).Count(c => c == '.');

                if (dots >= _sliceList[i].CountOfAsteriskInPattern)
                {
                    _sliceList.RemoveAt(i);
                }
            }
        }

        private void RemoveDuplicatedArrowsIfExist()
        {
            for (var i = _dependencies.Count - 1; i >= 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (_dependencies[i].GetPlantUmlString() == _dependencies[j].GetPlantUmlString())
                    {
                        _dependencies.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void RemoveDuplicateDependenciesWhenShowingPackages()
        {
            if (_sliceList.Any(slice => !slice.ContainsNamespace()))
            {
                return;
            }

            for (var j = _dependencies.Count-1; j >= 0; j--)
            {
                if (_sliceList.Any(slice => slice.Description.Contains(_dependencies[j].Target) &&
                                            slice.Description != _dependencies[j].Target))
                {
                    if (_dependencies[j].Origin.Contains(_dependencies[j].Target))
                    {
                        _dependencies.RemoveAt(j);
                    }
                    else if (_dependencies[j].DependencyType == DependencyType.PackageToOne)
                    {      
                        _dependencies[j] = 
                            new PlantUmlDependency(_dependencies[j].Origin, _dependencies[j].Target, DependencyType.PackageToPackage);
                    }
                    else
                    { 
                        _dependencies[j] = 
                            new PlantUmlDependency(_dependencies[j].Origin, _dependencies[j].Target, DependencyType.OneToPackage);
                    }
                    continue;
                }

                if (_dependencies[j].DependencyType != DependencyType.PackageToOne)
                {
                    continue;
                }

                if (_dependencies[j].Target.Contains(_dependencies[j].Origin))
                {
                    _dependencies.RemoveAt(j);
                }
            }

            for (var i = _dependencies.Count - 1; i >= 0; i--)
            {
                if (_dependencies[i].DependencyType == DependencyType.PackageToOne ||
                    _dependencies[i].DependencyType == DependencyType.PackageToPackage)
                {
                    if (_dependencies.Any(dependency => _dependencies[i].Target == dependency.Target &&
                                                        dependency.Origin.Contains(_dependencies[i].Origin) &&
                                                        dependency.Origin != _dependencies[i].Origin))
                    {
                        _dependencies.RemoveAt(i);
                        continue;
                    }
                }

                if (_dependencies[i].DependencyType == DependencyType.OneToPackage ||
                    _dependencies[i].DependencyType == DependencyType.PackageToPackage)
                {
                    if (_dependencies.Any(dependency => dependency.Target.Contains(_dependencies[i].Target) &&
                                                        dependency.Target != _dependencies[i].Target &&
                                                        dependency.Origin.Contains(_dependencies[i].Origin)))
                    {
                        _dependencies.RemoveAt(i);
                        continue;
                    }
                }

                if (
                    _dependencies[i].Target.Contains(_dependencies[i].Origin) &&
                    _dependencies[i].Target != _dependencies[i].Origin
                )
                {
                    _dependencies.RemoveAt(i);
                }
            }
        }

        private void ReplaceCirclesWithAppropriateDependencyType()
        {
            for (var i = _dependencies.Count-1; i >= 0; i--)
            {
                for (var j = i-1; j >= 0; j--)
                {
                    if (_dependencies[i].Target != _dependencies[j].Origin ||
                        _dependencies[i].Origin != _dependencies[j].Target)
                    {
                        continue;
                    }

                    _dependencies.RemoveAt(i);
                    _dependencies[j] = new PlantUmlDependency(_dependencies[j].Origin, _dependencies[j].Target,
                        DependencyType.Circle);
                    break;
                }
            }
        }

        private bool IsPackage(Slice slice)
        {
            if (!slice.ContainsNamespace())
            {
                return false;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                if (_sliceList[i].Description != slice.Description &
                    _sliceList[i].Description.StartsWith(slice.Description))
                {
                    return true;
                }
            }

            return false;
        }
    }
}