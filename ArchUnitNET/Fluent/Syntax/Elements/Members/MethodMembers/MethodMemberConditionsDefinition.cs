//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberConditionsDefinition
    {
        public static ICondition<MethodMember> BeConstructor()
        {
            return new SimpleCondition<MethodMember>(member => member.IsConstructor(), "be a constructor",
                "is no constructor");
        }

        public static ICondition<MethodMember> BeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => member.IsVirtual, "be virtual", "is not virtual");
        }

        public static ICondition<MethodMember> BeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsCalledBy(pattern, useRegularExpressions),
                "be called by types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "is called by a type with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<MethodMember> BeCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return patternList.Any(pattern => ruleType.IsCalledBy(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "be called by one of no types (always false)";
                failDescription = "is not called by one of no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "be called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "is not called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }


            return new SimpleCondition<MethodMember>(Condition, description, failDescription);
        }

        public static ICondition<MethodMember> BeCalledBy(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return BeCalledBy(types);
        }

        public static ICondition<MethodMember> BeCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return BeCalledBy(types);
        }

        public static ICondition<MethodMember> BeCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is not called by " + objectProvider.Description;
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be called by " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> BeCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is not called by one of no types (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                        "is not called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be called by one of no types (always false)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "be called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> BeCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(iTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is not called by one of no types (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                        "is not called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be called by one of no types (always false)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "be called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimpleCondition<MethodMember>(
                member => member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not have dependencies in method body to a type with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return patternList.Any(pattern => ruleType.HasDependencyInMethodBodyTo(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "have dependency in method body to one of no types (always false)";
                failDescription = "does not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "does not have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }


            return new SimpleCondition<MethodMember>(Condition, description, failDescription);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(typeList).Any())
                    .ToList();
                var failDescription = "does not have dependencies in method body to " + objectProvider.Description;
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "have dependencies in method body to " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "does not have dependencies in method body to one of no types (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                        "does not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(iTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "does not have dependencies in method body to one of no types (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                        "does not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }


        //Negations


        public static ICondition<MethodMember> BeNoConstructor()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsConstructor(), "be no constructor",
                "is a constructor");
        }

        public static ICondition<MethodMember> NotBeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsVirtual, "not be virtual", "is virtual");
        }

        public static ICondition<MethodMember> NotBeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(MethodMember member)
            {
                var pass = true;
                var description = "is called by";
                foreach (var type in member.GetMethodCallDependencies(true).Select(dependency => dependency.Origin)
                    .Distinct())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        description += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(member, pass, description);
            }

            return new SimpleCondition<MethodMember>(Condition,
                "not be called by types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<MethodMember> NotBeCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return !patternList.Any(pattern => ruleType.IsCalledBy(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "not be called by one of no types (always true)";
                failDescription = "is called by one of no types (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "not be called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "is called by types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }


            return new SimpleCondition<MethodMember>(Condition, description, failDescription);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return NotBeCalledBy(types);
        }

        public static ICondition<MethodMember> NotBeCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return NotBeCalledBy(types);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is called by " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not be called by " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is called by one of no types (always false)";
                }
                else
                {
                    failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                        "is called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be called by one of no types (always true)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "not be called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(iTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is called by one of no types (always false)";
                }
                else
                {
                    failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                        "is called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be called by one of no types (always true)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not be called by \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            ConditionResult Condition(MethodMember member)
            {
                var pass = true;
                var description = "does have dependencies in method body to";
                foreach (var type in member.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                    .Distinct())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        description += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(member, pass, description);
            }

            return new SimpleCondition<MethodMember>(Condition,
                "not have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(MethodMember ruleType)
            {
                return !patternList.Any(pattern =>
                    ruleType.HasDependencyInMethodBodyTo(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "not have dependencies in method body to one of no types (always true)";
                failDescription = "does have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "not have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "does have dependencies in method body to types with full name " +
                    (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }


            return new SimpleCondition<MethodMember>(Condition, description, failDescription);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(IType firstType,
            params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return NotHaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return NotHaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(typeList).Any())
                    .ToList();
                var failDescription = "does have dependencies in method body to " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not have dependencies in method body to " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "does not have dependencies in method body to one of no types (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                        "does not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList.Where(methodMember =>
                        methodMember.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                            .Intersect(iTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "does have dependencies in method body to one of no types (always false)";
                }
                else
                {
                    failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                        "does have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not have dependencies in method body to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }
    }
}