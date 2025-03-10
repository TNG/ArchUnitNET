using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public static class DependencyFilters
    {
        public static Func<ITypeDependency, bool> IgnoreDependenciesToParents()
        {
            return dependency =>
                OriginAndTargetHaveNoParents(dependency)
                || !dependency.Origin.FullName.StartsWith(dependency.Target.FullName);
        }

        public static Func<ITypeDependency, bool> IgnoreDependenciesToChildren()
        {
            return dependency =>
                OriginAndTargetHaveNoParents(dependency)
                || !dependency.Target.FullName.StartsWith(dependency.Origin.FullName);
        }

        public static Func<ITypeDependency, bool> IgnoreDependenciesToChildrenAndParents()
        {
            return dependency =>
                OriginAndTargetHaveNoParents(dependency)
                || !dependency.Target.FullName.StartsWith(dependency.Origin.FullName)
                    && !dependency.Origin.FullName.StartsWith(dependency.Target.FullName);
        }

        private static bool OriginAndTargetHaveNoParents(ITypeDependency dependency)
        {
            return !dependency.Origin.FullNameContains(".")
                && !dependency.Target.FullNameContains(".");
        }

        public static Func<ITypeDependency, bool> FocusOn(IType type)
        {
            return dependency => dependency.Target.Equals(type) ^ dependency.Origin.Equals(type);
        }

        public static Func<ITypeDependency, bool> FocusOn(IEnumerable<IType> types)
        {
            return dependency =>
            {
                var typeList = types.ToList();
                return typeList.Contains(dependency.Target) ^ typeList.Contains(dependency.Origin);
            };
        }

        public static Func<ITypeDependency, bool> HasOrigin(IType type)
        {
            return dependency => dependency.Origin.Equals(type);
        }

        public static Func<ITypeDependency, bool> HasOrigin(IEnumerable<IType> types)
        {
            return dependency => types.Contains(dependency.Origin);
        }

        public static Func<ITypeDependency, bool> HasTarget(IType type)
        {
            return dependency => dependency.Target.Equals(type);
        }

        public static Func<ITypeDependency, bool> HasTarget(IEnumerable<IType> types)
        {
            return dependency => types.Contains(dependency.Target);
        }
    }
}
