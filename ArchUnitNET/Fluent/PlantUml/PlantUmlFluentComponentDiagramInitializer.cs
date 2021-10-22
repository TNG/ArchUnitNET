//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramInitializer
    {
        private readonly PlantUmlFluentComponentDiagramCreator _fluentComponentDiagramCreator;

        internal PlantUmlFluentComponentDiagramInitializer(
            PlantUmlFluentComponentDiagramCreator fluentComponentDiagramCreator)
        {
            _fluentComponentDiagramCreator = fluentComponentDiagramCreator;
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithCustomDependencies(
            IEnumerable<PlantUmlDependency> dependencies,
            params string[] objectsWithoutDependencies)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(dependencies, objectsWithoutDependencies);
            _fluentComponentDiagramCreator.AddToDescription("with custom dependencies");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithCustomDependencies(
            IEnumerable<PlantUmlDependency> dependencies,
            IEnumerable<string> objectsWithoutDependencies)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(dependencies, objectsWithoutDependencies);
            _fluentComponentDiagramCreator.AddToDescription("with custom dependencies");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithDependenciesFromTypes(IEnumerable<IType> types,
            bool includeDependenciesToOther = false)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(types, includeDependenciesToOther);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithDependenciesFromTypes(IObjectProvider<IType> types,
            Architecture architecture, bool includeDependenciesToOther = false)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(types.GetObjects(architecture),
                includeDependenciesToOther);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithDependenciesFromSlices(IEnumerable<Slice> slices)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(slices);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenDefinedPlantUmlFluentComponentDiagram WithDependenciesFromSlices(IObjectProvider<Slice> slices,
            Architecture architecture)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(slices.GetObjects(architecture));
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenDefinedPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
    }
}