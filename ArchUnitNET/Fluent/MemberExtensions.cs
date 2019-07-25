/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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