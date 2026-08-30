using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramInitializer
    {
        private readonly PlantUmlFileBuilder _builder = new PlantUmlFileBuilder();

        internal PlantUmlFluentComponentDiagramInitializer()
        {
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(
            IEnumerable<IPlantUmlElement> elements
        )
        {
            _builder.WithElements(elements);
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(params IPlantUmlElement[] elements)
        {
            _builder.WithElements(elements);
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IEnumerable<IType> types,
            GenerationOptions generationOptions = null
        )
        {
            _builder.WithDependenciesFrom(types, generationOptions);
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IObjectProvider<IType> types,
            Architecture architecture,
            GenerationOptions generationOptions = null
        )
        {
            _builder.WithDependenciesFrom(
                types.GetObjects(architecture),
                generationOptions
            );
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            GenerationOptions generationOptions = null
        )
        {
            _builder.WithDependenciesFrom(slices, generationOptions);
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            string focusOnPackage
        )
        {
            _builder.WithDependenciesFromFocusOn(
                slices,
                focusOnPackage
            );
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IObjectProvider<Slice> slices,
            Architecture architecture
        )
        {
            _builder.WithDependenciesFrom(
                slices.GetObjects(architecture)
            );
            return new GivenPlantUmlFluentComponentDiagram(_builder);
        }
    }
}
