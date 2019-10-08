using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MembersConditionDefinition<TRuleType> where TRuleType : IMember
    {
        public static SimpleCondition<TRuleType> BeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.IsDeclaredIn(pattern),
                "be declared in types with full name matching \"" + pattern + "\"",
                "is not declared in a type with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> BeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return ruleType.DeclaringType.Equals(firstType) || moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> BeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.DeclaringType.Equals(architecture.GetTypeOfType(firstType)) ||
                       moreTypes.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> BeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "be declared in " + objectProvider.Description;
            var failDescription = "is not declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> BeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
                failDescription = "is declared in any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "is not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> BeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
                failDescription = "is declared in any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "is not declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> HaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasBodyTypeMemberDependencies(), "have body type member dependencies",
                "has no body type member dependencies");
        }

        public static SimpleCondition<TRuleType> HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"",
                "has no body type member dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasMethodCallDependencies(), "have method call dependencies",
                "has no method call dependencies");
        }

        public static SimpleCondition<TRuleType> HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"",
                "has no method call dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasFieldTypeDependencies(), "have field type dependencies",
                "has no field type dependencies");
        }

        public static SimpleCondition<TRuleType> HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"",
                "has no field type dependencies \"" + pattern + "\"");
        }


        //Negations


        public static SimpleCondition<TRuleType> NotBeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.IsDeclaredIn(pattern),
                "not be declared in types with full name matching \"" + pattern + "\"",
                "is declared in a type with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotBeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return !ruleType.DeclaringType.Equals(firstType) && !moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("not be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotBeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.DeclaringType.Equals(architecture.GetTypeOfType(firstType)) &&
                       !moreTypes.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("not be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotBeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "not be declared in " + objectProvider.Description;
            var failDescription = "is declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> NotBeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return !typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
                failDescription = "is not declared in any type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "not be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "is declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotBeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetTypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
                failDescription = "is not declared in any type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "not be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "is declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> NotHaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasBodyTypeMemberDependencies(), "not have body type member dependencies",
                "does have body type member dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasBodyTypeMemberDependencies(pattern),
                "not have body type member dependencies \"" + pattern + "\"",
                "does have body type member dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasMethodCallDependencies(), "not have method call dependencies",
                "does have method call dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasMethodCallDependencies(pattern),
                "not have method call dependencies \"" + pattern + "\"",
                "does have method call dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasFieldTypeDependencies(), "not have field type dependencies",
                "does have field type dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasFieldTypeDependencies(pattern),
                "not have field type dependencies \"" + pattern + "\"",
                "does have field type dependencies \"" + pattern + "\"");
        }
    }
}