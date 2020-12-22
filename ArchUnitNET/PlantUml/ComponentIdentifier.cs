using System.Collections.Generic;

namespace ArchUnitNET.PlantUml
{
    internal class ComponentIdentifier
    {
        public ComponentIdentifier(ComponentName componentName) : this(componentName, null)
        { }

        public ComponentIdentifier(ComponentName componentName, Alias alias)
        {
            ComponentName = componentName;
            Alias = alias;
        }

        public ComponentName ComponentName { get; private set; }

        public Alias Alias { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is ComponentIdentifier identifier &&
                   EqualityComparer<ComponentName>.Default.Equals(ComponentName, identifier.ComponentName) &&
                   EqualityComparer<Alias>.Default.Equals(Alias, identifier.Alias);
        }

        public override int GetHashCode()
        {
            int hashCode = -1327966062;
            hashCode = hashCode * -1521134295 + EqualityComparer<ComponentName>.Default.GetHashCode(ComponentName);
            if (Alias != null)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<Alias>.Default.GetHashCode(Alias);
            }
            return hashCode;
        }
    }
}