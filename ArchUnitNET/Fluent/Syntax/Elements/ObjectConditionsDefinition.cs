using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.EnumerableOperator;
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
            var description = moreObjects.Aggregate("be \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(o => o.Equals(firstObject) || moreObjects.Any(o.Equals),
                o => "is " + o.FullName, description);
        }

        public static ICondition<TRuleType> Be(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
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

            return new SimpleCondition<TRuleType>(obj => objectList.Any(o => o.Equals(obj)), o => "is " + o.FullName,
                description);
        }

        public static ICondition<TRuleType> Be(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Contains(ruleType);
            }

            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                "be " + objectProvider.Description);
        }

        public static ICondition<TRuleType> CallAnyMethod(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.CallsMethod(pattern, useRegularExpressions),
                "calls any method with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not call any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> CallAnyMethod(IEnumerable<string> patterns,
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

        public static ICondition<TRuleType> CallAnyMethod(MethodMember method, params MethodMember[] moreMethods)
        {
            bool Condition(TRuleType type)
            {
                return type.CallsMethod(method) || moreMethods.Any(m => type.CallsMethod(m));
            }

            var description = moreMethods.Aggregate("call \"" + method.FullName + "\"",
                (current, methodMember) => current + " or \"" + methodMember.FullName + "\"");
            var failDescription = moreMethods.Aggregate("call \"" + method.FullName + "\"",
                (current, methodMember) => current + " or \"" + methodMember.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> CallAnyMethod(IObjectProvider<MethodMember> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                var methods = objectProvider.GetObjects(architecture);
                return methods.Any(method => ruleType.CallsMethod(method));
            }

            var description = "call any " + objectProvider.Description;
            var failDescription = "call any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> CallAnyMethod(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            bool Condition(TRuleType ruleType)
            {
                return methodList.Any(method => ruleType.CallsMethod(method));
            }

            string description;
            string failDescription;
            if (methodList.IsNullOrEmpty())
            {
                description = "call one of no methods (impossible)";
                failDescription = "does not call one of no methods (always true)";
            }
            else
            {
                var firstMethod = methodList.First();
                description = methodList.Where(method => !method.Equals(firstMethod)).Distinct().Aggregate(
                    "call \"" + firstMethod.FullName + "\"",
                    (current, method) => current + " or \"" + method.FullName + "\"");
                failDescription = methodList.Where(method => !method.Equals(firstMethod)).Distinct().Aggregate(
                    "does not call \"" + firstMethod.FullName + "\"",
                    (current, method) => current + " or \"" + method.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
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
            bool Condition(TRuleType type)
            {
                return type.GetTypeDependencies().Any(target => target.Equals(firstType) || moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("does not depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> DependOnAny(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType type, Architecture architecture)
            {
                return type.GetTypeDependencies().Any(target =>
                    target.Equals(architecture.GetITypeOfType(firstType)) ||
                    moreTypes.Select(architecture.GetITypeOfType).Contains(target));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate(
                "does not depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> DependOnAny(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return !ruleType.GetTypeDependencies().IsNullOrEmpty() &&
                       ruleType.GetTypeDependencies().Any(target => types.Contains(target));
            }

            var description = "depend on any " + objectProvider.Description;
            var failDescription = "does not depend on any " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> DependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return !ruleType.GetTypeDependencies().IsNullOrEmpty() &&
                       ruleType.GetTypeDependencies().Any(target => typeList.Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "depend on one of no types (impossible)";
                failDescription = "does not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "does not depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ICondition<TRuleType> DependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.GetTypeDependencies().IsNullOrEmpty() &&
                       ruleType.GetTypeDependencies().Any(target =>
                           typeList.Select(architecture.GetITypeOfType).Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "depend on one of no types (impossible)";
                failDescription = "does not depend on no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "does not depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
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

                return new ConditionResult(pass, dynamicFailDescription);
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

                return new ConditionResult(pass, dynamicFailDescription);
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
            ConditionResult Condition(TRuleType ruleType)
            {
                var typeList = new List<IType> {firstType};
                typeList.AddRange(moreTypes);
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = new List<IType> {architecture.GetITypeOfType(firstType)};
                typeList.AddRange(moreTypes.Select(architecture.GetITypeOfType));
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = "only depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
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

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (!iTypeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
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
                obj => "is " + obj.Visibility.ToStringInLowerCase(), "be private");
        }

        public static ICondition<TRuleType> BePublic()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Public,
                obj => "is " + obj.Visibility.ToStringInLowerCase(), "be public");
        }

        public static ICondition<TRuleType> BeProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Protected,
                obj => "is " + obj.Visibility.ToStringInLowerCase(), "be protected");
        }

        public static ICondition<TRuleType> BeInternal()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Internal,
                obj => "is " + obj.Visibility.ToStringInLowerCase(), "be internal");
        }

        public static ICondition<TRuleType> BeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == ProtectedInternal, obj => "is " + obj.Visibility.ToStringInLowerCase(),
                "be protected internal");
        }

        public static ICondition<TRuleType> BePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == PrivateProtected, obj => "is " + obj.Visibility.ToStringInLowerCase(),
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
            var description = moreObjects.Aggregate("not be \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(o => !o.Equals(firstObject) && !moreObjects.Any(o.Equals),
                o => "is " + o.FullName, description);
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
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

            return new SimpleCondition<TRuleType>(obj => objectList.All(o => !o.Equals(obj)), o => "is " + o.FullName,
                description);
        }

        public static ICondition<TRuleType> NotBe(IObjectProvider<ICanBeAnalyzed> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !objectProvider.GetObjects(architecture).Contains(ruleType);
            }

            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                "not be " + objectProvider.Description);
        }

        public static ICondition<TRuleType> NotCallAnyMethod(string pattern, bool useRegularExpressions = false)
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

                return new ConditionResult(pass, dynamicFailDescription);
            }

            return new SimpleCondition<TRuleType>(Condition,
                "not call any method with full name " + (useRegularExpressions ? "matching" : "containing") +
                " \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotCallAnyMethod(IEnumerable<string> patterns,
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

                return new ConditionResult(pass, dynamicFailDescription);
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

        public static ICondition<TRuleType> NotCallAnyMethod(MethodMember method, params MethodMember[] moreMethods)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var methodList = new List<MethodMember> {method};
                methodList.AddRange(moreMethods);
                var pass = true;
                var dynamicFailDescription = "does call";
                foreach (var dependency in ruleType.GetCalledMethods())
                {
                    if (methodList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreMethods.Aggregate("not call \"" + method.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotCallAnyMethod(IObjectProvider<MethodMember> objectProvider)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var methodList = objectProvider.GetObjects(architecture).ToList();
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetCalledMethods())
                {
                    if (methodList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = "not call " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotCallAnyMethod(IEnumerable<MethodMember> methods)
        {
            var methodList = methods.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does call";
                foreach (var dependency in ruleType.GetCalledMethods())
                {
                    if (methodList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
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

            return new SimpleCondition<TRuleType>(Condition, description);
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

                return new ConditionResult(pass, dynamicFailDescription);
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

                return new ConditionResult(pass, dynamicFailDescription);
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
            ConditionResult Condition(TRuleType ruleType)
            {
                var typeList = new List<IType> {firstType};
                typeList.AddRange(moreTypes);
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = new List<IType> {architecture.GetITypeOfType(firstType)};
                typeList.AddRange(moreTypes.Select(architecture.GetITypeOfType));
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = "not depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (typeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
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

            return new SimpleCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotDependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var pass = true;
                var dynamicFailDescription = "does depend on";
                foreach (var dependency in ruleType.GetTypeDependencies())
                {
                    if (iTypeList.Contains(dependency))
                    {
                        dynamicFailDescription += pass ? " " + dependency.FullName : " and " + dependency.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(pass, dynamicFailDescription);
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