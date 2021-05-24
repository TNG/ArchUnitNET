//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain.Extensions
{
    public static class DependencyExtensions
    {
        public static bool CallsMethod(this IHasDependencies type, string pattern, bool useRegularExpressions = false)
        {
            return type.GetCalledMethods().Any(member => member.FullNameMatches(pattern, useRegularExpressions));
        }

        public static IEnumerable<MethodMember> GetCalledMethods(this IHasDependencies type)
        {
            return type.Dependencies.OfType<MethodCallDependency>()
                .Select(dependency => (MethodMember) dependency.TargetMember);
        }

        public static IEnumerable<FieldMember> GetAccessedFieldMembers(this IHasDependencies type)
        {
            return type.Dependencies.OfType<AccessFieldDependency>()
                .Select(dependency => (FieldMember) dependency.TargetMember);
        }

        public static bool DependsOn(this IHasDependencies c, string pattern, bool useRegularExpressions = false)
        {
            return c.GetTypeDependencies().Any(d => d.FullNameMatches(pattern, useRegularExpressions));
        }


        public static bool OnlyDependsOn(this IHasDependencies c, string pattern, bool useRegularExpressions = false)
        {
            return c.GetTypeDependencies().All(d => d.FullNameMatches(pattern, useRegularExpressions));
        }

        public static IEnumerable<IType> GetTypeDependencies(this IHasDependencies c)
        {
            return c.Dependencies.Select(dependency => dependency.Target);
        }

        public static IEnumerable<IType> GetTypeDependencies(this IHasDependencies c, Architecture architecture)
        {
            return c.Dependencies.Select(dependency => dependency.Target).Intersect(architecture.Types);
        }

        public static IEnumerable<ITypeDependency> GetFieldTypeDependencies(this IHasDependencies type,
            bool getBackwardsDependencies = false)
        {
            return getBackwardsDependencies
                ? type.BackwardsDependencies.OfType<FieldTypeDependency>()
                : type.Dependencies.OfType<FieldTypeDependency>();
        }
    }
}