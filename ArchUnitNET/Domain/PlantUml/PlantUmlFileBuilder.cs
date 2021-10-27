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
using System.Text;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using ArchUnitNET.Fluent.Exceptions;

namespace ArchUnitNET.Domain.PlantUml
{
    public class PlantUmlFileBuilder
    {
        private IEnumerable<PlantUmlDependency> _dependencies;
        private IEnumerable<string> _objectsWithoutDependencies = Enumerable.Empty<string>();
        private readonly List<string> _builtUml = new List<string>();
        private static readonly string[] ForbiddenCharacters = { "[", "]", "\r", "\n", "\f", "\a", "\b", "\v" };
        private bool _isBuilt;

        public PlantUmlFileBuilder Build()
        {
            _isBuilt = false;
            _builtUml.Clear();
            var objectsWithoutDependencies = _objectsWithoutDependencies.Where(o => !string.IsNullOrEmpty(o)).ToList();
            objectsWithoutDependencies.ForEach(CheckComponentName);
            var dependencies = _dependencies
                .Where(dep => !string.IsNullOrEmpty(dep.Origin) && !string.IsNullOrEmpty(dep.Target)).ToList();
            dependencies.ForEach(d => CheckComponentName(d.Origin), d => CheckComponentName(d.Target));

            _builtUml.Add("@startuml");
            _builtUml.AddRange(objectsWithoutDependencies.Select(obj => "[" + obj + "]"));
            _builtUml.AddRange(
                dependencies.Select(dependency => "[" + dependency.Origin + "] --> [" + dependency.Target + "]"));
            _builtUml.Add("@enduml");
            _isBuilt = true;
            return this;
        }

        public string AsString()
        {
            if (!_isBuilt)
            {
                throw new UmlNotBuiltException("The uml must be built first.");
            }

            var umlSb = new StringBuilder();
            foreach (var line in _builtUml)
            {
                umlSb.AppendLine(line);
            }

            return umlSb.ToString();
        }

        public List<string> AsLineList()
        {
            if (!_isBuilt)
            {
                throw new UmlNotBuiltException("The uml must be built first.");
            }

            return _builtUml;
        }

        public void WriteToFile(string path, bool overwrite = true)
        {
            if (!_isBuilt)
            {
                throw new UmlNotBuiltException("The uml must be built first.");
            }

            if (!overwrite && File.Exists(path))
            {
                throw new FileAlreadyExistsException("File already exists and overwriting is disabled.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path."));

            using (var sw = File.CreateText(path))
            {
                foreach (var line in _builtUml)
                {
                    sw.WriteLine(line);
                }
            }
        }

        private static void CheckComponentName(string name)
        {
            if (ForbiddenCharacters.Any(name.Contains))
            {
                throw new IllegalComponentNameException(
                    "PlantUml component names must not contain \"[\" or \"]\" or any of the escape characters \"\\r\", \"\\n\", \"\\f\", \"\\a\", \"\\b\", \"\\v\".");
            }
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<PlantUmlDependency> dependencies,
            params string[] objectsWithoutDependencies)
        {
            _dependencies = dependencies;
            _objectsWithoutDependencies = objectsWithoutDependencies;
            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<PlantUmlDependency> dependencies,
            IEnumerable<string> objectsWithoutDependencies)
        {
            _dependencies = dependencies;
            _objectsWithoutDependencies = objectsWithoutDependencies;
            return this;
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<IType> types,
            bool includeDependenciesToOther = false)
        {
            return WithDependenciesFrom(types, type => type.FullName, type => type.FullName,
                includeDependenciesToOther);
        }

        public PlantUmlFileBuilder WithDependenciesFrom(IEnumerable<Slice> slices)
        {
            return WithDependenciesFrom(slices, slice => slice.Description,
                type =>
                    (from slice in slices where slice.Types.Contains(type) select slice.Description).FirstOrDefault());
        }

        public PlantUmlFileBuilder WithDependenciesFrom<T>(IEnumerable<T> objects,
            Func<T, string> identifierSelectorForObjects, Func<IType, string> identifierSelectorForTargetTypes,
            bool includeDependenciesToOther = false) where T : IHasDependencies
        {
            var processedDependencies = new List<PlantUmlDependency>();
            var objectsWithoutDependencies = new List<string>();
            var groupedObjects = objects.GroupBy(identifierSelectorForObjects);

            foreach (var group in groupedObjects)
            {
                var targets = group.SelectMany(o => o.Dependencies)
                    .Select(dep => identifierSelectorForTargetTypes(dep.Target))
                    .Distinct().Where(t => t != group.Key);

                if (!includeDependenciesToOther)
                {
                    targets = targets.Where(t => groupedObjects.Select(g => g.Key).Contains(t));
                }

                var plantUmlDependencies = targets.Select(t => new PlantUmlDependency(group.Key, t)).ToList();
                processedDependencies.AddRange(plantUmlDependencies);

                if (!plantUmlDependencies.Any())
                {
                    objectsWithoutDependencies.Add(group.Key);
                }
            }

            _dependencies = processedDependencies;
            _objectsWithoutDependencies = objectsWithoutDependencies;
            return this;
        }
    }
}