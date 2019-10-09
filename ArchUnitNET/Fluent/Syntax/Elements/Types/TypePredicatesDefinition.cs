using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypePredicatesDefinition<T> where T : IType
    {
        public static IPredicate<T> Are(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return architecture.GetTypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> Are(IEnumerable<Type> types)
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

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "are assignable to types with full name matching \"" + pattern + "\"";
            return new Predicate<T>(type => type.IsAssignableTo(pattern), description);
        }

        public static IPredicate<T> AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.IsAssignableTo(firstType) || moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new Predicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return ruleType.IsAssignableTo(architecture.GetTypeOfType(firstType)) ||
                       moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetTypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<IType> types)
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

            return new Predicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<Type> types)
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

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> ImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(type => type.Implements(pattern),
                "implement interface with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> ResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(type => type.ResidesInNamespace(pattern),
                "reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HavePropertyMemberWithName(string name)
        {
            return new Predicate<T>(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
        }

        public static IPredicate<T> HaveFieldMemberWithName(string name)
        {
            return new Predicate<T>(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMethodMemberWithName(string name)
        {
            return new Predicate<T>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMemberWithName(string name)
        {
            return new Predicate<T>(type => type.HasMemberWithName(name), "have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNested()
        {
            return new Predicate<T>(type => type.IsNested, "are nested");
        }


        //Negations


        public static IPredicate<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return !architecture.GetTypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are not \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNot(IEnumerable<Type> types)
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

            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNotAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "are not assignable to types with full name matching \"" + pattern + "\"";
            return new Predicate<T>(type => !type.IsAssignableTo(pattern), description);
        }

        public static IPredicate<T> AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.IsAssignableTo(firstType) && !moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new Predicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !ruleType.IsAssignableTo(architecture.GetTypeOfType(firstType)) &&
                       !moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetTypeOfType(type)));
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "are not assignable to " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<IType> types)
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

            return new Predicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<Type> types)
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

            return new ArchitecturePredicate<T>(Condition, description);
        }


        public static IPredicate<T> DoNotImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(type => !type.Implements(pattern),
                "do not implement interface with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(type => !type.ResidesInNamespace(pattern),
                "do not reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new Predicate<T>(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new Predicate<T>(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new Predicate<T>(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMemberWithName(string name)
        {
            return new Predicate<T>(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNotNested()
        {
            return new Predicate<T>(type => !type.IsNested, "are not nested");
        }
    }
}