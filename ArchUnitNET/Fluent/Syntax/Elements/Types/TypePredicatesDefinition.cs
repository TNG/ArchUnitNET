using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypePredicatesDefinition<T> where T : IType
    {
        public static ArchitectureObjectFilter<T> Are(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return architecture.GetTypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Filter, description);
        }

        public static ArchitectureObjectFilter<T> Are(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).Any(type => type.Equals(ruleType));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "do not exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are \"" + firstType.FullName + "\"", (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitectureObjectFilter<T>(Filter, description);
        }

        public static ObjectFilter<T> AreAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "are assignable to types with full name matching \"" + pattern + "\"";
            return new ObjectFilter<T>(type => type.IsAssignableTo(pattern), description);
        }

        public static ObjectFilter<T> AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.IsAssignableTo(firstType) || moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return ruleType.IsAssignableTo(architecture.GetTypeOfType(firstType)) ||
                       moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetTypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are assignable to " + objectProvider.Description;
            return new ArchitectureObjectFilter<T>(Condition, description);
        }

        public static ObjectFilter<T> AreAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return typeList.Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are assignable to no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureObjectFilter<T>(Condition, description);
        }

        public static ObjectFilter<T> ImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new ObjectFilter<T>(type => type.Implements(pattern),
                "implement interface with full name matching \"" + pattern + "\"");
        }

        public static ObjectFilter<T> ResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new ObjectFilter<T>(type => type.ResidesInNamespace(pattern),
                "reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HavePropertyMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
        }

        public static ObjectFilter<T> HaveFieldMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> HaveMethodMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> HaveMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasMemberWithName(name), "have member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> AreNested()
        {
            return new ObjectFilter<T>(type => type.IsNested, "are nested");
        }


        //Negations


        public static ArchitectureObjectFilter<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return !architecture.GetTypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are not \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Filter, description);
        }

        public static ArchitectureObjectFilter<T> AreNot(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Filter(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).All(type => !type.Equals(ruleType));
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are not \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitectureObjectFilter<T>(Filter, description);
        }

        public static ObjectFilter<T> AreNotAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "are not assignable to types with full name matching \"" + pattern + "\"";
            return new ObjectFilter<T>(type => !type.IsAssignableTo(pattern), description);
        }

        public static ObjectFilter<T> AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.IsAssignableTo(firstType) && !moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !ruleType.IsAssignableTo(architecture.GetTypeOfType(firstType)) &&
                       !moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetTypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreNotAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are not assignable to " + objectProvider.Description;
            return new ArchitectureObjectFilter<T>(Condition, description);
        }

        public static ObjectFilter<T> AreNotAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return !typeList.Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "are not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ObjectFilter<T>(Condition, description);
        }

        public static ArchitectureObjectFilter<T> AreNotAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetTypeOfType).Any(ruleType.IsAssignableTo);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not assignable to no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "are not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureObjectFilter<T>(Condition, description);
        }


        public static ObjectFilter<T> DoNotImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new ObjectFilter<T>(type => !type.Implements(pattern),
                "do not implement interface with full name matching \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new ObjectFilter<T>(type => !type.ResidesInNamespace(pattern),
                "do not reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> AreNotNested()
        {
            return new ObjectFilter<T>(type => !type.IsNested, "are not nested");
        }
    }
}