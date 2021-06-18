using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml
{
    internal class ParsedDependency
    {
        public ParsedDependency(ComponentIdentifier origin, ComponentIdentifier target)
        {
            Origin = origin;
            Target = target;
        }

        public ComponentIdentifier Origin { get; }
        public ComponentIdentifier Target { get; }

        public override bool Equals(object obj)
        {
            return obj is ParsedDependency dependency &&
                   EqualityComparer<ComponentIdentifier>.Default.Equals(Origin, dependency.Origin) &&
                   EqualityComparer<ComponentIdentifier>.Default.Equals(Target, dependency.Target);
        }

        public override int GetHashCode()
        {
            int hashCode = -1013312977;
            if (Origin != null)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<ComponentIdentifier>.Default.GetHashCode(Origin);
            }
            if (Target != null)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<ComponentIdentifier>.Default.GetHashCode(Target);
            }
            return hashCode;
        }
    }
}