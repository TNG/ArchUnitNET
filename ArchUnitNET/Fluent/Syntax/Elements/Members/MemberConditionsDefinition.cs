using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberConditionsDefinition<TRuleType> where TRuleType : IMember
    {
        public static ICondition<TRuleType> BeDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(member => member.IsDeclaredIn(pattern, useRegularExpressions),
                member => "is declared in " + member.DeclaringType.FullName,
                "be declared in types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<TRuleType> BeDeclaredIn(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.Any(pattern => ruleType.IsDeclaredIn(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(obj => !obj.Equals(firstPattern)).Distinct().Aggregate(
                    "be declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
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


        //Negations


        public static ICondition<TRuleType> NotBeDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(member => !member.IsDeclaredIn(pattern, useRegularExpressions),
                member => "is declared in " + member.DeclaringType.FullName,
                "not be declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.All(pattern => !ruleType.IsDeclaredIn(pattern, useRegularExpressions));
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(obj => !obj.Equals(firstPattern)).Distinct().Aggregate(
                    "not be declared in types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition,
                member => "is declared in " + member.DeclaringType.FullName, description);
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
    }
}