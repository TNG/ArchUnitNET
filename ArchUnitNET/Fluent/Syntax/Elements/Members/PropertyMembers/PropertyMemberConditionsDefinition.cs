using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberConditionsDefinition
    {
        public static ICondition<PropertyMember> HaveGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.GetterVisibility != NotAccessible,
                "have a getter",
                "has no getter");
        }

        public static ICondition<PropertyMember> HavePrivateGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Private,
                member => "does have a " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a private getter");
        }

        public static ICondition<PropertyMember> HavePublicGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Public,
                member => "does have a " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a public getter");
        }

        public static ICondition<PropertyMember> HaveProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Protected,
                member => "does have a " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a protected getter");
        }

        public static ICondition<PropertyMember> HaveInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Internal,
                member => "does have a " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have an internal getter");
        }

        public static ICondition<PropertyMember> HaveProtectedInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.GetterVisibility == ProtectedInternal,
                member => "does have a " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a protected internal getter");
        }

        public static ICondition<PropertyMember> HavePrivateProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.GetterVisibility == PrivateProtected,
                member => "does have " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a private protected getter");
        }

        public static ICondition<PropertyMember> HaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != NotAccessible, "have a setter",
                "has no setter");
        }

        public static ICondition<PropertyMember> HavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Private,
                member => "does have a " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a private setter");
        }

        public static ICondition<PropertyMember> HavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Public,
                member => "does have a " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a public setter");
        }

        public static ICondition<PropertyMember> HaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Protected,
                member => "does have a " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a protected setter");
        }

        public static ICondition<PropertyMember> HaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Internal,
                member => "does have a " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have an internal setter");
        }

        public static ICondition<PropertyMember> HaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility == ProtectedInternal,
                member => "does have a " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a protected internal setter");
        }

        public static ICondition<PropertyMember> HavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility == PrivateProtected,
                member => "does have " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a private protected setter");
        }

        public static ICondition<PropertyMember> BeVirtual()
        {
            return new SimpleCondition<PropertyMember>(member => member.IsVirtual, "be virtual", "is not virtual");
        }


        //Negations


        public static ICondition<PropertyMember> NotHaveGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == NotAccessible, "have no getter", "does have a getter");
        }

        public static ICondition<PropertyMember> NotHavePrivateGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Private, "not have a private getter",
                "does have a private getter");
        }

        public static ICondition<PropertyMember> NotHavePublicGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Public, "not have a public getter", "does have a public getter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Protected, "not have a protected getter",
                "does have a protected getter");
        }

        public static ICondition<PropertyMember> NotHaveInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Internal, "not have an internal getter",
                "does have an internal getter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.GetterVisibility != ProtectedInternal,
                "not have a protected internal getter", "does have a protected internal getter");
        }

        public static ICondition<PropertyMember> NotHavePrivateProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.GetterVisibility != PrivateProtected,
                "not have a private protected getter", "does have a private protected getter");
        }

        public static ICondition<PropertyMember> NotHaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == NotAccessible, "have no setter", "does have a setter");
        }

        public static ICondition<PropertyMember> NotHavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Private, "not have a private setter",
                "does have a private setter");
        }

        public static ICondition<PropertyMember> NotHavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Public, "not have a public setter", "does have a public setter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Protected, "not have a protected setter",
                "does have a protected setter");
        }

        public static ICondition<PropertyMember> NotHaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Internal, "not have an internal setter",
                "does have an internal setter");
        }

        public static ICondition<PropertyMember> NotHaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility != ProtectedInternal,
                "not have a protected internal setter", "does have a protected internal setter");
        }

        public static ICondition<PropertyMember> NotHavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(member => member.SetterVisibility != PrivateProtected,
                "not have a private protected setter", "does have a private protected setter");
        }

        public static ICondition<PropertyMember> NotBeVirtual()
        {
            return new SimpleCondition<PropertyMember>(member => !member.IsVirtual, "not be virtual", "is virtual");
        }
    }
}