using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnitNETTests")]
namespace ArchUnitNET.PlantUml
{
    internal class PlantUmlDiagram
    {
        private PlantUmlComponents _plantUmlComponents;

        public PlantUmlDiagram(PlantUmlComponents plantUmlComponents)
        {
            _plantUmlComponents = plantUmlComponents ?? throw new System.ArgumentNullException(nameof(plantUmlComponents));
        }

        public ISet<PlantUmlComponent> AllComponents
        {
            get
            {
                return ImmutableHashSet.ToImmutableHashSet(_plantUmlComponents.AllComponents);
            }
        }

        public ISet<PlantUmlComponent> ComponentsWithAlias
        {
            get
            {
                return ImmutableHashSet.ToImmutableHashSet(_plantUmlComponents.CompomenentsWithAlias);
            }
        }
    }
}