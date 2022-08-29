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
            var dependencies = new List<PlantUmlDependency>();

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
                dependencies.AddRange(plantUmlDependencies);

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
                            dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target}).Contains(node.Key.FullName))
                        .Select(node => node.Value);

            _diagram.AddElements(nodeElements);
            _diagram.AddElements(dependencies);

            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<Slice> slices, GenerationOptions generationOptions = null)
        {
            
            var sliceList = slices.Distinct().ToList();
            var sliceListCopy = slices.Distinct().ToList();
            
            if (generationOptions == null)
            {
                generationOptions = new GenerationOptions();
            }

            var nodes = new Dictionary<Slice, IPlantUmlElement>();
            var dependencies = new List<PlantUmlDependency>();

            for (var i = sliceListCopy.Capacity-1; i >= 0; i--)
            {
                var tmpslc = sliceListCopy[i].Description;
                var dots = 0;
                while (tmpslc.Contains("."))
                {
                    dots++;
                    tmpslc = tmpslc.Remove(0, tmpslc.IndexOf(".", StringComparison.Ordinal)+1);
                }
                if (sliceListCopy[i].NameSpace != null)
                {
                    tmpslc = sliceListCopy[i].NameSpace;
                    while (tmpslc.Contains("."))
                    {
                        dots--;
                        tmpslc = tmpslc.Remove(0, tmpslc.IndexOf(".", StringComparison.Ordinal)+1);
                    }
                }
                if (sliceListCopy[i].CountOfAsteriskInPattern != null & dots >= sliceListCopy[i].CountOfAsteriskInPattern)
                {
                    sliceList.RemoveAt(i);
                }
            }

            foreach (var slice in sliceList)
            {
                var dependencyTargets = sliceList.Where(targetSlice =>
                    targetSlice.Description != slice.Description &&
                    slice.Dependencies.Where(dep =>
                            generationOptions.DependencyFilter?.Invoke(dep) ?? true)
                        .Any(dep => targetSlice.Types.Contains(dep.Target)));

                dependencies.AddRange(dependencyTargets.Select(target =>
                    new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOne)));

                if (slice is Namespace) //This throws errors if namespaces and slices are used in the same diagram, the syntax can't be mixed in PlantUML
                {
                    nodes.Add(slice, new PlantUmlNamespace(slice.Description));
                }
                else
                {
                    nodes.Add(slice, new PlantUmlSlice(slice.Description, slice.CountOfAsteriskInPattern, slice.NameSpace));
                }
            }

            if (!generationOptions.IncludeNodesWithoutDependencies)
            {
                foreach (var entry in nodes.Where(node =>
                             !dependencies.SelectMany(dep => new[] {dep.Origin, dep.Target})
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

            _diagram.AddElements(nodeElements.OrderBy(element =>
                element is PlantUmlNamespace @namespace ? @namespace.Name.Length : -1));
            _diagram.AddElements(dependencies);

            return this;
        }
    }
}