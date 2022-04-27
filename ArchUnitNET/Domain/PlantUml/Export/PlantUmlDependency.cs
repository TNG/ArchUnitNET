using System;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlDependency : IPlantUmlElement
    {
        public string Target { get; }
        public string Origin { get; }
        public DependencyType DependencyType { get; }

        public PlantUmlDependency(string origin, string target, DependencyType dependencyType)
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(origin, target);
            PlantUmlNameChecker.AssertNotNullOrEmpty(origin, target);
            Target = target;
            Origin = origin;
            DependencyType = dependencyType;
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            switch (DependencyType)
            {
                case DependencyType.OneToOne:
                    return "\"" + Origin + "\"" + " --|> " + "\"" + Target + "\"" + Environment.NewLine;
                case DependencyType.OneToMany:
                    return "\"" + Origin + "\"" + " \"1\" --|> \"many\" " + "\"" + Target + "\"" + " " +
                           Environment.NewLine;
            }

            return "";
        }

        private bool Equals(PlantUmlDependency other)
        {
            return Equals(Target, other.Target) && Equals(Origin, other.Origin) &&
                   Equals(DependencyType, other.DependencyType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((PlantUmlDependency)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Target.GetHashCode();
                hashCode = (hashCode * 397) ^ Origin.GetHashCode();
                hashCode = (hashCode * 397) ^ DependencyType.GetHashCode();
                return hashCode;
            }
        }
    }

    public enum DependencyType
    {
        OneToOne,
        OneToMany
    }
}