//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

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

        public GivenPlantUmlFluentComponentDiagram WithElements(IEnumerable<IPlantUmlElement> elements)
        {
            _fluentComponentDiagramCreator.Builder.WithElements(elements);
            _fluentComponentDiagramCreator.AddToDescription("with custom elements");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(params IPlantUmlElement[] elements)
        {
            _fluentComponentDiagramCreator.Builder.WithElements(elements);
            _fluentComponentDiagramCreator.AddToDescription("with custom elements");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(IEnumerable<IType> types,
            GenerationOptions generationOptions = null)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(types, generationOptions);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(IObjectProvider<IType> types,
            Architecture architecture, GenerationOptions generationOptions = null)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(types.GetObjects(architecture),
                generationOptions);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(IEnumerable<Slice> slices, GenerationOptions generationOptions = null)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(slices, generationOptions);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
        
        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(IEnumerable<Slice> slices, string focusOnPackage)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFromFocusOn(slices, focusOnPackage);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
        
        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(IObjectProvider<Slice> slices,
            Architecture architecture)
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(slices.GetObjects(architecture));
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
    }
}