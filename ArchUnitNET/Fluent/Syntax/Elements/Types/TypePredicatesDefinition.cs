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
                return architecture.GetITypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
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
                return typeList.Select(architecture.GetITypeOfType).Any(type => type.Equals(ruleType));
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
            return new SimplePredicate<T>(type => type.IsAssignableToTypesWithFullNameMatching(pattern), description);
        }

        public static IPredicate<T> AreAssignableToTypesWithFullNameContaining(string pattern)
        {
            var description = "are assignable to types with full name containing \"" + pattern + "\"";
            return new SimplePredicate<T>(type => type.IsAssignableToTypesWithFullNameContaining(pattern), description);
        }

        public static IPredicate<T> AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.IsAssignableTo(firstType) || moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return ruleType.IsAssignableTo(architecture.GetITypeOfType(firstType)) ||
                       moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetITypeOfType(type)));
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

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Any(ruleType.IsAssignableTo);
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
            return new SimplePredicate<T>(type => type.ImplementsInterfacesWithFullNameMatching(pattern),
                "implement interface with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> ImplementInterfaceWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => type.ImplementsInterfacesWithFullNameContaining(pattern),
                "implement interface with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> ResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(type => type.ResidesInNamespaceWithFullNameMatching(pattern),
                "reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> ResideInNamespaceWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => type.ResidesInNamespaceWithFullNameContaining(pattern),
                "reside in namespace with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> ResideInAssemblyWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(type => type.ResidesInAssemblyWithFullNameMatching(pattern),
                "reside in assembly with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> ResideInAssemblyWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => type.ResidesInAssemblyWithFullNameContaining(pattern),
                "reside in assembly with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> HavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
        }

        public static IPredicate<T> HaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> HaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => type.HasMemberWithName(name),
                "have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNested()
        {
            return new SimplePredicate<T>(type => type.IsNested, "are nested");
        }


        //Negations


        public static IPredicate<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return !architecture.GetITypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
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
                return typeList.Select(architecture.GetITypeOfType).All(type => !type.Equals(ruleType));
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
            return new SimplePredicate<T>(type => !type.IsAssignableToTypesWithFullNameMatching(pattern), description);
        }

        public static IPredicate<T> AreNotAssignableToTypesWithFullNameContaining(string pattern)
        {
            var description = "are not assignable to types with full name containing \"" + pattern + "\"";
            return new SimplePredicate<T>(type => !type.IsAssignableToTypesWithFullNameContaining(pattern),
                description);
        }

        public static IPredicate<T> AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.IsAssignableTo(firstType) && !moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("are not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !ruleType.IsAssignableTo(architecture.GetITypeOfType(firstType)) &&
                       !moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetITypeOfType(type)));
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

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetITypeOfType).Any(ruleType.IsAssignableTo);
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
            return new SimplePredicate<T>(type => !type.ImplementsInterfacesWithFullNameMatching(pattern),
                "do not implement interface with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotImplementInterfaceWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => !type.ImplementsInterfacesWithFullNameContaining(pattern),
                "do not implement interface with full name containing \"" + pattern + "\"");
        }


        public static IPredicate<T> DoNotResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(type => !type.ResidesInNamespaceWithFullNameMatching(pattern),
                "do not reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInNamespaceWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => !type.ResidesInNamespaceWithFullNameContaining(pattern),
                "do not reside in namespace with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInAssemblyWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(type => !type.ResidesInAssemblyWithFullNameMatching(pattern),
                "do not reside in assembly with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotResideInAssemblyWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(type => !type.ResidesInAssemblyWithFullNameContaining(pattern),
                "do not reside in assembly with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
        }

        public static IPredicate<T> DoNotHaveMemberWithName(string name)
        {
            return new SimplePredicate<T>(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
        }

        public static IPredicate<T> AreNotNested()
        {
            return new SimplePredicate<T>(type => !type.IsNested, "are not nested");
        }
    }
}