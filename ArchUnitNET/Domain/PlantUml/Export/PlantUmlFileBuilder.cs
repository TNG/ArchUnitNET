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
using ArchUnitNET.Domain.Extensions;
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

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<IType> types,
            bool includeDependenciesToOther = false)
        {
            var typeList = types.Distinct().ToList();

            foreach (var type in typeList)
            {
                if (type is Interface)
                {
                    _diagram.AddElement(new PlantUmlInterface(type.FullName));
                }
                else
                {
                    var plantUmlClass = new PlantUmlClass(type.FullName);
                    foreach (var member in type.Members)
                    {
                        plantUmlClass.AddField(member.Name);
                    }

                    _diagram.AddElement(plantUmlClass);
                }

                var targets = type.GetTypeDependencies().Where(target => !Equals(target, type))
                    .Distinct();

                if (!includeDependenciesToOther)
                {
                    targets = targets.Intersect(typeList);
                }

                var plantUmlDependencies = targets.Select(t =>
                    new PlantUmlDependency(type.FullName, t.FullName, DependencyType.OneToOne));
                _diagram.AddElements(plantUmlDependencies);
            }

            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<Slice> slices)
        {
            var sliceList = slices.Distinct().ToList();

            foreach (var slice in sliceList)
            {
                if (slice is Namespace)
                {
                    var namespaceName = slice.Description;
                    var dotIndex = namespaceName.Length;
                    var parentNamespaces = new List<string>();
                    while (dotIndex != -1)
                    {
                        namespaceName = namespaceName.Substring(0, dotIndex);
                        parentNamespaces.Add(namespaceName);
                        dotIndex = namespaceName.LastIndexOf('.');
                    }

                    parentNamespaces.Reverse();
                    foreach (var namespc in parentNamespaces.Where(namespcName => _diagram.PlantUmlElements
                                 .OfType<PlantUmlNamespace>()
                                 .All(element => element.Name != namespcName)))
                    {
                        _diagram.AddElement(new PlantUmlNamespace(namespc));
                    }
                }
                else
                {
                    _diagram.AddElement(new PlantUmlSlice(slice.Description));
                }

                var dependencyTargets = sliceList.Where(targetSlice =>
                    targetSlice.Description != slice.Description &&
                    slice.GetTypeDependencies().Any(type => targetSlice.Types.Contains(type)));

                _diagram.AddElements(dependencyTargets.Select(target =>
                    new PlantUmlDependency(slice.Description, target.Description, DependencyType.OneToOne)));
            }

            return this;
        }
    }
}