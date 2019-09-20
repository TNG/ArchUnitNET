using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypesConditionDefinition<TRuleType> where TRuleType : IType
    {
        public static SimpleCondition<TRuleType> ImplementInterface(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(pattern), "implement interface \"" + pattern + "\"",
                "does not implement interface \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> ImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ImplementsInterface(intf), "implement interface \"" + intf.FullName + "\"",
                "does not implement interface \"" + intf.FullName + "\"");
        }

        public static SimpleCondition<TRuleType> ResideInNamespace(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => type.ResidesInNamespace(pattern), "reside in namespace \"" + pattern + "\"",
                "does not reside in namespace \"" + pattern + "\"");
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

        public static SimpleCondition<TRuleType> NotImplementInterface(string pattern)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterface(pattern),
                "not implement interface \"" + pattern + "\"", "does implement interface \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotImplementInterface(Interface intf)
        {
            return new SimpleCondition<TRuleType>(type => !type.ImplementsInterface(intf),
                "not implement interface \"" + intf.FullName + "\"",
                "does implement interface \"" + intf.FullName + "\"");
        }

        public static SimpleCondition<TRuleType> NotResideInNamespace(string pattern)
        {
            return new SimpleCondition<TRuleType>(
                type => !type.ResidesInNamespace(pattern), "not reside in namespace \"" + pattern + "\"",
                "does reside in namespace \"" + pattern + "\"");
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