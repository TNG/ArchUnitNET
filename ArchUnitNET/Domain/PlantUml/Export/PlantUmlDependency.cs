using System;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlDependency : IPlantUmlElement
    {
        private string Target { get; }
        private string Origin { get; }
        private DependencyType DependencyType { get; }

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
                    return Origin + " --|> " + Target + Environment.NewLine;
                case DependencyType.OneToMany:
                    return Origin + " \"1\" --|> \"many\" " + Target + " " + Environment.NewLine;
            }

            return "";
        }
    }

    public enum DependencyType
    {
        OneToOne,
        OneToMany
    }
}