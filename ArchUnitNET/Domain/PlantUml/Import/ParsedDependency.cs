using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml.Import
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
            return obj is ParsedDependency dependency
                && EqualityComparer<ComponentIdentifier>.Default.Equals(Origin, dependency.Origin)
                && EqualityComparer<ComponentIdentifier>.Default.Equals(Target, dependency.Target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (Origin != null ? Origin.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
