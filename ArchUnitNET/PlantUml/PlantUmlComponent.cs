using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ArchUnitNET.PlantUml
{
    internal class PlantUmlComponent
    {
        private IList<PlantUmlComponentDependency> _dependencies = new List<PlantUmlComponentDependency>();

        public PlantUmlComponent(ComponentName componentName, ISet<Stereotype> stereotypes, Alias alias)
        {
            ComponentName = componentName ?? throw new ArgumentNullException(nameof(componentName));
            Stereotypes = stereotypes ?? throw new ArgumentNullException(nameof(stereotypes));
            Alias = alias;
        }
        public ISet<Stereotype> Stereotypes { get; private set; }

        public Alias Alias { get; private set; }

        public ComponentName ComponentName { get; private set; }

        public ComponentIdentifier Identifier
        {
            get
            {
                return Alias != null
                        ? new ComponentIdentifier(ComponentName, Alias)
                        : new ComponentIdentifier(ComponentName);
            }
        }

        internal void Finish(IList<PlantUmlComponentDependency> dependencies)
        {
            _dependencies = ImmutableList.CreateRange(dependencies);
        }
    }
}