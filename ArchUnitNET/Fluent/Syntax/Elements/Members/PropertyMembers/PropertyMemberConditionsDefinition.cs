using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberConditionsDefinition
    {
        public static ICondition<PropertyMember> HaveGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.Visibility != NotAccessible, "have getter",
                "has no getter");
        }

        public static ICondition<PropertyMember> HaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != NotAccessible, "have setter",
                "has no setter");
        }

        public static ICondition<PropertyMember> HavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Private, "have private setter",
                "does not have private setter");
        }

        public static ICondition<PropertyMember> HavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Public, "have public setter",
                "does not have public setter");
        }

        public static ICondition<PropertyMember> HaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Protected, "have protected setter",
                "does not have protected setter");
        }

        public static ICondition<PropertyMember> HaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Internal, "have internal setter",
                "does not have internal setter");
        }

        public static ICondition<PropertyMember> HaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility == ProtectedInternal,
                "have protected internal setter", "does not have protected internal setter");
        }

        public static ICondition<PropertyMember> HavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility == PrivateProtected,
                "have private protected setter", "does not have private protected setter");
        }

        public static ICondition<PropertyMember> BeVirtual()
        {
            return new SimpleCondition<PropertyMember>(member => member.IsVirtual, "be virtual", "is not virtual");
        }


        //Negations


        public static ICondition<PropertyMember> NotHaveGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.Visibility == NotAccessible, "not have getter",
                "does have a getter");
        }

        public static ICondition<PropertyMember> NotHaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == NotAccessible, "not have setter",
                "does have a setter");
        }

        public static ICondition<PropertyMember> NotHavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Private, "not have private setter",
                "does have a private setter");
        }

        public static ICondition<PropertyMember> NotHavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Public, "not have public setter",
                "does have a public setter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Protected,
                "not have protected setter", "does have a protected setter");
        }

        public static ICondition<PropertyMember> NotHaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Internal, "not have internal setter",
                "does have an internal setter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility != ProtectedInternal,
                "not have protected internal setter", "does have a protected internal setter");
        }

        public static ICondition<PropertyMember> NotHavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility != PrivateProtected,
                "not have private protected setter", "does have a private protected setter");
        }

        public static ICondition<PropertyMember> NotBeVirtual()
        {
            return new SimpleCondition<PropertyMember>(member => !member.IsVirtual, "not be virtual", "is virtual");
        }
    }
}