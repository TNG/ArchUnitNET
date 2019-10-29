using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Conditions.EnumerableOperator;
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
                obj => "is " + obj.FullName,
                "have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> Be(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            var patternList = patterns.ToList();
            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not exist";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "have full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
            }

            return new SimpleCondition<TRuleType>(
                obj => patternList.Any(pattern => obj.FullNameMatches(pattern, useRegularExpressions)),
                obj => "is " + obj.FullName, description);
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

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var typeList = ruleTypes.ToList();
                var passedObjects = objectList.OfType<TRuleType>().Intersect(typeList).ToList();
                foreach (var failedObject in typeList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (objectList.IsNullOrEmpty())
            {
                description = "not exist";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "be \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
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
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
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
                "calls any method with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not call any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
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
                    "calls any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not call any methods with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
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
                "depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not depend on any type with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
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
                    "depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
                    (current, pattern) => current + " or \"" + pattern + "\"");
                failDescription = patternList.Where(pattern => !pattern.Equals(firstPattern)).Distinct().Aggregate(
                    "does not depend any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"",
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
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var passedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(iTypeList).Any())
                    .ToList();
                foreach (var failedObject in ruleTypeList.Except(passedObjects))
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(iTypeList))
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
                "only depend on types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
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
                    "only depend on types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
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
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Except(typeList).Any())
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

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes)
            {
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Except(typeList).Any())
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

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> ruleTypes, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Except(iTypeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Except(iTypeList))
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

        public static ICondition<TRuleType> HaveName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Name.Equals(name), obj => "does have name " + obj.Name, "have name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> HaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => obj.NameMatches(pattern),
                obj => "does have name " + obj.Name,
                "have full name matching \"" + pattern + "\"");
        }


        public static ICondition<TRuleType> HaveFullName(string fullname)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullName.Equals(fullname), obj => "does have full name " + obj.FullName,
                "have full name \"" + fullname + "\"");
        }

        public static ICondition<TRuleType> HaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => obj.FullNameMatches(pattern),
                obj => "does have full name " + obj.FullName,
                "have full name matching \"" + pattern + "\"");
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

        public static RelationCondition<TRuleType, Class> DependOnAnyClassesThat()
        {
            return new RelationCondition<TRuleType, Class>(obj => obj.GetClassDependencies(), Any,
                "depend on any classes that", "does not depend on any classes that");
        }

        public static RelationCondition<TRuleType, Class> OnlyDependOnClassesThat()
        {
            return new RelationCondition<TRuleType, Class>(obj => obj.GetClassDependencies(), All,
                "only depend on classes that", "does not only depend on classes that");
        }

        public static RelationCondition<TRuleType, Interface> DependOnAnyInterfacesThat()
        {
            return new RelationCondition<TRuleType, Interface>(obj => obj.GetInterfaceDependencies(), Any,
                "depend on any interfaces that", "does not depend on any interfaces that");
        }

        public static RelationCondition<TRuleType, Interface> OnlyDependOnInterfacesThat()
        {
            return new RelationCondition<TRuleType, Interface>(obj => obj.GetInterfaceDependencies(), All,
                "only depend on interfaces that", "does not only depend on interfaces that");
        }

        public static RelationCondition<TRuleType, IType> DependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(obj => obj.GetTypeDependencies(), Any,
                "depend on any types that", "does not depend on any types that");
        }

        public static RelationCondition<TRuleType, IType> OnlyDependOnTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(obj => obj.GetTypeDependencies(), All,
                "only depend on types that", "does not only depend on types that");
        }

        public static RelationCondition<TRuleType, Attribute> HaveAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(obj => obj.Attributes, Any,
                "have attributes that",
                "does not have attributes that");
        }

        public static RelationCondition<TRuleType, Attribute> OnlyHaveAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(obj => obj.Attributes, All,
                "only have attributes that",
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
                "not have full name " + (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
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
                    "not have full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
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
                    yield return new ConditionResult(failedObject, false, "is " + failedObject.FullName);
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
                "not call any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
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
                    "not call methods with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
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
                    foreach (var method in failedObject.GetCalledMethods().Union(methodList))
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
                    foreach (var method in failedObject.GetCalledMethods().Union(methodList))
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
                "not depend on any types with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
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
                    "not depend on types with full name " + (useRegularExpressions ? "matching" : "containing") +
                    " \"" + firstPattern + "\"", (current, pattern) => current + " or \"" + pattern + "\"");
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
                    foreach (var type in failedObject.GetTypeDependencies().Union(typeList))
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
                    foreach (var type in failedObject.GetTypeDependencies().Union(typeList))
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
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var ruleTypeList = ruleTypes.ToList();
                var failedObjects = ruleTypeList.Where(type => type.GetTypeDependencies().Intersect(iTypeList).Any())
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var dynamicFailDescription = "does depend on";
                    var first = true;
                    foreach (var type in failedObject.GetTypeDependencies().Union(iTypeList))
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

        public static ICondition<TRuleType> NotHaveName(string name)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.Name.Equals(name), obj => "does have name " + obj.Name,
                "not have name \"" + name + "\"");
        }

        public static ICondition<TRuleType> NotHaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameMatches(pattern), obj => "does have name " + obj.Name,
                "not have name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotHaveFullName(string fullname)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.FullName.Equals(fullname),
                obj => "does have full name " + obj.FullName, "not have full name \"" + fullname + "\"");
        }

        public static ICondition<TRuleType> NotHaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.FullNameMatches(pattern),
                obj => "does have full name " + obj.FullName,
                "not have full name matching \"" + pattern + "\"");
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

        public static RelationCondition<TRuleType, Class> NotDependOnAnyClassesThat()
        {
            return new RelationCondition<TRuleType, Class>(obj => obj.GetClassDependencies(), None,
                "not depend on any classes that", "does depend on any classes that");
        }

        public static RelationCondition<TRuleType, Interface> NotDependOnAnyInterfacesThat()
        {
            return new RelationCondition<TRuleType, Interface>(obj => obj.GetInterfaceDependencies(), None,
                "not depend on any interfaces that", "does depend on any interfaces that");
        }

        public static RelationCondition<TRuleType, IType> NotDependOnAnyTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(obj => obj.GetTypeDependencies(), None,
                "not depend on any types that", "does depend on any types that");
        }

        public static RelationCondition<TRuleType, Attribute> NotHaveAttributesThat()
        {
            return new RelationCondition<TRuleType, Attribute>(obj => obj.Attributes, None,
                "not have attributes that", "does have attributes that");
        }
    }
}