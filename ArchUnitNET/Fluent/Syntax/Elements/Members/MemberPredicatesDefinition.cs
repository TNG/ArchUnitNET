using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberPredicatesDefinition<T> where T : IMember
    {
        public static IPredicate<T> AreDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(member => member.IsDeclaredInTypeWithFullNameMatching(pattern),
                "are declared in types with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> AreDeclaredInTypesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(member => member.IsDeclaredInTypeWithFullNameContaining(pattern),
                "are declared in types with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.DeclaringType.Equals(firstType) || moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return ruleType.DeclaringType.Equals(architecture.GetTypeOfType(firstType)) ||
                       moreTypes.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "are declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "are declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> HaveBodyTypeMemberDependencies()
        {
            return new SimplePredicate<T>(member => member.HasBodyTypeMemberDependencies(),
                "have body type member dependencies");
        }

        public static IPredicate<T> HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasBodyTypeMemberDependenciesWithFullNameMatching(pattern),
                "have body type member dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasBodyTypeMemberDependenciesWithFullNameContaining(pattern),
                "have body type member dependencies with full name containing\"" + pattern + "\"");
        }

        public static IPredicate<T> HaveMethodCallDependencies()
        {
            return new SimplePredicate<T>(member => member.HasMethodCallDependencies(),
                "have method call dependencies");
        }

        public static IPredicate<T> HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasMethodCallDependenciesWithFullNameMatching(pattern),
                "have method call dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasMethodCallDependenciesWithFullNameContaining(pattern),
                "have method call dependencies with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveFieldTypeDependencies()
        {
            return new SimplePredicate<T>(member => member.HasFieldTypeDependencies(),
                "have field type dependencies");
        }

        public static IPredicate<T> HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasFieldTypeDependenciesWithFullNameMatching(pattern),
                "have field type dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> HaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => member.HasFieldTypeDependenciesWithFullNameContaining(pattern),
                "have field type dependencies with full name containing \"" + pattern + "\"");
        }


        //Negations


        public static IPredicate<T> AreNotDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(member => !member.IsDeclaredInTypeWithFullNameMatching(pattern),
                "are not declared in types with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> AreNotDeclaredInTypesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(member => !member.IsDeclaredInTypeWithFullNameContaining(pattern),
                "are not declared in types with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.DeclaringType.Equals(firstType) && !moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !ruleType.DeclaringType.Equals(architecture.GetTypeOfType(firstType)) &&
                       !moreTypes.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "are not declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType)
            {
                return !typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "are not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimplePredicate<T>(Condition, description);
        }

        public static IPredicate<T> AreNotDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(T ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are declared in any type (always true9";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "are not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static IPredicate<T> DoNotHaveBodyTypeMemberDependencies()
        {
            return new SimplePredicate<T>(
                member => !member.HasBodyTypeMemberDependencies(), "do not have body type member dependencies");
        }

        public static IPredicate<T> DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasBodyTypeMemberDependenciesWithFullNameMatching(pattern),
                "do not have body type member dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasBodyTypeMemberDependenciesWithFullNameContaining(pattern),
                "do not have body type member dependencies with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveMethodCallDependencies()
        {
            return new SimplePredicate<T>(
                member => !member.HasMethodCallDependencies(), "do not have method call dependencies");
        }

        public static IPredicate<T> DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasMethodCallDependenciesWithFullNameMatching(pattern),
                "do not have method call dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasMethodCallDependenciesWithFullNameContaining(pattern),
                "do not have method call dependencies with full name containing \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveFieldTypeDependencies()
        {
            return new SimplePredicate<T>(
                member => !member.HasFieldTypeDependencies(), "do not have field type dependencies");
        }

        public static IPredicate<T> DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasFieldTypeDependenciesWithFullNameMatching(pattern),
                "do not have field type dependencies with full name matching \"" + pattern + "\"");
        }

        public static IPredicate<T> DoNotHaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            return new SimplePredicate<T>(
                member => !member.HasFieldTypeDependenciesWithFullNameContaining(pattern),
                "do not have field type dependencies with full name containing \"" + pattern + "\"");
        }
    }
}