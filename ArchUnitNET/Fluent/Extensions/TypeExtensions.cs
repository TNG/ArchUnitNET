/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Fluent.Extensions
{
    public static class TypeExtensions
    {
        public static bool CallsMethod(this IHasDependencies type, MethodMember method)
        {
            return type.GetCalledMethods().Contains(type);
        }

        public static bool CallsMethod(this IHasDependencies type, string pattern, bool useRegularExpressions = false)
        {
            return type.GetCalledMethods().Any(member => member.FullNameMatches(pattern, useRegularExpressions));
        }

        public static bool CallsAnyMethodFromType(this IHasDependencies type, string pattern,
            bool useRegularExpressions = false)
        {
            return type.GetMethodCallDependencies().Select(dependency => dependency.Target)
                .Any(member => member.FullNameMatches(pattern, useRegularExpressions));
        }

        public static IEnumerable<IType> GetAssignableTypes(this IType type)
        {
            switch (type)
            {
                case Interface intf:
                    return intf.ImplementedInterfaces.Append(intf);
                case Class cls:
                    return cls.InheritedClasses.Append(cls).Concat(cls.ImplementedInterfaces);
                default:
                    return Enumerable.Empty<IType>();
            }
        }

        public static IEnumerable<MethodMember> GetCalledMethods(this IHasDependencies type)
        {
            return type.GetMethodCallDependencies().Select(dependency => (MethodMember) dependency.TargetMember);
        }

        public static IEnumerable<PropertyMember> GetPropertyMembersWithName(this IType type, string name)
        {
            return type.GetPropertyMembers().WhereNameIs(name);
        }

        public static bool HasPropertyMemberWithName(this IType type, string name)
        {
            return !type.GetPropertyMembersWithName(name).IsNullOrEmpty();
        }

        public static IEnumerable<FieldMember> GetFieldMembersWithName(this IType type, string name)
        {
            return type.GetFieldMembers().WhereNameIs(name);
        }

        public static bool HasFieldMemberWithName(this IType type, string name)
        {
            return !type.GetFieldMembersWithName(name).IsNullOrEmpty();
        }

        public static IEnumerable<MethodMember> GetMethodMembersWithName(this IType type, string name)
        {
            return type.GetMethodMembers().WhereNameIs(name);
        }

        public static bool HasMethodMemberWithName(this IType type, string name)
        {
            return !type.GetMethodMembersWithName(name).IsNullOrEmpty();
        }

        public static IEnumerable<IMember> GetMembersWithName(this IType type, string name)
        {
            return type.Members.WhereNameIs(name);
        }

        public static bool HasMemberWithName(this IType type, string name)
        {
            return !type.GetMembersWithName(name).IsNullOrEmpty();
        }

        public static PropertyMember GetPropertyMemberWithFullName(this IType type, string fullName)
        {
            return type.GetPropertyMembers().WhereFullNameIs(fullName);
        }

        public static bool HasPropertyMemberWithFullName(this IType type, string fullname)
        {
            return type.GetPropertyMemberWithFullName(fullname) != null;
        }

        public static MethodMember GetMethodMemberWithFullName(this IType type, string fullName)
        {
            return type.GetMethodMembers().WhereFullNameIs(fullName);
        }

        public static bool HasMethodMemberWithFullName(this IType type, string fullname)
        {
            return type.GetMethodMemberWithFullName(fullname) != null;
        }

        public static FieldMember GetFieldMemberWithFullName(this IType type, string fullName)
        {
            return type.GetFieldMembers().WhereFullNameIs(fullName);
        }

        public static bool HasFieldMemberWithFullName(this IType type, string fullname)
        {
            return type.GetFieldMemberWithFullName(fullname) != null;
        }

        public static IMember GetMemberWithFullName(this IType type, string fullName)
        {
            return type.Members.WhereFullNameIs(fullName);
        }

        public static bool HasMemberWithFullName(this IType type, string fullname)
        {
            return type.GetMemberWithFullName(fullname) != null;
        }

        public static Attribute GetAttributeOfType(this IType type, Class attributeClass)
        {
            return type.Attributes.Find(attribute => attribute.FullName.Equals(attributeClass.FullName));
        }

        public static bool NameEndsWith(this IHasName cls, string pattern)
        {
            return cls.Name.ToLower().EndsWith(pattern.ToLower());
        }

        public static bool NameStartsWith(this IHasName cls, string pattern)
        {
            return cls.Name.ToLower().StartsWith(pattern.ToLower());
        }

        public static bool NameContains(this IHasName cls, string pattern)
        {
            return pattern != null && cls.Name.ToLower().Contains(pattern.ToLower());
        }

        public static bool NameMatches(this IHasName cls, string pattern, bool useRegularExpressions = false)
        {
            if (useRegularExpressions)
            {
                return pattern != null && Regex.IsMatch(cls.Name, pattern);
            }

            return cls.NameContains(pattern);
        }

        public static bool FullNameMatches(this IHasName cls, string pattern, bool useRegularExpressions = false)
        {
            if (useRegularExpressions)
            {
                return pattern != null && Regex.IsMatch(cls.FullName, pattern);
            }

            return cls.FullNameContains(pattern);
        }

        public static bool FullNameContains(this IHasName cls, string pattern)
        {
            return pattern != null && cls.FullName.ToLower().Contains(pattern.ToLower());
        }

        public static bool ResidesInNamespace(this IType e, string pattern, bool useRegularExpressions = false)
        {
            return e.Namespace.FullNameMatches(pattern, useRegularExpressions);
        }

        public static bool ResidesInAssembly(this IType e, string pattern, bool useRegularExpressions = false)
        {
            return e.Assembly.FullNameMatches(pattern, useRegularExpressions);
        }

        public static bool DependsOn(this IHasDependencies c, string pattern, bool useRegularExpressions = false)
        {
            return c.GetTypeDependencies().Any(d => d.FullNameMatches(pattern, useRegularExpressions));
        }

        public static bool DependsOn(this IHasDependencies c, IType type)
        {
            return c.GetTypeDependencies().Contains(type);
        }

        public static bool IsDeclaredAsFieldIn(this IType type, string pattern, bool useRegularExpressions = false)
        {
            return type.GetFieldTypeDependencies(true).Any(dependency =>
                dependency.Target.FullNameMatches(pattern, useRegularExpressions));
        }

        public static bool OnlyDependsOn(this IHasDependencies c, string pattern, bool useRegularExpressions = false)
        {
            return c.GetTypeDependencies().All(d => d.FullNameMatches(pattern, useRegularExpressions));
        }

        public static IEnumerable<Class> GetClassDependencies(this IHasDependencies c)
        {
            return c.GetTypeDependencies().OfType<Class>();
        }

        public static IEnumerable<Interface> GetInterfaceDependencies(this IHasDependencies c)
        {
            return c.GetTypeDependencies().OfType<Interface>();
        }

        public static IEnumerable<IType> GetTypeDependencies(this IHasDependencies c)
        {
            return c.Dependencies.Select(dependency => dependency.Target);
        }

        public static bool HasDependency(this IType type, ITypeDependency dependency)
        {
            return type.Dependencies.Contains(dependency);
        }

        public static bool HasDependencies(this IType type, IEnumerable<ITypeDependency> dependencies)
        {
            return dependencies.All(dependency => type.Dependencies.Contains(dependency));
        }

        public static IEnumerable<PropertyMember> GetPropertyMembers(this IType type)
        {
            return type.Members.OfType<PropertyMember>();
        }

        public static IEnumerable<FieldMember> GetFieldMembers(this IType type)
        {
            return type.Members.OfType<FieldMember>();
        }

        public static IEnumerable<MethodMember> GetMethodMembers(this IType type)
        {
            return type.Members.OfType<MethodMember>();
        }

        public static IEnumerable<MethodMember> GetConstructors(this IType type)
        {
            return type.GetMethodMembers().Where(method => method.IsConstructor());
        }

        public static IEnumerable<MethodCallDependency> GetMethodCallDependencies(this IHasDependencies type,
            bool getBackwardsDependencies = false)
        {
            return getBackwardsDependencies
                ? type.BackwardsDependencies.OfType<MethodCallDependency>()
                : type.Dependencies.OfType<MethodCallDependency>();
        }

        public static IEnumerable<AttributeTypeDependency> GetAttributeTypeDependencies(this IType type,
            bool getBackwardsDependencies = false)
        {
            return getBackwardsDependencies
                ? type.BackwardsDependencies.OfType<AttributeTypeDependency>()
                : type.Dependencies.OfType<AttributeTypeDependency>();
        }

        public static IEnumerable<ImplementsInterfaceDependency> GetImplementsInterfaceDependencies(this IType type,
            bool getBackwardsDependencies = false)
        {
            return getBackwardsDependencies
                ? type.BackwardsDependencies.OfType<ImplementsInterfaceDependency>()
                : type.Dependencies.OfType<ImplementsInterfaceDependency>();
        }

        public static IEnumerable<InheritsBaseClassDependency> GetInheritsBaseClassDependencies(this IType type,
            bool getBackwardsDependencies = false)
        {
            return getBackwardsDependencies
                ? type.BackwardsDependencies.OfType<InheritsBaseClassDependency>()
                : type.Dependencies.OfType<InheritsBaseClassDependency>();
        }
    }
}