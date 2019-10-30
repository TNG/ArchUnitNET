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
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypeConditionsDefinition<TRuleType> where TRuleType : IType
    {
        public static ICondition<TRuleType> Be(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return Be(types);
        }

        public static ICondition<TRuleType> Be(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.OfType<IType>().Intersect(iTypeList).ToList();
                foreach (var failedObject in ruleTypeList.Cast<IType>().Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "be \"" + firstType.FullName + "\"", (current, type) => current + " or \"" + type.FullName + "\"");
            }


            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            var description = "be assignable to types with full name " +
                              (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"";
            var failDescription = "is not assignable to a type with full name " +
                                  (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(type => type.IsAssignableTo(pattern, useRegularExpressions),
                description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.Any(pattern => ruleType.IsAssignableTo(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
                failDescription = "is assignable to any type (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "be assignable to types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "is not assignable to types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static ICondition<TRuleType> BeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return BeAssignableTo(types);
        }

        public static ICondition<TRuleType> BeAssignableTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is not assignable to " + objectProvider.Description;
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is assignable to any type (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                        "is not assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
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
                description = "be assignable to no types (always false)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(iTypeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is assignable to any type (always true)";
                }
                else
                {
                    failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                        "is not assignable to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\"");
                }

                foreach (var failedObject in ruleTypeList.Except(passedObjects))
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
                description = "be assignable to no types (always false)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> ImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(pattern, useRegularExpressions),
                "implement interface with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not implement interface with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> ResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<TRuleType> ResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInAssembly(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Assembly.FullName,
                "reside in assembly with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<TRuleType> ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly)) ||
                       moreAssemblies.Any(asm => ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm)));
            }

            var description = moreAssemblies.Aggregate("reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\"");

            return new ArchitectureCondition<TRuleType>(Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName, description);
        }

        public static ICondition<TRuleType> HavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => type.HasPropertyMemberWithName(name),
                "have a property member with name \"" + name + "\"",
                "does not have a property member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> HaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasFieldMemberWithName(name), "have a field member with name \"" + name + "\"",
                "does not have a field member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> HaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => type.HasMethodMemberWithName(name),
                "have a method member with name \"" + name + "\"",
                "does not have a method member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> HaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMemberWithName(name), "have a member with name \"" + name + "\"",
                "does not have a member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> BeNested()
        {
            return new SimpleCondition<TRuleType>(type => type.IsNested, "be nested", "is not nested");
        }


        //Negations


        public static ICondition<TRuleType> NotBe(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return NotBe(types);
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.OfType<IType>().Intersect(iTypeList).ToList();
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
                }

                foreach (var passedObject in ruleTypeList.Cast<IType>().Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not be \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var type in ruleType.GetAssignableTypes())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        dynamicFailDescription += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            var description = "not be assignable to types with full name " +
                              (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var type in ruleType.GetAssignableTypes())
                {
                    if (patternList.Any(pattern => type.FullNameMatches(pattern, useRegularExpressions)))
                    {
                        dynamicFailDescription += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(type => !type.Equals(firstPattern)).Distinct().Aggregate(
                    "not be assignable to types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return NotBeAssignableTo(types);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is assignable to " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not be assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(typeList).Any())
                    .ToList();
                string dynamicFailDescription;
                if (typeList.IsNullOrEmpty())
                {
                    dynamicFailDescription = "is assignable to any type (always true)";
                    foreach (var failedObject in failedObjects)
                    {
                        yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                    }
                }
                else
                {
                    foreach (var failedObject in failedObjects)
                    {
                        dynamicFailDescription = "is assignable to";
                        var first = true;
                        foreach (var type in failedObject.GetAssignableTypes().Intersect(typeList))
                        {
                            dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                            first = false;
                        }

                        yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                    }
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "not be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetAssignableTypes().Intersect(iTypeList).Any())
                    .ToList();
                string dynamicFailDescription;
                if (typeList.IsNullOrEmpty())
                {
                    dynamicFailDescription = "is assignable to any type (always true)";
                    foreach (var failedObject in failedObjects)
                    {
                        yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                    }
                }
                else
                {
                    foreach (var failedObject in failedObjects)
                    {
                        dynamicFailDescription = "is assignable to";
                        var first = true;
                        foreach (var type in failedObject.GetAssignableTypes().Intersect(iTypeList))
                        {
                            dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                            first = false;
                        }

                        yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                    }
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
            }
            else
            {
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterface(pattern, useRegularExpressions),
                "not implement interface with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"",
                "does implement interface with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInAssembly(pattern, useRegularExpressions),
                obj => "does reside in " + obj.Assembly.FullName,
                "not reside in assembly with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<TRuleType> NotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(assembly)) &&
                       !moreAssemblies.Any(asm => ruleType.Assembly.Equals(architecture.GetAssemblyOfAssembly(asm)));
            }

            var description = moreAssemblies.Aggregate("not reside in assembly \"" + assembly.FullName + "\"",
                (current, asm) => current + " or \"" + asm.FullName + "\"");

            return new ArchitectureCondition<TRuleType>(Condition,
                (type, architecture) => "does reside in " + type.Assembly.FullName, description);
        }

        public static ICondition<TRuleType> NotHavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasPropertyMemberWithName(name),
                "not have property member with name \"" + name + "\"",
                "does have property member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> NotHaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasFieldMemberWithName(name),
                "not have field member with name \"" + name + "\"",
                "does have field member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> NotHaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasMethodMemberWithName(name),
                "not have method member with name \"" + name + "\"",
                "does have method member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> NotHaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMemberWithName(name), "not have member with name \"" + name + "\"",
                "does have member with name \"" + name + "\"");
        }

        public static ICondition<TRuleType> NotBeNested()
        {
            return new SimpleCondition<TRuleType>(type => !type.IsNested, "not be nested", "is nested");
        }
    }
}