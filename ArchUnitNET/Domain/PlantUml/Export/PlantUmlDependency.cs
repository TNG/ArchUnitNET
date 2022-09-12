using System;
using System.Linq;

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

        private static int CountOfDots(string str)
        {
            return str.Count(c => c == '.');
        }

        public int OriginCountOfDots()
        {
            return CountOfDots(Origin);
        }

        public int TargetCountOfDots()
        {
            return CountOfDots(Target);
        }

        public string GetPlantUmlString(RenderOptions renderOptions = null)
        {
            switch (DependencyType)
            {
                case DependencyType.OneToOne:
                    return "[" + Origin + "]" + " --|> " + "[" + Target + "]" + Environment.NewLine;

                case DependencyType.OneToMany:
                    return "[" + Origin + "]" + " \"1\" --|> \"many\" " + "[" + Target + "]" + " " +
                           Environment.NewLine;

                case DependencyType.OneToOneIfSimilarNamespace:
                    if (OriginCountOfDots() == TargetCountOfDots() &
                        (OriginCountOfDots() == 0 |
                         Origin.Remove(Origin.LastIndexOf(".", StringComparison.Ordinal)) ==
                         Target.Remove(Target.LastIndexOf(".", StringComparison.Ordinal)))
                       )
                    {
                        return Origin + " --|> " + Target + Environment.NewLine;
                    }

                    if (OriginCountOfDots() < TargetCountOfDots())
                    {
                        var tmp = Target.Remove(Target.LastIndexOf(".", StringComparison.Ordinal));
                        while (OriginCountOfDots() < CountOfDots(tmp))
                        {
                            tmp = tmp.Remove(tmp.LastIndexOf(".", StringComparison.Ordinal));
                        }

                        if (tmp != Origin)
                        {
                            if (tmp.Remove(tmp.LastIndexOf(".", StringComparison.Ordinal)) ==
                                Origin.Remove(Origin.LastIndexOf(".", StringComparison.Ordinal)))
                            {
                                return Origin + " --> " +
                                       tmp.Remove(0, tmp.LastIndexOf(".", StringComparison.Ordinal) + 1)
                                       + Environment.NewLine;
                            }
                        }
                    }

                    if (OriginCountOfDots() > TargetCountOfDots())
                    {
                        var tmp = Origin.Remove(Origin.LastIndexOf(".", StringComparison.Ordinal));
                        while (CountOfDots(tmp) > TargetCountOfDots())
                        {
                            tmp = tmp.Remove(tmp.LastIndexOf(".", StringComparison.Ordinal));
                        }
                        
                        if (tmp != Target)
                        {
                            if (tmp.Remove(tmp.LastIndexOf(".", StringComparison.Ordinal)) == 
                                 Target.Remove(Target.LastIndexOf(".", StringComparison.Ordinal)))
                            {
                                return tmp.Remove(0, tmp.LastIndexOf(".", StringComparison.Ordinal) + 1) 
                                       + " -> " + Target + Environment.NewLine;    
                            }   
                        }
                    }
                    return "";
                
                case DependencyType.PackageToPackageIfSimilarNamespace:
                    if (OriginCountOfDots() == TargetCountOfDots() &
                        (OriginCountOfDots() == 0 | 
                         Origin.Remove(Origin.LastIndexOf(".", StringComparison.Ordinal)) == 
                         Target.Remove(Target.LastIndexOf(".", StringComparison.Ordinal)))
                        )
                    {
                        return  Origin.Remove(0, Origin.LastIndexOf(".", StringComparison.Ordinal) + 1) 
                                + " ..> " + 
                                Target.Remove(0, Target.IndexOf(".", StringComparison.Ordinal) + 1) + Environment.NewLine;                        
                    }
                    return "";
                
                case DependencyType.OneToPackage:
                 return  "["+ Origin + "] -[#red]> " + 
                             Target.Remove(0, Target.LastIndexOf(".", StringComparison.Ordinal) + 1) 
                             + Environment.NewLine;

                case DependencyType.PackageToOne:
                    return  Origin.Remove(0, Origin.LastIndexOf(".", StringComparison.Ordinal) + 1) 
                            + " -[#blue]> [" + Target + "]" +Environment.NewLine;                        
                
                case DependencyType.PackageToPackage:
                    return  Origin.Remove(0, Origin.LastIndexOf(".", StringComparison.Ordinal) + 1)
                            + " -[#green]> " + 
                            Target.Remove(0, Target.LastIndexOf(".", StringComparison.Ordinal) + 1) 
                            + Environment.NewLine;
                
                case DependencyType.OneToOneCompact:
                    if (OriginCountOfDots() == TargetCountOfDots())
                    {
                        return  "[" + Origin  + "] --> [" + Target + "]" + Environment.NewLine;
                    }
                    return "";
                
                case DependencyType.Circle:
                    return "[" + Origin + "]" + " <-[#red]> " + "[" + Target + "]" + Environment.NewLine;
                
                case DependencyType.NoDependency:
                    return "";
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
        OneToMany,
        OneToPackage,
        PackageToOne,
        PackageToPackage,
        OneToOneIfSimilarNamespace,
        PackageToPackageIfSimilarNamespace,
        OneToOneCompact,
        Circle,
        NoDependency
    }
}