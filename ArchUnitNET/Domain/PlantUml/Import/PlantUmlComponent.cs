using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class PlantUmlComponent
    {
        private IEnumerable<PlantUmlComponentDependency> _dependencies;

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

        public IList<PlantUmlComponent> Dependencies
        {
            get { return _dependencies.Select(d => d.Target).ToList(); }
        }

        public override bool Equals(object obj)
        {
            return obj is PlantUmlComponent component &&
                   Stereotypes.SequenceEqual(component.Stereotypes) &&
                   EqualityComparer<Alias>.Default.Equals(Alias, component.Alias) &&
                   EqualityComparer<ComponentName>.Default.Equals(ComponentName, component.ComponentName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (Alias != null ? Alias.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ComponentName != null ? ComponentName.GetHashCode() : 0);

                return Stereotypes.Aggregate(hashCode,
                    (current, element) => (current * 397) ^ (element != null ? element.GetHashCode() : 0));
            }
        }

        internal void Finish(IList<PlantUmlComponentDependency> dependencies)
        {
            _dependencies = ImmutableList.CreateRange(dependencies);
        }
    }
}