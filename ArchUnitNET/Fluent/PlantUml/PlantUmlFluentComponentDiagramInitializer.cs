using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramInitializer
    {
        internal PlantUmlFluentComponentDiagramInitializer()
        {
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(
            IEnumerable<IPlantUmlElement> elements
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithElements(elements);
            return new GivenPlantUmlFluentComponentDiagram(builder, "with custom elements");
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(params IPlantUmlElement[] elements)
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithElements(elements);
            return new GivenPlantUmlFluentComponentDiagram(builder, "with custom elements");
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IEnumerable<IType> types,
            GenerationOptions generationOptions = null
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithDependenciesFrom(types, generationOptions);
            return new GivenPlantUmlFluentComponentDiagram(builder, "with dependencies from types");
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IObjectProvider<IType> types,
            Architecture architecture,
            GenerationOptions generationOptions = null
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithDependenciesFrom(
                types.GetObjects(architecture),
                generationOptions
            );
            return new GivenPlantUmlFluentComponentDiagram(builder, "with dependencies from types");
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            GenerationOptions generationOptions = null
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithDependenciesFrom(slices, generationOptions);
            return new GivenPlantUmlFluentComponentDiagram(builder, "with dependencies from slices");
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            string focusOnPackage
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithDependenciesFromFocusOn(
                slices,
                focusOnPackage
            );
            return new GivenPlantUmlFluentComponentDiagram(builder, "with dependencies from slices");
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IObjectProvider<Slice> slices,
            Architecture architecture
        )
        {
            var builder = new PlantUmlFileBuilder();
            builder.WithDependenciesFrom(
                slices.GetObjects(architecture)
            );
            return new GivenPlantUmlFluentComponentDiagram(builder, "with dependencies from slices");
        }
    }
}
