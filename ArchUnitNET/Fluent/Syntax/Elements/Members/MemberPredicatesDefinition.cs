using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberPredicatesDefinition<T> where T : IMember
    {
        public static Predicate<T> AreDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(member => member.IsDeclaredIn(pattern),
                "are declared in types with full name matching \"" + pattern + "\"");
        }

        public static Predicate<T> AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return ruleType.DeclaringType.Equals(firstType) || moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new Predicate<T>(Condition, description);
        }

        public static ArchitecturePredicate<T> AreDeclaredIn(Type firstType, params Type[] moreTypes)
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

        public static ArchitecturePredicate<T> AreDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "are declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static Predicate<T> AreDeclaredIn(IEnumerable<IType> types)
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

            return new Predicate<T>(Condition, description);
        }

        public static ArchitecturePredicate<T> AreDeclaredIn(IEnumerable<Type> types)
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

        public static Predicate<T> HaveBodyTypeMemberDependencies()
        {
            return new Predicate<T>(member => member.HasBodyTypeMemberDependencies(),
                "have body type member dependencies");
        }

        public static Predicate<T> HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"");
        }

        public static Predicate<T> HaveMethodCallDependencies()
        {
            return new Predicate<T>(member => member.HasMethodCallDependencies(),
                "have method call dependencies");
        }

        public static Predicate<T> HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"");
        }

        public static Predicate<T> HaveFieldTypeDependencies()
        {
            return new Predicate<T>(member => member.HasFieldTypeDependencies(),
                "have field type dependencies");
        }

        public static Predicate<T> HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"");
        }


        //Negations


        public static Predicate<T> AreNotDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(member => !member.IsDeclaredIn(pattern),
                "are not declared in types with full name matching \"" + pattern + "\"");
        }

        public static Predicate<T> AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(T ruleType)
            {
                return !ruleType.DeclaringType.Equals(firstType) && !moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("are not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new Predicate<T>(Condition, description);
        }

        public static ArchitecturePredicate<T> AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
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

        public static ArchitecturePredicate<T> AreNotDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(T ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "are not declared in " + objectProvider.Description;
            return new ArchitecturePredicate<T>(Condition, description);
        }

        public static Predicate<T> AreNotDeclaredIn(IEnumerable<IType> types)
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

            return new Predicate<T>(Condition, description);
        }

        public static ArchitecturePredicate<T> AreNotDeclaredIn(IEnumerable<Type> types)
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

        public static Predicate<T> DoNotHaveBodyTypeMemberDependencies()
        {
            return new Predicate<T>(
                member => !member.HasBodyTypeMemberDependencies(), "do not have body type member dependencies");
        }

        public static Predicate<T> DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => !member.HasBodyTypeMemberDependencies(pattern),
                "do not have body type member dependencies \"" + pattern + "\"");
        }

        public static Predicate<T> DoNotHaveMethodCallDependencies()
        {
            return new Predicate<T>(
                member => !member.HasMethodCallDependencies(), "do not have method call dependencies");
        }

        public static Predicate<T> DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => !member.HasMethodCallDependencies(pattern),
                "do not have method call dependencies \"" + pattern + "\"");
        }

        public static Predicate<T> DoNotHaveFieldTypeDependencies()
        {
            return new Predicate<T>(
                member => !member.HasFieldTypeDependencies(), "do not have field type dependencies");
        }

        public static Predicate<T> DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new Predicate<T>(
                member => !member.HasFieldTypeDependencies(pattern),
                "do not have field type dependencies \"" + pattern + "\"");
        }
    }
}