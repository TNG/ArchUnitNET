//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Slice<string>> SlicedBy(this IEnumerable<IType> source, string fullName)
        {
            return source.GroupBy(type => type.FullName)
                .Select(sliceItems => new Slice<string>(sliceItems.Key, sliceItems.ToList()));
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

        public static bool ResidesInNamespace(this IType e, string pattern, bool useRegularExpressions = false)
        {
            return e.Namespace.FullNameMatches(pattern, useRegularExpressions);
        }

        public static bool ResidesInAssembly(this IType e, string pattern, bool useRegularExpressions = false)
        {
            return e.Assembly.FullNameMatches(pattern, useRegularExpressions);
        }

        public static bool IsDeclaredAsFieldIn(this IType type, string pattern, bool useRegularExpressions = false)
        {
            return type.GetFieldTypeDependencies(true).Any(dependency =>
                dependency.Target.FullNameMatches(pattern, useRegularExpressions));
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