using System.Collections.Generic;

namespace ArchUnitNET.PlantUml
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
            int hashCode = -878893848;
            hashCode = hashCode * -1521134295 + EqualityComparer<PlantUmlComponent>.Default.GetHashCode(Component);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlantUmlComponent>.Default.GetHashCode(Target);
            return hashCode;
        }
    }
}