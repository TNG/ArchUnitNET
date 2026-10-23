using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class PlantUmlFluentComponentDiagramInitializer
    {
        private readonly PlantUmlFluentComponentDiagramCreator _fluentComponentDiagramCreator;

        internal PlantUmlFluentComponentDiagramInitializer(
            PlantUmlFluentComponentDiagramCreator fluentComponentDiagramCreator
        )
        {
            _fluentComponentDiagramCreator = fluentComponentDiagramCreator;
        }

        public GivenPlantUmlFluentComponentDiagram WithElements(
            IEnumerable<IPlantUmlElement> elements
        )
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

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IEnumerable<IType> types,
            GenerationOptions generationOptions = null
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(types, generationOptions);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromTypes(
            IObjectProvider<IType> types,
            Architecture architecture,
            GenerationOptions generationOptions = null
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(
                types.GetObjects(architecture),
                generationOptions
            );
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from types");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            GenerationOptions generationOptions = null
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(slices, generationOptions);
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        /// <summary>
        /// Draws only the slices whose types all lie in the namespace
        /// <paramref name="focusOnPackage"/> or below it, and the slices they depend on or that
        /// depend on them; see <see cref="PlantUmlFileBuilder.WithDependenciesFromFocusOn"/>.
        /// </summary>
        /// <param name="slices">The slices to draw.</param>
        /// <param name="focusOnPackage">The namespace to focus on.</param>
        /// <param name="generationOptions">Only
        /// <see cref="GenerationOptions.NestSlicesInPackages"/> is read.</param>
        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            string focusOnPackage,
            GenerationOptions generationOptions = null
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFromFocusOn(
                slices,
                focusOnPackage,
                generationOptions
            );
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IObjectProvider<Slice> slices,
            Architecture architecture
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(
                slices.GetObjects(architecture)
            );
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from slices");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
    }
}
