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
                    return "[" + Origin + "]" + " \"1\" --|> \"many\" " + "[" + Target + "]" +
                           Environment.NewLine;

                case DependencyType.OneToPackage:
                    return  "["+ Origin + "] -[#red]> " + GetChildNamespace(Target) + Environment.NewLine;

                case DependencyType.PackageToOne:
                    return GetChildNamespace(Origin) + " -[#blue]> [" + Target + "]" +Environment.NewLine;                        
                
                case DependencyType.PackageToPackage:
                    return GetChildNamespace(Origin) + " -[#green]> " + GetChildNamespace(Target) + Environment.NewLine;
                
                case DependencyType.OneToOneCompact:
                    if (OriginCountOfDots() == TargetCountOfDots())
                    {
                        return  "[" + Origin  + "] --> [" + Target + "]" + Environment.NewLine;
                    }
                    return "";
                
                case DependencyType.Circle:
                    return "[" + Origin + "]" + " <-[#red]> " + "[" + Target + "]" + Environment.NewLine;

                case DependencyType.PackageToPackageIfSameParentNamespace:
                    if (OriginCountOfDots() == TargetCountOfDots() &&
                        (OriginCountOfDots() == 0 || HaveSameParentNamespace(Origin, Target)))
                    {
                        return  GetChildNamespace(Origin) + " ..> " + GetChildNamespace(Target) + Environment.NewLine;                        
                    }
                    return "";

                case DependencyType.OneToOneIfSameParentNamespace:
                    if (OriginCountOfDots() == TargetCountOfDots() &&
                        (OriginCountOfDots() == 0 || HaveSameParentNamespace(Origin, Target))
                       )
                    {
                        return Origin + " --|> " + Target + Environment.NewLine;
                    } 
                    
                    if (OriginCountOfDots() < TargetCountOfDots())
                    {
                        var tmp = GetParentNamespace(Target);
                        while (OriginCountOfDots() < CountOfDots(tmp))
                        {
                            tmp = GetParentNamespace(tmp);
                        }

                        if (tmp != Origin && HaveSameParentNamespace(tmp, Origin))
                        {
                            return Origin + " --> " + GetChildNamespace(tmp) + Environment.NewLine;
                        }
                    }
                    else
                    {
                        var tmp = GetParentNamespace(Origin);
                        while (CountOfDots(tmp) > TargetCountOfDots())
                        {
                            tmp = GetParentNamespace(tmp);
                        }
                        
                        if (tmp != Target && HaveSameParentNamespace(tmp, Target))
                        {
                            return GetChildNamespace(tmp) + " -> " + Target + Environment.NewLine;    
                        }
                    }
                    return "";

                case DependencyType.NoDependency:
                    return "";
            }

            return "";
        }

        private static string GetParentNamespace(string ns) => 
            ns.Remove(ns.LastIndexOf(".", StringComparison.Ordinal));

        private static string GetChildNamespace(string ns) => 
            ns.Remove(0,ns.LastIndexOf(".", StringComparison.Ordinal) + 1);

        private static bool HaveSameParentNamespace(string origin, string target) => 
            (GetParentNamespace(origin) == GetParentNamespace(target));

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
        OneToOneIfSameParentNamespace,
        PackageToPackageIfSameParentNamespace,
        OneToOneCompact,
        Circle,
        NoDependency
    }
}