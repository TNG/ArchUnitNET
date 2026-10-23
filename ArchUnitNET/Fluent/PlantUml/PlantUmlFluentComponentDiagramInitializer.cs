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

        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromSlices(
            IEnumerable<Slice> slices,
            string focusOnPackage
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFromFocusOn(
                slices,
                focusOnPackage
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

        /// <summary>
        /// Draws the given namespaces as nested PlantUML namespaces, with an arrow from one
        /// namespace to another wherever one of its types depends on one of the other's; see
        /// <see cref="PlantUmlFileBuilder.WithDependenciesFrom(IEnumerable{Namespace}, GenerationOptions)"/>.
        /// </summary>
        /// <param name="namespaces">The namespaces to draw, e.g. <see cref="Architecture.Namespaces"/>.</param>
        /// <param name="generationOptions">Filters and layout options; the defaults if <c>null</c>.
        /// <see cref="GenerationOptions.C4Style"/> does not apply to namespaces.</param>
        public GivenPlantUmlFluentComponentDiagram WithDependenciesFromNamespaces(
            IEnumerable<Namespace> namespaces,
            GenerationOptions generationOptions = null
        )
        {
            _fluentComponentDiagramCreator.Builder.WithDependenciesFrom(
                namespaces,
                generationOptions
            );
            _fluentComponentDiagramCreator.AddToDescription("with dependencies from namespaces");
            return new GivenPlantUmlFluentComponentDiagram(_fluentComponentDiagramCreator);
        }
    }
}
