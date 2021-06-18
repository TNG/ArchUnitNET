using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnitNETTests")]
namespace ArchUnitNET.Domain.PlantUml
{
    internal class PlantUmlDiagram
    {
        private PlantUmlComponents _plantUmlComponents;

        public PlantUmlDiagram(PlantUmlComponents plantUmlComponents)
        {
            _plantUmlComponents = plantUmlComponents ?? throw new ArgumentNullException(nameof(plantUmlComponents));
        }

        public ISet<PlantUmlComponent> AllComponents
        {
            get
            {
                return _plantUmlComponents.AllComponents.ToImmutableHashSet();
            }
        }

        public ISet<PlantUmlComponent> ComponentsWithAlias
        {
            get
            {
                return _plantUmlComponents.CompomenentsWithAlias.ToImmutableHashSet();
            }
        }
    }
}