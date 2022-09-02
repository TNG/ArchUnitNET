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
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlFileBuilder
    {
        private readonly PlantUmlDiagram _diagram = new PlantUmlDiagram();
        
        private List<PlantUmlDependency> _dependencies = new List<PlantUmlDependency>();

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

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<Slice> slices)
        {
            _sliceList = slices.Distinct().ToList();
            RemovePatternInappropriateSlices();

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            var generationOptions = new GenerationOptions();


            foreach (var slice in _sliceList)
            {
                var sliceIsPackage = IfPackage(slice);

                var dependencyTargets = _sliceList.Where(targetSlice =>
                    targetSlice.Description != slice.Description &&
                    slice.Dependencies.Where(dep =>
                            generationOptions.DependencyFilter?.Invoke(dep) ?? true)
                        .Any(dep => targetSlice.Types.Contains(dep.Target)));

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
                    nodes.Add(slice, new PlantUmlSlice(slice.Description, slice.CountOfAsteriskInPattern, slice.NameSpace));
                }
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                foreach (var entry in nodes.Where(node =>
                             !_dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target})
                                 .Contains(node.Key.Description)))
                {
                    nodes.Remove(entry.Key);
                }
            }

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

            RemoveDuplicateDependenciesWhenShowingPackages();
            RemoveCircles();
            RemoveDuplicatedArrowsIfExist();
            _diagram.AddElements(nodeElements.OrderBy(element =>
                element is PlantUmlNamespace @namespace ? @namespace.Name.Length : -1));
            _diagram.AddElements(_dependencies);

            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFromCompact(IEnumerable<Slice> slices)
        {
            _sliceList = slices.Distinct().ToList();
            RemovePatternInappropriateSlices();
            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            var generationOptions = new GenerationOptions();

            foreach (var slice in _sliceList)
            {
                
                var sliceIsPackage = IfPackage(slice);

                if (slice is Namespace) //This throws errors if namespaces and slices are used in the same diagram, the syntax can't be mixed in PlantUML
                {
                    nodes.Add(slice, new PlantUmlNamespace(slice.Description));
                }
                else if (!sliceIsPackage || !slice.ContainsNamespace())
                {
                    nodes.Add(slice,
                        new PlantUmlSlice(slice.Description, slice.CountOfAsteriskInPattern, slice.NameSpace));
                }
                else
                {
                    nodes.Add(slice,
                        new PlantUmlSlice(slice.Description + ".", slice.CountOfAsteriskInPattern, slice.NameSpace));
                }
                   
                
                var dependencyTargets = _sliceList.Where(targetSlice =>
                    targetSlice.Description != slice.Description &&
                    slice.Dependencies.Where(dep =>
                            generationOptions.DependencyFilter?.Invoke(dep) ?? true)
                        .Any(dep => targetSlice.Types.Contains(dep.Target)));

                if (slice.ContainsNamespace())
                {
                    if (sliceIsPackage)
                    {
                        _dependencies.AddRange(dependencyTargets.Select(target =>
                            new PlantUmlDependency(slice.Description, target.Description, DependencyType.PackageToPackageIfSimilarNamespace)));
                    }
                    else
                    {
                        var dt = new List<PlantUmlDependency>();
                        dt.AddRange(dependencyTargets.Select(target =>
                            new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOneIfSimilarNamespace)));
                        for (var i = dt.Count - 1; i >= 0; i--)
                        {
                            if (dt[i].OriginCountOfDots() >= dt[i].TargetCountOfDots() && 
                                _sliceList.Any(slc => slc.Description.Contains(dt[i].Target) && 
                                                      slc.Description != dt[i].Target))
                            {
                                dt.RemoveAt(i);
                            }
                        }
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
                foreach (var entry in nodes.Where(node =>
                             !_dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target})
                                 .Contains(node.Key.Description)))
                {
                    nodes.Remove(entry.Key);
                }
            }

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

            RemoveCircles();
            RemoveDuplicatedArrowsIfExist();
            
            _diagram.AddElements(nodeElements.OrderBy(element =>
                element is PlantUmlNamespace @namespace ? @namespace.Name.Length : -1));
            _diagram.AddElements(_dependencies);

            return this;
        }

        private void RemovePatternInappropriateSlices()
        {
            var sliceListCopy = new List<Slice>();
            sliceListCopy.AddRange(_sliceList);
            
            for (var i = sliceListCopy.Capacity - 1; i >= 0; i--)
            {
                var currentSliceDescription = sliceListCopy[i].Description;
                var dots = 0;
                while (currentSliceDescription.Contains("."))
                {
                    dots++;
                    currentSliceDescription = currentSliceDescription.Remove(0, currentSliceDescription.IndexOf(".", StringComparison.Ordinal) + 1);
                }

                if (sliceListCopy[i].ContainsNamespace())
                {
                    currentSliceDescription = sliceListCopy[i].NameSpace;
                    while (currentSliceDescription.Contains("."))
                    {
                        dots--;
                        currentSliceDescription = currentSliceDescription.Remove(0, currentSliceDescription.IndexOf(".", StringComparison.Ordinal) + 1);
                    }
                }

                if (sliceListCopy[i].CountOfAsteriskInPattern != null &
                    dots >= sliceListCopy[i].CountOfAsteriskInPattern)
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
            if (!_sliceList[0].ContainsNamespace())
            {
                return;
            }

            for (var j = _dependencies.Count-1; j >= 0; j--)
            {
                if (_sliceList.Any(slice => slice.Description.Contains(_dependencies[j].Target) &&
                                            slice.Description != _dependencies[j].Target))
                {
                    if (_dependencies[j].DependencyType == DependencyType.PackageToOne)
                    {
                        if (_dependencies[j].Origin.Contains(_dependencies[j].Target))
                        {
                            _dependencies.RemoveAt(j);
                        }
                        else
                        {
                            _dependencies[j] =
                                new PlantUmlDependency(_dependencies[j].Origin, _dependencies[j].Target,
                                    DependencyType.PackageToPackage);
                        }
                    }
                    else
                    {
                        if (_dependencies[j].Origin.Contains(_dependencies[j].Target))
                        {
                            _dependencies.RemoveAt(j);
                        }
                        else
                        {
                            _dependencies[j] =
                                new PlantUmlDependency(_dependencies[j].Origin, _dependencies[j].Target,
                                    DependencyType.OneToPackage);   
                        }
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

        private bool IfPackage(Slice slice)
        {
            if (!slice.ContainsNamespace())
            {
                return false;
            }

            for (var i = _sliceList.Count - 1; i >= 0; i--)
            {
                if (_sliceList[i].Description != slice.Description &
                    _sliceList[i].Description.Contains(slice.Description))
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveCircles()
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
    }
}