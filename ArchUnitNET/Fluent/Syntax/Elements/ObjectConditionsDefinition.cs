//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public static class ObjectConditionsDefinition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public static ICondition<TRuleType> Exist()
        {
            return new ExistsCondition<TRuleType>(true);
        }

        public static ICondition<TRuleType> Be(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameMatches(pattern, useRegularExpressions),
                "have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"",
                "does not have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern +
                "\"");
        }

        public static ICondition<TRuleType> Be(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();
            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "not exist";
                failDescription = "does exist";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "have full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not have full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(
                obj => patternList.Any(pattern => obj.FullNameMatches(pattern, useRegularExpressions)),
                description, failDescription);
        }


        public static ICondition<TRuleType> Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var objects = new List<ICanBeAnalyzed> {firstObject};
            objects.AddRange(moreObjects);
            return Be(objects);
        }

        public static ICondition<TRuleType> Be(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();

            string description;
            string failDescription;
            if (objectList.IsNullOrEmpty())
            {
                description = "not exist";
                failDescription = "does exist";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "be \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "is not \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var typeList = ruleTypes.ToList();
                var passedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }


            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> Be(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var objectList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var passedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, "is not " + objectProvider.Description);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new ArchitectureCondition<TRuleType>(Condition, "be " + objectProvider.Description);
        }

        public static ICondition<TRuleType> CallAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.CallsMethod(pattern, useRegularExpressions),
                "calls any method with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"",
                "does not call any method with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> CallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.Any(pattern => ruleType.CallsMethod(pattern, useRegularExpressions));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "call one of no methods (impossible)";
                failDescription = "does not call one of no methods (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "calls any method with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not call any methods with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> CallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            var methods = new List<MethodMember> {method};
            methods.AddRange(moreMethods);
            return CallAny(methods);
        }

        public static ICondition<TRuleType> CallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var methodList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var passedObjects = typeList.Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Except(methodList))
                    {
                        dynamicFailDescription += first ? " " + method.FullName : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "call any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> CallAny(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var typeList = ruleTypes.ToList();
                var passedObjects = typeList.Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Except(methodList))
                    {
                        dynamicFailDescription += first ? " " + method.FullName : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (methodList.IsNullOrEmpty())
            {
                description = "call one of no methods (impossible)";
            }
            else
            {
                var firstMethod = methodList.First();
                description = methodList.Where(method => !method.Equals(firstMethod)).Distinct().Aggregate(
                    "call \"" + firstMethod.FullName + "\"",
                    (current, method) => current + " or \"" + method.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> DependOnAny(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.DependsOn(pattern, useRegularExpressions),
                "depend on any types with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"",
                "does not depend on any type with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> DependOnAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return !ruleType.GetTypeDependencies().IsNullOrEmpty() &&
                       ruleType.GetTypeDependencies().Any(target =>
                           patternList.Any(pattern => target.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "depend on one of no types (impossible)";
                failDescription = "does not depend on no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "depend on any types with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not depend any types with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> DependOnAny(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return DependOnAny(types);
        }

        public static ICondition<TRuleType> DependOnAny(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return DependOnAny(types);
        }

        public static ICondition<TRuleType> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "depend on any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> DependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "depend on one of no types (impossible)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> DependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture e)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies().Intersect(archUnitTypeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(archUnitTypeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "depend on one of no types (impossible)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> FollowCustomCondition(Func<TRuleType, ConditionResult> condition,
            string description)
        {
            return new SimpleCondition<TRuleType>(condition, description);
        }

        public static ICondition<TRuleType> FollowCustomCondition(Func<TRuleType, bool> condition, string description,
            string failDescription)
        {
            return new SimpleCondition<TRuleType>(condition, description, failDescription);
        }

        public static ICondition<TRuleType> OnlyDependOn(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!dependency.FullNameMatches(pattern, useRegularExpressions))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            return new SimpleCondition<TRuleType>(Condition,
                "only depend on types with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"");
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!patternList.Any(pattern => dependency.FullNameMatches(pattern, useRegularExpressions)))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "only depend on types with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return OnlyDependOn(types);
        }

        public static ICondition<TRuleType> OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return OnlyDependOn(types);
        }

        public static ICondition<TRuleType> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies(architecture).Except(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "only depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies(architecture).Except(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture e)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies(architecture).Except(archUnitTypeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(archUnitTypeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.HasAttribute(pattern, useRegularExpressions),
                "have any attribute with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"",
                "does not have any attribute with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return ruleType.Attributes.Any(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
                failDescription = "does not have one of no attributes (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "have any attribute with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not have any attribute with full name " +
                    (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return HaveAnyAttributes(attributes);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return HaveAnyAttributes(attributes);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false,
                        "does not have any " + objectProvider.Description);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "have any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            string description;
            string failDescription;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
                failDescription = "does not have one of no attributes (always true)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => !attribute.Equals(firstAttribute)).Distinct().Aggregate(
                    "have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
                failDescription = attributeList.Where(attribute => !attribute.Equals(firstAttribute)).Distinct()
                    .Aggregate(
                        "does not have attribute \"" + firstAttribute.FullName + "\"",
                        (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            string description;
            string failDescription;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have one of no attributes (impossible)";
                failDescription = "does not have one of no attributes (always true)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => attribute != firstAttribute).Distinct().Aggregate(
                    "have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
                failDescription = attributeList.Where(attribute => attribute != firstAttribute).Distinct().Aggregate(
                    "does not have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var archAttributeList = attributeList.Select(architecture.GetAttributeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(archAttributeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }


        public static ICondition<TRuleType> OnlyHaveAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.OnlyHasAttributes(pattern, useRegularExpressions),
                "only have attributes with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"",
                "does not only have attributes with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return ruleType.Attributes.IsNullOrEmpty() || ruleType.Attributes.All(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "have no attributes";
                failDescription = "does have attributes";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "only have attributes with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " and \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not only have attributes with full name " +
                    (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return OnlyHaveAttributes(attributes);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return OnlyHaveAttributes(attributes);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Except(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Except(attributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "does only have " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Except(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Except(attributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have no attributes";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => !attribute.Equals(firstAttribute)).Distinct().Aggregate(
                    "only have attributes \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " and \"" + attribute.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var archUnitAttributeList = new List<Attribute>();
                foreach (var type in attributeList)
                {
                    try
                    {
                        var archUnitAttribute = architecture.GetAttributeOfType(type);
                        archUnitAttributeList.Add(archUnitAttribute);
                    }
                    catch (TypeDoesNotExistInArchitecture e)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Except(archUnitAttributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Except(archUnitAttributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "have no attributes";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => attribute != firstAttribute).Distinct().Aggregate(
                    "only have attributes \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " and \"" + attribute.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithArguments(object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAnyAttributesWithArguments(argumentValues);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return HaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(string attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(Attribute attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments(Type attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return HaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have no or any attributes with arguments (always true)";
                failDescription = (ruleType, architecture) =>
                    "not have no or any attributes with arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(attribute => attribute != firstArgument).Aggregate(
                    "have any attributes with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");


                failDescription = (ruleType, architecture) =>
                {
                    var actualArgumentValues = ruleType.AttributeInstances
                        .SelectMany(instance =>
                            instance.AttributeArguments.Select(argument => argument.Value)).ToList();
                    if (!actualArgumentValues.Any())
                    {
                        return "does have no attribute with an argument";
                    }

                    var firstActualArgumentValue = actualArgumentValues.First();
                    return actualArgumentValues.Aggregate(
                        "does have attributes with argument values \"" + firstActualArgumentValue + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attributeArguments = obj.AttributeInstances
                    .SelectMany(instance => instance.AttributeArguments.Select(arg => arg.Value)).ToList();
                var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>()).ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (!attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments([NotNull] string attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute + "\"";
                failDescription = "does not have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments([NotNull] Attribute attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithArguments([NotNull] Type attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture e)
                {
                    //can't have a dependency
                    return false;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments)
        {
            var argumentList = attributeArguments.ToList();
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentList.IsNullOrEmpty())
            {
                description = "have no or any attributes with named arguments (always true)";
                failDescription = (ruleType, architecture) =>
                    "not have no or any attributes with named arguments (impossible)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(attribute => attribute != firstArgument).Aggregate(
                    "have any attributes with named arguments \"" + firstArgument.Item1 + "=" + firstArgument.Item2 +
                    "\"",
                    (current, arg) =>
                        current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");


                failDescription = (ruleType, architecture) =>
                {
                    var actualNamedArguments = ruleType.AttributeInstances
                        .SelectMany(instance =>
                            instance.AttributeArguments.OfType<AttributeNamedArgument>()).ToList();
                    if (!actualNamedArguments.Any())
                    {
                        return "does have no attribute with a named argument";
                    }

                    var firstActualNamedArgument = actualNamedArguments.First();
                    return actualNamedArguments.Aggregate(
                        "does have attributes with named arguments \"" + firstActualNamedArgument.Name + "=" +
                        firstActualNamedArgument.Value + "\"",
                        (current, arg) =>
                            current + " and \"" + arg.Name + "=" + arg.Value + "\"");
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attArguments = obj.AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value)))
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (typeAttributeArguments.All(t =>
                                t.Name != arg.Item1 ||
                                t.Value is ITypeInstance<IType> typeInstance &&
                                typeInstance.Type.FullName != argType.FullName ||
                                t.Value is IType type && type.FullName != argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (!attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments([NotNull] string attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute + "\"";
                failDescription = "does not have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1 ||
                                    t.Value is ITypeInstance<IType> typeInstance &&
                                    typeInstance.Type.FullName != argType.FullName ||
                                    t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments([NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1 ||
                                    t.Value is ITypeInstance<IType> typeInstance &&
                                    typeInstance.Type.FullName != argType.FullName ||
                                    t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveAttributeWithNamedArguments([NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "have attribute \"" + attribute.FullName + "\"";
                failDescription = "does not have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does not have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture e)
                {
                    //can't have a dependency
                    return false;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t =>
                                    t.Name != arg.Item1 ||
                                    t.Value is ITypeInstance<IType> typeInstance &&
                                    typeInstance.Type.FullName != argType.FullName ||
                                    t.Value is IType type && type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return true;
                    NextAttribute: ;
                }

                return false;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> HaveName(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(obj => obj.NameMatches(pattern, useRegularExpressions),
                obj => "does have name " + obj.Name,
                "have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveFullName(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(obj => obj.FullNameMatches(pattern, useRegularExpressions),
                obj => "does have full name " + obj.FullName,
                "have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameStartsWith(pattern), obj => "does have name " + obj.Name,
                "have name starting with \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameEndsWith(pattern), obj => "does have name " + obj.Name,
                "have name ending with \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameContains(pattern), obj => "does have name " + obj.Name,
                "have name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> HaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullNameContains(pattern), obj => "does have full name " + obj.FullName,
                "have full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> BePrivate()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Private,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility), "be private");
        }

        public static ICondition<TRuleType> BePublic()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Public,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility), "be public");
        }

        public static ICondition<TRuleType> BeProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Protected,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility), "be protected");
        }

        public static ICondition<TRuleType> BeInternal()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Internal,
                obj => "is " + VisibilityStrings.ToString(obj.Visibility), "be internal");
        }

        public static ICondition<TRuleType> BeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == ProtectedInternal, obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be protected internal");
        }

        public static ICondition<TRuleType> BePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == PrivateProtected, obj => "is " + VisibilityStrings.ToString(obj.Visibility),
                "be private protected");
        }


        //Relation Conditions


        public static RelationCondition<TRuleType, IType> DependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(DependOnAny, "depend on any types that",
                "does not depend on any types that");
        }

        public static RelationCondition<TRuleType, IType> OnlyDependOnTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(OnlyDependOn, "only depend on types that",
                "does not only depend on types that");
        }

        public static RelationCondition<TRuleType, Attribute> HaveAnyAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(HaveAnyAttributes, "have attributes that",
                "does not have attributes that");
        }

        public static RelationCondition<TRuleType, Attribute> OnlyHaveAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(OnlyHaveAttributes, "only have attributes that",
                "does not only have attributes that");
        }


        //Negations


        public static ICondition<TRuleType> NotExist()
        {
            return new ExistsCondition<TRuleType>(false);
        }

        public static ICondition<TRuleType> NotBe(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullNameMatches(pattern, useRegularExpressions),
                obj => "is " + obj.FullName,
                "not have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();
            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "exist";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "not have full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(
                obj => patternList.All(pattern => !obj.FullNameMatches(pattern, useRegularExpressions)),
                obj => "is " + obj.FullName, description);
        }

        public static ICondition<TRuleType> NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var objects = new List<ICanBeAnalyzed> {firstObject};
            objects.AddRange(moreObjects);
            return NotBe(objects);
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var typeList = ruleTypes.ToList();
                var failedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "exist";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "not be \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBe(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var objectList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var failedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, "is " + objectProvider.Description);
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            return new ArchitectureCondition<TRuleType>(Condition, "not be " + objectProvider.Description);
        }

        public static ICondition<TRuleType> NotCallAny(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does call";
                foreach (var dependency in ruleType.GetCalledMethods())
                {
                    if (dependency.FullNameMatches(pattern, useRegularExpressions))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            return new SimpleCondition<TRuleType>(Condition,
                "not call any method with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotCallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does call";
                foreach (var dependency in ruleType.GetCalledMethods())
                {
                    if (patternList.Any(pattern => dependency.FullNameMatches(pattern, useRegularExpressions)))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not call no methods (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "not call methods with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotCallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            var methods = new List<MethodMember> {method};
            methods.AddRange(moreMethods);
            return NotCallAny(methods);
        }

        public static ICondition<TRuleType> NotCallAny(IObjectProvider<MethodMember> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var methodList = objectProvider.GetObjects(architecture).ToList();
                var typeList = ruleTypes.ToList();
                var failedObjects = typeList.Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Intersect(methodList))
                    {
                        dynamicFailDescription += first ? " " + method.FullName : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not call " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotCallAny(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var typeList = ruleTypes.ToList();
                var failedObjects = typeList.Where(type => type.GetCalledMethods().Intersect(methodList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does call";
                    var first = true;
                    foreach (var method in failedObject.GetCalledMethods().Intersect(methodList))
                    {
                        dynamicFailDescription += first ? " " + method.FullName : " and " + method.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in typeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (methodList.IsNullOrEmpty())
            {
                description = "not call no methods (always true)";
            }
            else
            {
                var firstMethod = methodList.First();
                description = methodList.Where(obj => !obj.Equals(firstMethod)).Distinct().Aggregate(
                    "not call \"" + firstMethod.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }


        public static ICondition<TRuleType> NotDependOnAny(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (dependency.FullNameMatches(pattern, useRegularExpressions))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            return new SimpleCondition<TRuleType>(Condition,
                "not depend on any types with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotDependOnAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (patternList.Any(pattern => dependency.FullNameMatches(pattern, useRegularExpressions)))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(ruleType, pass, dynamicFailDescription);
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not depend on no types (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "not depend on types with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> {firstType};
            types.AddRange(moreTypes);
            return NotDependOnAny(types);
        }

        public static ICondition<TRuleType> NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> {firstType};
            types.AddRange(moreTypes);
            return NotDependOnAny(types);
        }

        public static ICondition<TRuleType> NotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Intersect(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(typeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Intersect(typeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var ruleTypeList = ruleTypes.ToList();
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture e)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var failedObjects = ruleTypeList
                    .Where(type => type.GetTypeDependencies().Intersect(archUnitTypeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Intersect(archUnitTypeList))
                    {
                        dynamicFailDescription += first ? " " + type.FullName : " and " + type.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.HasAttribute(pattern, useRegularExpressions),
                "not have any attribute with full name " + (useRegularExpressions ? "matching " : "") + "\"" +
                pattern + "\"",
                "does have any attribute with full name " + (useRegularExpressions ? "matching " : "") +
                "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return !ruleType.Attributes.Any(attribute =>
                    patternList.Any(pattern => attribute.FullNameMatches(pattern, useRegularExpressions)));
            }

            string description;
            string failDescription;
            if (patternList.IsNullOrEmpty())
            {
                description = "not have one of no attributes (always true)";
                failDescription = "does have one of no attributes (impossible)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "not have any attribute with full name " + (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does have any attribute with full name " +
                    (useRegularExpressions ? "matching " : "") +
                    "\"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            var attributes = new List<Attribute> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return NotHaveAnyAttributes(attributes);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            var attributes = new List<Type> {firstAttribute};
            attributes.AddRange(moreAttributes);
            return NotHaveAnyAttributes(attributes);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(IObjectProvider<Attribute> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var attributeList = objectProvider.GetObjects(architecture).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Intersect(attributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not have any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(attributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Intersect(attributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "not have one of no attributes (always true)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => !attribute.Equals(firstAttribute)).Distinct().Aggregate(
                    "not have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            var attributeList = attributes.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var archUnitAttributeList = new List<Attribute>();
                foreach (var type in attributeList)
                {
                    try
                    {
                        var archUnitAttribute = architecture.GetAttributeOfType(type);
                        archUnitAttributeList.Add(archUnitAttribute);
                    }
                    catch (TypeDoesNotExistInArchitecture e)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.Attributes.Intersect(archUnitAttributeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does have attribute";
                    var first = true;
                    foreach (var attribute in failedObject.Attributes.Intersect(archUnitAttributeList))
                    {
                        dynamicFailDescription += first ? " " + attribute.FullName : " and " + attribute.FullName;
                        first = false;
                    }

                    yield return new ConditionResult(failedObject, false, dynamicFailDescription);
                }

                foreach (var passedObject in ruleTypeList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (attributeList.IsNullOrEmpty())
            {
                description = "not have one of no attributes (always true)";
            }
            else
            {
                var firstAttribute = attributeList.First();
                description = attributeList.Where(attribute => attribute != firstAttribute).Distinct().Aggregate(
                    "not have attribute \"" + firstAttribute.FullName + "\"",
                    (current, attribute) => current + " or \"" + attribute.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithArguments(object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAnyAttributesWithArguments(argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            var argumentValues = new List<object> {firstArgumentValue};
            argumentValues.AddRange(moreArgumentValues);
            return NotHaveAttributeWithArguments(attribute, argumentValues);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAnyAttributesWithNamedArguments(attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(string attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(Attribute attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments(Type attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            var attributeArguments = new List<(string, object)> {firstAttributeArgument};
            attributeArguments.AddRange(moreAttributeArguments);
            return NotHaveAttributeWithNamedArguments(attribute, attributeArguments);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have no or any attributes with arguments (impossible)";
                failDescription = (ruleType, architecture) => "have no or any attributes with arguments (always)";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(attribute => attribute != firstArgument).Aggregate(
                    "not have any attributes with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");


                failDescription = (ruleType, architecture) =>
                {
                    var actualArgumentValues = ruleType.AttributeInstances
                        .SelectMany(instance =>
                            instance.AttributeArguments.Select(argument => argument.Value)).ToList();
                    if (!actualArgumentValues.Any())
                    {
                        return "does have no attribute with an argument";
                    }

                    var firstActualArgumentValue = actualArgumentValues.First();
                    return actualArgumentValues.Aggregate(
                        "does have attributes with argument values \"" + firstActualArgumentValue + "\"",
                        (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attributeArguments = obj.AttributeInstances
                    .SelectMany(instance => instance.AttributeArguments.Select(arg => arg.Value)).ToList();
                var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                    .Union(attributeArguments.OfType<IType>()).ToList();
                foreach (var arg in argumentValueList)
                {
                    if (arg is Type argType)
                    {
                        if (typeAttributeArguments.Any(t => t.FullName == argType.FullName))
                        {
                            return false;
                        }
                    }
                    else if (attributeArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments([NotNull] string attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute + "\"";
                failDescription = "does have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments([NotNull] Attribute attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithArguments([NotNull] Type attribute,
            IEnumerable<object> argumentValues)
        {
            string description, failDescription;
            var argumentValueList = argumentValues?.ToList() ?? new List<object> {null};
            if (argumentValueList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentValueList.First();
                description = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
                failDescription = argumentValueList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute.FullName + "\" with arguments \"" + firstArgument + "\"",
                    (current, argumentValue) => current + " and \"" + argumentValue + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture e)
                {
                    //can't have a dependency
                    return true;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArguments = attributeInstance.AttributeArguments.Select(arg => arg.Value).ToList();
                    var typeAttributeArguments = attributeArguments.OfType<ITypeInstance<IType>>().Select(t => t.Type)
                        .Union(attributeArguments.OfType<IType>()).ToList();
                    foreach (var arg in argumentValueList)
                    {
                        if (arg is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!attributeArguments.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments)
        {
            var argumentList = attributeArguments.ToList();
            string description;
            Func<TRuleType, Architecture, string> failDescription;
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have no or any attributes with named arguments (impossible)";
                failDescription = (ruleType, architecture) =>
                    "have no or any attributes with named arguments (always true)";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(attribute => attribute != firstArgument).Aggregate(
                    "not have any attributes with named arguments \"" + firstArgument.Item1 + "=" +
                    firstArgument.Item2 +
                    "\"",
                    (current, arg) =>
                        current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");


                failDescription = (ruleType, architecture) =>
                {
                    var actualNamedArguments = ruleType.AttributeInstances
                        .SelectMany(instance =>
                            instance.AttributeArguments.OfType<AttributeNamedArgument>()).ToList();
                    if (!actualNamedArguments.Any())
                    {
                        return "does have no attribute with a named argument";
                    }

                    var firstActualNamedArgument = actualNamedArguments.First();
                    return actualNamedArguments.Aggregate(
                        "does have attributes with named arguments \"" + firstActualNamedArgument.Name + "=" +
                        firstActualNamedArgument.Value + "\"",
                        (current, arg) =>
                            current + " and \"" + arg.Name + "=" + arg.Value + "\"");
                };
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                var attArguments = obj.AttributeInstances.SelectMany(instance =>
                        instance.AttributeArguments.OfType<AttributeNamedArgument>()
                            .Select(arg => (arg.Name, arg.Value)))
                    .ToList();
                var typeAttributeArguments = attArguments
                    .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                foreach (var arg in argumentList)
                {
                    if (arg.Item2 is Type argType)
                    {
                        if (typeAttributeArguments.Any(t =>
                                t.Name == arg.Item1 &&
                                (t.Value is ITypeInstance<IType> typeInstance &&
                                 typeInstance.Type.FullName == argType.FullName ||
                                 t.Value is IType type && type.FullName == argType.FullName)))
                        {
                            return false;
                        }
                    }
                    else if (attArguments.Contains(arg))
                    {
                        return false;
                    }
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments([NotNull] string attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute + "\"";
                failDescription = "does have attribute \"" + attribute + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.FullNameMatches(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments([NotNull] Attribute attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(attribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveAttributeWithNamedArguments([NotNull] Type attribute,
            IEnumerable<(string, object)> attributeArguments)
        {
            string description, failDescription;
            var argumentList = attributeArguments.ToList();
            if (argumentList.IsNullOrEmpty())
            {
                description = "not have attribute \"" + attribute.FullName + "\"";
                failDescription = "does have attribute \"" + attribute.FullName + "\"";
            }
            else
            {
                var firstArgument = argumentList.First();
                description = argumentList.Where(att => att != firstArgument).Aggregate(
                    "not have attribute \"" + attribute.FullName + "\" with named arguments \"" + firstArgument.Item1 +
                    "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
                failDescription = argumentList.Where(att => att != firstArgument).Aggregate(
                    "does have attribute \"" + attribute.FullName + "\" with named arguments \"" +
                    firstArgument.Item1 + "=" + firstArgument.Item2 + "\"",
                    (current, arg) => current + " and \"" + arg.Item1 + "=" + arg.Item2 + "\"");
            }

            bool Condition(TRuleType obj, Architecture architecture)
            {
                Attribute archUnitAttribute;
                try
                {
                    archUnitAttribute = architecture.GetAttributeOfType(attribute);
                }
                catch (TypeDoesNotExistInArchitecture e)
                {
                    //can't have a dependency
                    return true;
                }

                foreach (var attributeInstance in obj.AttributeInstances)
                {
                    if (!attributeInstance.Type.Equals(archUnitAttribute))
                    {
                        goto NextAttribute;
                    }

                    var attributeArgs = attributeInstance.AttributeArguments.OfType<AttributeNamedArgument>()
                        .Select(arg => (arg.Name, arg.Value)).ToList();
                    var typeAttributeArguments = attributeArgs
                        .Where(arg => arg.Value is ITypeInstance<IType> || arg.Value is IType).ToList();
                    foreach (var arg in argumentList)
                    {
                        if (arg.Item2 is Type argType)
                        {
                            if (typeAttributeArguments.All(t => t.Name != arg.Item1 ||
                                                                t.Value is ITypeInstance<IType> typeInstance &&
                                                                typeInstance.Type.FullName != argType.FullName ||
                                                                t.Value is IType type &&
                                                                type.FullName != argType.FullName))
                            {
                                goto NextAttribute;
                            }
                        }
                        else if (!argumentList.Contains(arg))
                        {
                            goto NextAttribute;
                        }
                    }

                    return false;
                    NextAttribute: ;
                }

                return true;
            }

            return new ArchitectureCondition<TRuleType>(Condition, failDescription, description);
        }

        public static ICondition<TRuleType> NotHaveName(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameMatches(pattern, useRegularExpressions), obj => "does have name " + obj.Name,
                "not have name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveFullName(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.FullNameMatches(pattern, useRegularExpressions),
                obj => "does have full name " + obj.FullName,
                "not have full name " + (useRegularExpressions ? "matching " : "") + "\"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameStartsWith(pattern),
                obj => "does have name " + obj.Name,
                "not have name starting with \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameEndsWith(pattern),
                obj => "does have name " + obj.Name,
                "not have name ending with \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameContains(pattern),
                obj => "does have name " + obj.Name,
                "not have name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.FullNameContains(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotBePrivate()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Private, "not be private", "is private");
        }

        public static ICondition<TRuleType> NotBePublic()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Public, "not be public", "is public");
        }

        public static ICondition<TRuleType> NotBeProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Protected, "not be protected",
                "is protected");
        }

        public static ICondition<TRuleType> NotBeInternal()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Internal, "not be internal", "is internal");
        }

        public static ICondition<TRuleType> NotBeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != ProtectedInternal, "not be protected internal", "is protected internal");
        }

        public static ICondition<TRuleType> NotBePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != PrivateProtected, "not be private protected",
                "is private protected");
        }


        //Relation Condition Negations

        public static RelationCondition<TRuleType, IType> NotDependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(NotDependOnAny, "not depend on any types that",
                "does depend on any types that");
        }

        public static RelationCondition<TRuleType, Attribute> NotHaveAnyAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(NotHaveAnyAttributes, "not have attributes that",
                "does have attributes that");
        }
    }
}
