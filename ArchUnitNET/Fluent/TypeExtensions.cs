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
using ArchUnitNET.Domain.Dependencies.Types;

namespace ArchUnitNET.Fluent
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyMember> GetPropertyMembersWithName(this IType type, string name)
        {
            return type.Members.OfType<PropertyMember>().WhereNameIs(name);
        }

        public static IEnumerable<FieldMember> GetFieldMembersWithName(this IType type, string name)
        {
            return type.Members.OfType<FieldMember>().WhereNameIs(name);
        }

        public static IEnumerable<MethodMember> GetMethodMembersWithName(this IType type, string name)
        {
            return type.Members.OfType<MethodMember>().WhereNameIs(name);
        }

        public static MethodMember GetMethodMemberWithFullName(this IType type, string fullName)
        {
            return type.Members.OfType<MethodMember>().WhereFullNameIs(fullName);
        }

        public static IEnumerable<IMember> GetMembersWithName(this IType type, string name)
        {
            return type.Members.WhereNameIs(name);
        }

        public static IMember GetMemberWithFullName(this IType type, string fullName)
        {
            return type.Members.WhereFullNameIs(fullName);
        }

        public static Attribute GetAttributeOfType(this IType type, Class attributeClass)
        {
            return type.Attributes.Find(attribute => attribute.Type.FullName.Equals(attributeClass.FullName));
        }

        public static bool NameEndsWith(this IHasName cls, string pattern)
        {
            return cls.Name.ToLower().EndsWith(pattern.ToLower());
        }

        public static bool ResidesInNamespace(this Class e, string pattern)
        {
            return e.Namespace.FullName.ToLower().Contains(pattern.ToLower());
        }

        public static bool DependsOn(this IHasDependencies c, string pattern)
        {
            return c.Dependencies.Exists(d => d.Target.FullName.ToLower().Contains(pattern.ToLower()));
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

        public static IEnumerable<AttributeTypeDependency> GetAttributeTypeDependencies(this IType type)
        {
            return type.Dependencies.OfType<AttributeTypeDependency>();
        }
        
        public static IEnumerable<ImplementsInterfaceDependency> GetImplementsInterfaceDependencies(this IType type)
        {
            return type.Dependencies.OfType<ImplementsInterfaceDependency>();
        }

        public static IEnumerable<InheritsBaseClassDependency> GetInheritsBaseClassDependencies(this IType type)
        {
            return type.Dependencies.OfType<InheritsBaseClassDependency>();
        }
    }
}