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
    public static class ObjectConditionsDefinition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        public static ExistsCondition<TRuleType> Exist()
        {
            return new ExistsCondition<TRuleType>(true);
        }

        public static SimpleCondition<TRuleType> Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var description = moreObjects.Aggregate("be \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreObjects.Aggregate("is not \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(o => o.Equals(firstObject) || moreObjects.Any(o.Equals), description,
                failDescription);
        }

        public static SimpleCondition<TRuleType> Be(IEnumerable<ICanBeAnalyzed> objects)
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

            return new SimpleCondition<TRuleType>(obj => objectList.Any(o => o.Equals(obj)), description,
                failDescription);
        }

        public static SimpleCondition<TRuleType> DependOnAnyTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.DependsOn(pattern), "depend on any types with full name matching \"" + pattern + "\"",
                "does not depend on any type with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> DependOnAny(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType type)
            {
                return type.GetTypeDependencies().Any(target => target.Equals(firstType) || moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate(
                "does not depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> DependOnAny(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType type, Architecture architecture)
            {
                return type.GetTypeDependencies().Any(target =>
                    target.Equals(architecture.GetTypeOfType(firstType)) ||
                    moreTypes.Select(architecture.GetTypeOfType).Contains(target));
            }

            var description = moreTypes.Aggregate("depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate(
                "does not depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> DependOnAny(IObjectProvider<IType> objectProvider)
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

        public static SimpleCondition<TRuleType> DependOnAny(IEnumerable<IType> types)
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
                description = "depend on no types (always false)";
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

        public static ArchitectureCondition<TRuleType> DependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !ruleType.GetTypeDependencies().IsNullOrEmpty() &&
                       ruleType.GetTypeDependencies().Any(target =>
                           typeList.Select(architecture.GetTypeOfType).Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "depend on no types (always false)";
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

        public static SimpleCondition<TRuleType> OnlyDependOnTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => obj.OnlyDependsOn(pattern),
                "only depend on types with full name matching \"" + pattern + "\"",
                "does not only depend on types with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return ruleType.GetTypeDependencies()
                    .All(target => target.Equals(firstType) || moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreTypes.Aggregate(
                "does also depend on other types than just \"" + firstType.FullName + "\"",
                (current, obj) => current + " and \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.GetTypeDependencies().All(target =>
                    target.Equals(architecture.GetTypeOfType(firstType)) ||
                    moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(target)));
            }

            var description = moreTypes.Aggregate("only depend on \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate(
                "does also depend on other types than just \"" + firstType.FullName + "\"",
                (current, type) => current + " and \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> OnlyDependOn(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleType.GetTypeDependencies().All(target => types.Contains(target));
            }

            var description = "only depend on " + objectProvider.Description;
            var failDescription = "does also depend on other types than just " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> OnlyDependOn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return ruleType.GetTypeDependencies().All(target => typeList.Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
                failDescription = "has dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "does also depend on other types than just \"" + firstType.FullName + "\"",
                    (current, type) => current + " and \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> OnlyDependOn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.GetTypeDependencies()
                    .All(target => typeList.Select(architecture.GetTypeOfType).Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "have no dependencies";
                failDescription = "has dependencies";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "only depend on \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "does also depend on other types than just \"" + firstType.FullName + "\"",
                    (current, type) => current + " and \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> HaveName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Name.Equals(name), "have name \"" + name + "\"",
                "does not have name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> HaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\"", "does not have full name matching \"" + pattern + "\"");
        }


        public static SimpleCondition<TRuleType> HaveFullName(string fullname)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.FullName.Equals(fullname),
                "have full name \"" + fullname + "\"", "does not have full name \"" + fullname + "\"");
        }

        public static SimpleCondition<TRuleType> HaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => obj.FullNameMatches(pattern),
                "have full name matching \"" + pattern + "\"", "does not have full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameStartsWith(pattern),
                "have name starting with \"" + pattern + "\"", "does not have name starting with \"");
        }

        public static SimpleCondition<TRuleType> HaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameEndsWith(pattern),
                "have name ending with \"" + pattern + "\"", "does not have name ending with \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.NameMatches(pattern),
                "have name containing \"" + pattern + "\"", "does not have name containing \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> BePrivate()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Private, "be private", "is not private");
        }

        public static SimpleCondition<TRuleType> BePublic()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Public, "be public", "is not public");
        }

        public static SimpleCondition<TRuleType> BeProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Protected, "be protected",
                "is not protected");
        }

        public static SimpleCondition<TRuleType> BeInternal()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility == Internal, "be internal", "is not internal");
        }

        public static SimpleCondition<TRuleType> BeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == ProtectedInternal, "be protected internal",
                "is not protected internal");
        }

        public static SimpleCondition<TRuleType> BePrivateProtected()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility == PrivateProtected, "be private protected",
                "is not private protected");
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


        public static ExistsCondition<TRuleType> NotExist()
        {
            return new ExistsCondition<TRuleType>(false);
        }

        public static SimpleCondition<TRuleType> NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            var description = moreObjects.Aggregate("not be \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreObjects.Aggregate("is not \"" + firstObject.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(o => !o.Equals(firstObject) && !moreObjects.Any(o.Equals),
                description, failDescription);
        }

        public static SimpleCondition<TRuleType> NotBe(IEnumerable<ICanBeAnalyzed> objects)
        {
            var objectList = objects.ToList();
            string description;
            string failDescription;
            if (objectList.IsNullOrEmpty())
            {
                description = "not be no object (always true)";
                failDescription = "is no object";
            }
            else
            {
                var firstObject = objectList.First();
                description = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "not be \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = objectList.Where(obj => !obj.Equals(firstObject)).Distinct().Aggregate(
                    "is \"" + firstObject.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(obj => objectList.All(o => !o.Equals(obj)), description,
                failDescription);
        }

        public static SimpleCondition<TRuleType> NotDependOnAnyTypesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.DependsOn(pattern), "not depend on any types with full name matching \"" + pattern + "\"",
                "does depend on types with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return ruleType.GetTypeDependencies()
                    .All(target => !target.Equals(firstType) && !moreTypes.Contains(target));
            }

            var description = moreTypes.Aggregate("not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreTypes.Aggregate("does depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.GetTypeDependencies().All(target =>
                    !target.Equals(architecture.GetTypeOfType(firstType)) &&
                    moreTypes.All(type => !architecture.GetTypeOfType(type).Equals(target)));
            }

            var description = moreTypes.Aggregate("not depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreTypes.Aggregate("does depend on \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotDependOnAny(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleType.GetTypeDependencies().All(target => !types.Contains(target));
            }

            var description = "not depend on " + objectProvider.Description;
            var failDescription = "does depend on " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> NotDependOnAny(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType type)
            {
                return type.GetTypeDependencies().All(target => !typeList.Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not depend on no types (always true)";
                failDescription = "does depend on one of no types (impossible)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = typeList.Where(obj => !obj.Equals(firstType)).Distinct().Aggregate(
                    "does depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotDependOnAny(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType type, Architecture architecture)
            {
                return type.GetTypeDependencies()
                    .All(target => !typeList.Select(architecture.GetTypeOfType).Contains(target));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not depend on no types (always true)";
                failDescription = "does depend on one of no types (impossible)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "not depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "does depend on \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> NotHaveName(string name)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.Name.Equals(name), "not have name \"" + name + "\"", "does have name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameMatches(pattern),
                "not have name matching \"" + pattern + "\"", "does have name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveFullName(string fullname)
        {
            return new SimpleCondition<TRuleType>(
                obj => !obj.FullName.Equals(fullname), "not have full name \"" + fullname + "\"",
                "does have full name \"" + fullname + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.FullNameMatches(pattern),
                "not have full name matching \"" + pattern + "\"", "does have full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveNameStartingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameStartsWith(pattern),
                "not have name starting with \"" + pattern + "\"", "does have name starting with \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveNameEndingWith(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameEndsWith(pattern),
                "not have name ending with \"" + pattern + "\"", "does have name ending with \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(obj => !obj.NameMatches(pattern),
                "not have name containing \"" + pattern + "\"", "does have name containing \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotBePrivate()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Private, "not be private", "is private");
        }

        public static SimpleCondition<TRuleType> NotBePublic()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Public, "not be public", "is public");
        }

        public static SimpleCondition<TRuleType> NotBeProtected()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Protected, "not be protected",
                "is protected");
        }

        public static SimpleCondition<TRuleType> NotBeInternal()
        {
            return new SimpleCondition<TRuleType>(obj => obj.Visibility != Internal, "not be internal", "is internal");
        }

        public static SimpleCondition<TRuleType> NotBeProtectedInternal()
        {
            return new SimpleCondition<TRuleType>(
                obj => obj.Visibility != ProtectedInternal, "not be protected internal", "is protected internal");
        }

        public static SimpleCondition<TRuleType> NotBePrivateProtected()
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