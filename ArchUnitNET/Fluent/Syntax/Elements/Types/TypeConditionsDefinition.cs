using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypeConditionsDefinition<TRuleType> where TRuleType : IType
    {
        public static ICondition<TRuleType> Be(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return architecture.GetITypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("be \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                description, failDescription);
        }

        public static ICondition<TRuleType> Be(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Any(type => type.Equals(ruleType));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not exist";
                failDescription = "does exist";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "be \"" + firstType.FullName + "\"", (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "is not \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }


            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "be assignable to types with full name matching \"" + pattern + "\"";
            var failDescription = "is not assignable to a type with full name matching \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(type => type.IsAssignableToTypesWithFullNameMatching(pattern),
                description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableToTypesWithFullNameContaining(string pattern)
        {
            var description = "be assignable to types with full name containing \"" + pattern + "\"";
            var failDescription = "is not assignable to a type with full name containing \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(type => type.IsAssignableToTypesWithFullNameContaining(pattern),
                description, failDescription);
        }

        public static SimpleCondition<TRuleType> BeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            bool Condition(TRuleType ruleType)
            {
                return ruleType.IsAssignableTo(firstType) || moreTypes.Any(ruleType.IsAssignableTo);
            }

            var description = moreTypes.Aggregate("be assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>((Func<TRuleType, bool>) Condition, description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return ruleType.IsAssignableTo(architecture.GetITypeOfType(firstType)) ||
                       moreTypes.Any(type => ruleType.IsAssignableTo(architecture.GetITypeOfType(type)));
            }

            var description = moreTypes.Aggregate("be assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, bool>) Condition, description,
                failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(IObjectProvider<IType> objectProvider)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return objectProvider.GetObjects(architecture).Any(ruleType.IsAssignableTo);
            }

            var description = "be assignable to " + objectProvider.Description;
            var failDescription = "is not assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, bool>) Condition, description,
                failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType)
            {
                return typeList.Any(ruleType.IsAssignableTo);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
                failDescription = "is assignable to any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "is not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>((Func<TRuleType, bool>) Condition, description, failDescription);
        }

        public static ICondition<TRuleType> BeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).Any(ruleType.IsAssignableTo);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "be assignable to no types (always false)";
                failDescription = "is assignable to any type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "is not assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, bool>) Condition, description,
                failDescription);
        }

        public static ICondition<TRuleType> ImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterfacesWithFullNameMatching(pattern),
                "implement interface with full name matching \"" + pattern + "\"",
                "does not implement interface with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> ImplementInterfaceWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterfacesWithFullNameContaining(pattern),
                "implement interface with full name containing \"" + pattern + "\"",
                "does not implement interface with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> ResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespaceWithFullNameMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name matching \"" + pattern + "\"",
                "does not reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> ResideInNamespaceWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespaceWithFullNameContaining(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "reside in namespace with full name containing \"" + pattern + "\"",
                "does not reside in namespace with full name containing \"" + pattern + "\"");
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
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !architecture.GetITypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetITypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("not be \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBe(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetITypeOfType).All(type => !type.Equals(ruleType));
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be no type (always true)";
                failDescription = "is no type";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not be \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "is \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }


            return new ArchitectureCondition<TRuleType>(Condition, (obj, architecture) => "is " + obj.FullName,
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableToTypesWithFullNameMatching(string pattern)
        {
            var description = "not be assignable to types with full name matching \"" + pattern + "\"";
            var failDescription = "is assignable to a type with full name matching \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(type => !type.IsAssignableToTypesWithFullNameMatching(pattern),
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableToTypesWithFullNameContaining(string pattern)
        {
            var description = "not be assignable to types with full name containing \"" + pattern + "\"";
            var failDescription = "is assignable to a type with full name containing \"" + pattern + "\"";
            return new SimpleCondition<TRuleType>(type => !type.IsAssignableToTypesWithFullNameContaining(pattern),
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            ConditionResult Condition(TRuleType ruleType)
            {
                var typeList = new List<IType> {firstType};
                typeList.AddRange(moreTypes);
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var assignableType in typeList.Where(ruleType.IsAssignableTo))
                {
                    dynamicFailDescription += pass ? " " + assignableType.FullName : " and " + assignableType.FullName;
                    pass = false;
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("not be assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new SimpleCondition<TRuleType>((Func<TRuleType, ConditionResult>) Condition, description,
                failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = new List<IType> {architecture.GetITypeOfType(firstType)};
                typeList.AddRange(moreTypes.Select(architecture.GetITypeOfType));
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var assignableType in typeList.Where(ruleType.IsAssignableTo))
                {
                    dynamicFailDescription += pass ? " " + assignableType.FullName : " and " + assignableType.FullName;
                    pass = false;
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = moreTypes.Aggregate("not be assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is assignable to \"" + firstType.FullName + "\"",
                (current, type) => current + " or \"" + type.FullName + "\"");
            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, ConditionResult>) Condition,
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IObjectProvider<IType> objectProvider)
        {
            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var assignableType in typeList.Where(ruleType.IsAssignableTo))
                {
                    dynamicFailDescription += pass ? " " + assignableType.FullName : " and " + assignableType.FullName;
                    pass = false;
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            var description = "not be assignable to " + objectProvider.Description;
            var failDescription = "is assignable to " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, ConditionResult>) Condition,
                description, failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType)
            {
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var assignableType in typeList.Where(ruleType.IsAssignableTo))
                {
                    dynamicFailDescription += pass ? " " + assignableType.FullName : " and " + assignableType.FullName;
                    pass = false;
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
                failDescription = "is assignable to no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "not be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => !type.Equals(firstType)).Distinct().Aggregate(
                    "is assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new SimpleCondition<TRuleType>((Func<TRuleType, ConditionResult>) Condition, description,
                failDescription);
        }

        public static ICondition<TRuleType> NotBeAssignableTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            ConditionResult Condition(TRuleType ruleType, Architecture architecture)
            {
                var iTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                var pass = true;
                var dynamicFailDescription = "is assignable to";
                foreach (var assignableType in iTypeList.Where(ruleType.IsAssignableTo))
                {
                    dynamicFailDescription += pass ? " " + assignableType.FullName : " and " + assignableType.FullName;
                    pass = false;
                }

                return new ConditionResult(pass, dynamicFailDescription);
            }

            string description;
            string failDescription;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be assignable to no types (always true)";
                failDescription = "is assignable to no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "not be assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
                failDescription = typeList.Where(type => type != firstType).Distinct().Aggregate(
                    "is assignable to \"" + firstType.FullName + "\"",
                    (current, type) => current + " or \"" + type.FullName + "\"");
            }

            return new ArchitectureCondition<TRuleType>((Func<TRuleType, Architecture, ConditionResult>) Condition,
                description, failDescription);
        }

        public static ICondition<TRuleType> NotImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterfacesWithFullNameMatching(pattern),
                "not implement interface with full name matching \"" + pattern + "\"",
                "does implement interface with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotImplementInterfaceWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterfacesWithFullNameContaining(pattern),
                "not implement interface with full name containing \"" + pattern + "\"",
                "does implement interface with full name containing \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespaceWithFullNameMatching(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name matching \"" + pattern + "\"",
                "does reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static ICondition<TRuleType> NotResideInNamespaceWithFullNameContaining(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespaceWithFullNameContaining(pattern),
                obj => "does reside in " + obj.Namespace.FullName,
                "not reside in namespace with full name containing \"" + pattern + "\"",
                "does reside in namespace with full name containing \"" + pattern + "\"");
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