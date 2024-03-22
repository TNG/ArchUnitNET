using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class PlantUmlDiagramBuilder
    {
        private readonly PlantUmlComponents _plantUmlComponents;
        private Dictionary<ComponentIdentifier, IList<ParsedDependency>> _originToParsedDependency =
            new Dictionary<ComponentIdentifier, IList<ParsedDependency>>();

        public PlantUmlDiagramBuilder(PlantUmlComponents plantUmlComponents)
        {
            _plantUmlComponents = plantUmlComponents;
        }

        public PlantUmlDiagramBuilder WithDependencies(IEnumerable<ParsedDependency> dependencies)
        {
            var groupedByOrigin =
                from d in dependencies
                group d by d.Origin into g
                select (Origin: g.Key, Dependencies: g);
            foreach (var g in groupedByOrigin)
            {
                _originToParsedDependency.Add(g.Origin, g.Dependencies.Distinct().ToList());
            }
            return this;
        }

        public PlantUmlParsedDiagram Build()
        {
            foreach (PlantUmlComponent component in _plantUmlComponents.AllComponents)
            {
                Finish(component);
            }
            return new PlantUmlParsedDiagram(_plantUmlComponents);
        }

        private void Finish(PlantUmlComponent component)
        {
            var dependencies = ImmutableList.CreateBuilder<PlantUmlComponentDependency>();

            if (_originToParsedDependency.ContainsKey(component.Identifier))
            {
                foreach (
                    ParsedDependency dependencyOriginatingFromComponent in _originToParsedDependency[
                        component.Identifier
                    ]
                )
                {
                    PlantUmlComponent target = _plantUmlComponents.findComponentWith(
                        dependencyOriginatingFromComponent.Target
                    );
                    dependencies.Add(new PlantUmlComponentDependency(component, target));
                }
            }
            component.Finish(dependencies.ToImmutable());
        }
    }
}
