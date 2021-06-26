using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml
{
    internal class PlantUmlComponentDependency
    {
        public PlantUmlComponentDependency(PlantUmlComponent component, PlantUmlComponent target)
        {
            Component = component ?? throw new System.ArgumentNullException(nameof(component));
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
        }

        public PlantUmlComponent Component { get; }
        public PlantUmlComponent Target { get; }

        public override bool Equals(object obj)
        {
            return obj is PlantUmlComponentDependency dependency &&
                   EqualityComparer<PlantUmlComponent>.Default.Equals(Component, dependency.Component) &&
                   EqualityComparer<PlantUmlComponent>.Default.Equals(Target, dependency.Target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (Component != null ? Component.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}