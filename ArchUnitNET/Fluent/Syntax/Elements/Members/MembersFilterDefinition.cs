using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MembersFilterDefinition<T> where T : IMember
    {
        public static ObjectFilter<T> HaveBodyTypeMemberDependencies()
        {
            return new ObjectFilter<T>(member => member.HasBodyTypeMemberDependencies(),
                "have body type member dependencies");
        }

        public static ObjectFilter<T> HaveBodyTypeMemberDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HaveMethodCallDependencies()
        {
            return new ObjectFilter<T>(member => member.HasMethodCallDependencies(),
                "have method call dependencies");
        }

        public static ObjectFilter<T> HaveMethodCallDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HaveFieldTypeDependencies()
        {
            return new ObjectFilter<T>(member => member.HasFieldTypeDependencies(),
                "have field type dependencies");
        }

        public static ObjectFilter<T> HaveFieldTypeDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"");
        }


        //Negations


        public static ObjectFilter<T> DoNotHaveBodyTypeMemberDependencies()
        {
            return new ObjectFilter<T>(
                member => !member.HasBodyTypeMemberDependencies(), "do not have body type member dependencies");
        }

        public static ObjectFilter<T> DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => !member.HasBodyTypeMemberDependencies(pattern),
                "do not have body type member dependencies \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHaveMethodCallDependencies()
        {
            return new ObjectFilter<T>(
                member => !member.HasMethodCallDependencies(), "do not have method call dependencies");
        }

        public static ObjectFilter<T> DoNotHaveMethodCallDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => !member.HasMethodCallDependencies(pattern),
                "do not have method call dependencies \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHaveFieldTypeDependencies()
        {
            return new ObjectFilter<T>(
                member => !member.HasFieldTypeDependencies(), "do not have field type dependencies");
        }

        public static ObjectFilter<T> DoNotHaveFieldTypeDependencies(string pattern)
        {
            return new ObjectFilter<T>(
                member => !member.HasFieldTypeDependencies(pattern),
                "do not have field type dependencies \"" + pattern + "\"");
        }
    }
}