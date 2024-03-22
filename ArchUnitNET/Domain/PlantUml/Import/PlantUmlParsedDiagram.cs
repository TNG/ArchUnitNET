using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnitNETTests")]

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class PlantUmlParsedDiagram
    {
        private PlantUmlComponents _plantUmlComponents;

        public PlantUmlParsedDiagram(PlantUmlComponents plantUmlComponents)
        {
            _plantUmlComponents =
                plantUmlComponents ?? throw new ArgumentNullException(nameof(plantUmlComponents));
        }

        public ISet<PlantUmlComponent> AllComponents
        {
            get { return _plantUmlComponents.AllComponents.ToImmutableHashSet(); }
        }

        public ISet<PlantUmlComponent> ComponentsWithAlias
        {
            get { return _plantUmlComponents.CompomenentsWithAlias.ToImmutableHashSet(); }
        }
    }
}
