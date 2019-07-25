/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Fluent
{
    public static class MemberExtensions
    {
        public static IEnumerable<BodyTypeMemberDependency> GetBodyTypeMemberDependencies(this IMember member)
        {
            return member.MemberDependencies.OfType<BodyTypeMemberDependency>();
        }

        public static IEnumerable<MethodCallDependency> GetMethodCallDependencies(this IMember member)
        {
            return member.MemberDependencies.OfType<MethodCallDependency>();
        }

        public static IEnumerable<ITypeDependency> GetFieldTypeDependencies(this IHasDependencies type)
        {
            return type.Dependencies.OfType<FieldTypeDependency>();
        }

        public static Attribute GetAttributeFromMember(this IMember member, Class attributeClass)
        {
            return member.Attributes.Find(attribute => attribute.Type.FullName.Equals(attributeClass.FullName));
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