/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Fluent.Extensions
{
    public static class MemberExtensions
    {
        public static IEnumerable<BodyTypeMemberDependency> GetBodyTypeMemberDependencies(this IMember member)
        {
            return member.MemberDependencies.OfType<BodyTypeMemberDependency>();
        }

        public static bool HasBodyTypeMemberDependencies(this IMember member)
        {
            return member.GetBodyTypeMemberDependencies().Any();
        }

        public static bool HasBodyTypeMemberDependencies(this IMember member, string pattern)
        {
            return member.GetBodyTypeMemberDependencies().Any(dependency =>
                dependency.Origin.NameContains(pattern) || dependency.Target.NameContains(pattern));
        }

        public static IEnumerable<MethodCallDependency> GetMethodCallDependencies(this IMember member)
        {
            return member.MemberDependencies.OfType<MethodCallDependency>();
        }

        public static bool HasMethodCallDependencies(this IMember member)
        {
            return member.GetMethodCallDependencies().Any();
        }

        public static bool HasMethodCallDependencies(this IMember member, string pattern)
        {
            return member.GetMethodCallDependencies().Any(dependency =>
                dependency.Origin.NameContains(pattern) || dependency.Target.NameContains(pattern));
        }

        public static IEnumerable<ITypeDependency> GetFieldTypeDependencies(this IHasDependencies type)
        {
            return type.Dependencies.OfType<FieldTypeDependency>();
        }

        public static bool HasFieldTypeDependencies(this IMember member)
        {
            return member.GetFieldTypeDependencies().Any();
        }

        public static bool HasFieldTypeDependencies(this IMember member, string pattern)
        {
            return member.GetFieldTypeDependencies().Any(dependency =>
                dependency.Origin.NameContains(pattern) || dependency.Target.NameContains(pattern));
        }

        public static Attribute GetAttributeFromMember(this IMember member, Class attributeClass)
        {
            return member.Attributes.Find(attribute => attribute.FullName.Equals(attributeClass.FullName));
        }

        public static IEnumerable<Attribute> GetAttributes(this IMember member)
        {
            return member.Dependencies.OfType<AttributeMemberDependency>().Select(dependency => dependency.Target)
                .OfType<Attribute>();
        }

        public static bool HasMethodSignatureDependency(this IMember member,
            MethodSignatureDependency methodSignatureDependency)
        {
            return member.MemberDependencies.OfType<MethodSignatureDependency>().Contains(methodSignatureDependency);
        }

        public static bool HasMemberDependency(this IMember member,
            IMemberTypeDependency memberDependency)
        {
            return member.MemberDependencies.Contains(memberDependency);
        }

        public static bool HasDependency(this IMember member,
            IMemberTypeDependency memberDependency)
        {
            return member.Dependencies.Contains(memberDependency);
        }

        public static bool IsConstructor(this MethodMember methodMember)
        {
            return methodMember.MethodForm == MethodForm.Constructor;
        }
    }
}