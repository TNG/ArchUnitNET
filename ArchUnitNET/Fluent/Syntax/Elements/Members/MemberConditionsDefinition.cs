using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberConditionsDefinition<TRuleType> where TRuleType : IMember
    {
        public static ICondition<TRuleType> BeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.IsDeclaredInTypeWithFullNameMatching(pattern),
                member => "is declared in " + member.DeclaringType.FullName,
                "be declared in types with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> BeDeclaredInTypesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.IsDeclaredInTypeWithFullNameContaining(pattern),
                member => "is declared in " + member.DeclaringType.FullName,
                "be declared in types with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> BeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return ruleType.DeclaringType.Equals(firstType) || moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.DeclaringType.Equals(architecture.GetITypeOfType(firstType)) ||
                       moreTypes.Select(architecture.GetITypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "be declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> HaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasBodyTypeMemberDependencies(), "have body type member dependencies",
                "has no body type member dependencies");
        }

        public static ICondition<TRuleType> HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasBodyTypeMemberDependenciesWithFullNameMatching(pattern),
                "have body type member dependencies with full name matching \"" + pattern + "\"",
                "has no body type member dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasBodyTypeMemberDependenciesWithFullNameContaining(pattern),
                "have body type member dependencies with full name containing \"" + pattern + "\"",
                "has no body type member dependencies with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasMethodCallDependencies(), "have method call dependencies",
                "has no method call dependencies");
        }

        public static ICondition<TRuleType> HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasMethodCallDependenciesWithFullNameMatching(pattern),
                "have method call dependencies with full name matching \"" + pattern + "\"",
                "has no method call dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasMethodCallDependenciesWithFullNameContaining(pattern),
                "have method call dependencies with full name matching \"" + pattern + "\"",
                "has no method call dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasFieldTypeDependencies(), "have field type dependencies",
                "has no field type dependencies");
        }

        public static ICondition<TRuleType> HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasFieldTypeDependenciesWithFullNameMatching(pattern),
                "have field type dependencies with full name matching \"" + pattern + "\"",
                "has no field type dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasFieldTypeDependenciesWithFullNameContaining(pattern),
                "have field type dependencies with full name containing \"" + pattern + "\"",
                "has no field type dependencies with full name containing \"" + pattern + "\"");
        }


        //Negations


        public static ICondition<TRuleType> NotBeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.IsDeclaredInTypeWithFullNameMatching(pattern),
                member => "is declared in " + member.DeclaringType.FullName,
                "not be declared in types with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotBeDeclaredInTypesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.IsDeclaredInTypeWithFullNameContaining(pattern),
                member => "is declared in " + member.DeclaringType.FullName,
                "not be declared in types with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return !ruleType.DeclaringType.Equals(firstType) && !moreTypes.Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("not be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.DeclaringType.Equals(architecture.GetITypeOfType(firstType)) &&
                       !moreTypes.Select(architecture.GetITypeOfType).Contains(ruleType.DeclaringType);
            }

            var description = moreTypes.Aggregate("not be declared in \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Contains(ruleType.DeclaringType);
            }

            var description = "not be declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return !typeList.Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "not be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !typeList.Select(architecture.GetITypeOfType).Contains(ruleType.DeclaringType);
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "not be declared in \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition,
                (member, architecture) => "is declared in " + member.DeclaringType.FullName, description);
        }

        public static ICondition<TRuleType> NotHaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasBodyTypeMemberDependencies(), "not have body type member dependencies",
                "does have body type member dependencies");
        }

        public static ICondition<TRuleType> NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasBodyTypeMemberDependenciesWithFullNameMatching(pattern),
                "not have body type member dependencies with full name matching \"" + pattern + "\"",
                "does have body type member dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasBodyTypeMemberDependenciesWithFullNameContaining(pattern),
                "not have body type member dependencies with full name containing \"" + pattern + "\"",
                "does have body type member dependencies with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasMethodCallDependencies(), "not have method call dependencies",
                "does have method call dependencies");
        }

        public static ICondition<TRuleType> NotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasMethodCallDependenciesWithFullNameMatching(pattern),
                "not have method call dependencies with full name matching \"" + pattern + "\"",
                "does have method call dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasMethodCallDependenciesWithFullNameContaining(pattern),
                "not have method call dependencies with full name containing \"" + pattern + "\"",
                "does have method call dependencies with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasFieldTypeDependencies(), "not have field type dependencies",
                "does have field type dependencies");
        }

        public static ICondition<TRuleType> NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasFieldTypeDependenciesWithFullNameMatching(pattern),
                "not have field type dependencies with full name matching \"" + pattern + "\"",
                "does have field type dependencies with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasFieldTypeDependenciesWithFullNameContaining(pattern),
                "not have field type dependencies with full name containing \"" + pattern + "\"",
                "does have field type dependencies with full name containing \"" + pattern + "\"");
        }
    }
}