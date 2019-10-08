using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypesConditionDefinition<TRuleType> where TRuleType : IType
    {
        public static ArchitectureCondition<TRuleType> Be(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return architecture.GetTypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("be \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is not \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> Be(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).Any(type => type.Equals(ruleType));
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
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "be \"" + firstType.FullName + "\"", (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "is not \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }


            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static SimpleCondition<TRuleType> ImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(pattern),
                "implement interface with full name matching \"" + pattern + "\"",
                "does not implement interface with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> ImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(intf), "implement interface \"" + intf.FullName + "\"",
                "does not implement interface \"" + intf.FullName + "\"");
        }

        public static SimpleCondition<TRuleType> ResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(pattern),
                "reside in namespace with full name matching \"" + pattern + "\"",
                "does not reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => type.HasPropertyMemberWithName(name),
                "have property member with name \"" + name + "\"",
                "does not have property member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> HaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasFieldMemberWithName(name), "have field member with name \"" + name + "\"",
                "does not have field member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> HaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"",
                "does not have method member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> HaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => type.HasMemberWithName(name), "have member with name \"" + name + "\"",
                "does not have member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> BeNested()
        {
            return new SimpleCondition<TRuleType>(type => type.IsNested, "be nested", "is not nested");
        }


        //Negations


        public static ArchitectureCondition<TRuleType> NotBe(Type firstType, params Type[] moreTypes)
        {
            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return !architecture.GetTypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("not be \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            var failDescription = moreTypes.Aggregate("is \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }

        public static ArchitectureCondition<TRuleType> NotBe(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            bool Condition(TRuleType ruleType, Architecture architecture)
            {
                return typeList.Select(architecture.GetTypeOfType).All(type => !type.Equals(ruleType));
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
                description = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "not be \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
                failDescription = typeList.Where(obj => obj != firstType).Distinct().Aggregate(
                    "is \"" + firstType.FullName + "\"",
                    (current, obj) => current + " or \"" + obj.FullName + "\"");
            }


            return new ArchitectureCondition<TRuleType>(Condition, description, failDescription);
        }


        public static SimpleCondition<TRuleType> NotImplementInterfaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterface(pattern),
                "not implement interface with full name matching \"" + pattern + "\"",
                "does implement interface with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterface(intf),
                "not implement interface \"" + intf.FullName + "\"",
                "does implement interface \"" + intf.FullName + "\"");
        }

        public static SimpleCondition<TRuleType> NotResideInNamespaceWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(pattern),
                "not reside in namespace with full name matching \"" + pattern + "\"",
                "does reside in namespace with full name matching \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHavePropertyMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasPropertyMemberWithName(name),
                "not have property member with name \"" + name + "\"",
                "does have property member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveFieldMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasFieldMemberWithName(name),
                "not have field member with name \"" + name + "\"",
                "does have field member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveMethodMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(type => !type.HasMethodMemberWithName(name),
                "not have method member with name \"" + name + "\"",
                "does have method member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveMemberWithName(string name)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.HasMemberWithName(name), "not have member with name \"" + name + "\"",
                "does have member with name \"" + name + "\"");
        }

        public static SimpleCondition<TRuleType> NotBeNested()
        {
            return new SimpleCondition<TRuleType>(type => !type.IsNested, "not be nested", "is nested");
        }
    }
}