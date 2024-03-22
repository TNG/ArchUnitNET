using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class ComponentIdentifier
    {
        public ComponentIdentifier(ComponentName componentName)
            : this(componentName, null) { }

        public ComponentIdentifier(ComponentName componentName, Alias alias)
        {
            ComponentName = componentName;
            Alias = alias;
        }

        public ComponentName ComponentName { get; private set; }

        public Alias Alias { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is ComponentIdentifier identifier
                && EqualityComparer<ComponentName>.Default.Equals(
                    ComponentName,
                    identifier.ComponentName
                )
                && EqualityComparer<Alias>.Default.Equals(Alias, identifier.Alias);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (ComponentName != null ? ComponentName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
