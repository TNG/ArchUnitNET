using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MembersConditionDefinition<TRuleType> where TRuleType : IMember
    {
        public static SimpleCondition<TRuleType> HaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasBodyTypeMemberDependencies(), "have body type member dependencies",
                "has no body type member dependencies");
        }

        public static SimpleCondition<TRuleType> HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"",
                "has no body type member dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasMethodCallDependencies(), "have method call dependencies",
                "has no method call dependencies");
        }

        public static SimpleCondition<TRuleType> HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"",
                "has no method call dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> HaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => member.HasFieldTypeDependencies(), "have field type dependencies",
                "has no field type dependencies");
        }

        public static SimpleCondition<TRuleType> HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"",
                "has no field type dependencies \"" + pattern + "\"");
        }


        //Negations


        public static SimpleCondition<TRuleType> NotHaveBodyTypeMemberDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasBodyTypeMemberDependencies(), "not have body type member dependencies",
                "does have body type member dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasBodyTypeMemberDependencies(pattern),
                "not have body type member dependencies \"" + pattern + "\"",
                "does have body type member dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveMethodCallDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasMethodCallDependencies(), "not have method call dependencies",
                "does have method call dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasMethodCallDependencies(pattern),
                "not have method call dependencies \"" + pattern + "\"",
                "does have method call dependencies \"" + pattern + "\"");
        }

        public static SimpleCondition<TRuleType> NotHaveFieldTypeDependencies()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.HasFieldTypeDependencies(), "not have field type dependencies",
                "does have field type dependencies");
        }

        public static SimpleCondition<TRuleType> NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            return new SimpleCondition<TRuleType>(member => !member.HasFieldTypeDependencies(pattern),
                "not have field type dependencies \"" + pattern + "\"",
                "does have field type dependencies \"" + pattern + "\"");
        }
    }
}