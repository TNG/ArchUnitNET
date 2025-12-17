using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberConditionsDefinition
    {
        public static IOrderedCondition<PropertyMember> HaveGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != NotAccessible,
                "have a getter",
                "has no getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Private,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.GetterVisibility)
                    + " getter",
                "have a private getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePublicGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Public,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.GetterVisibility)
                    + " getter",
                "have a public getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Protected,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.GetterVisibility)
                    + " getter",
                "have a protected getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == Internal,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.GetterVisibility)
                    + " getter",
                "have an internal getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveProtectedInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == ProtectedInternal,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.GetterVisibility)
                    + " getter",
                "have a protected internal getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == PrivateProtected,
                member =>
                    "does have " + VisibilityStrings.ToString(member.GetterVisibility) + " getter",
                "have a private protected getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != NotAccessible,
                "have a setter",
                "has no setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Private,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.SetterVisibility)
                    + " setter",
                "have a private setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Public,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.SetterVisibility)
                    + " setter",
                "have a public setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Protected,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.SetterVisibility)
                    + " setter",
                "have a protected setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == Internal,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.SetterVisibility)
                    + " setter",
                "have an internal setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == ProtectedInternal,
                member =>
                    "does have a "
                    + VisibilityStrings.ToString(member.SetterVisibility)
                    + " setter",
                "have a protected internal setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == PrivateProtected,
                member =>
                    "does have " + VisibilityStrings.ToString(member.SetterVisibility) + " setter",
                "have a private protected setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HaveInitSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.Writability == Writability.InitOnly,
                "have an only init only setter",
                "does not have an init only setter"
            );
        }

        public static IOrderedCondition<PropertyMember> BeVirtual()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.IsVirtual,
                "be virtual",
                "is not virtual"
            );
        }

        //Negations

        public static IOrderedCondition<PropertyMember> NotHaveGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == NotAccessible,
                "have no getter",
                "does have a getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePrivateGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Private,
                "not have a private getter",
                "does have a private getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePublicGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Public,
                "not have a public getter",
                "does have a public getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Protected,
                "not have a protected getter",
                "does have a protected getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != Internal,
                "not have an internal getter",
                "does have an internal getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveProtectedInternalGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != ProtectedInternal,
                "not have a protected internal getter",
                "does have a protected internal getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePrivateProtectedGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != PrivateProtected,
                "not have a private protected getter",
                "does have a private protected getter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == NotAccessible,
                "have no setter",
                "does have a setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePrivateSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Private,
                "not have a private setter",
                "does have a private setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePublicSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Public,
                "not have a public setter",
                "does have a public setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Protected,
                "not have a protected setter",
                "does have a protected setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != Internal,
                "not have an internal setter",
                "does have an internal setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveProtectedInternalSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != ProtectedInternal,
                "not have a protected internal setter",
                "does have a protected internal setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePrivateProtectedSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != PrivateProtected,
                "not have a private protected setter",
                "does have a private protected setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHaveInitSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.Writability != Writability.InitOnly,
                "not have an init only setter",
                "has an init only setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotBeVirtual()
        {
            return new SimpleCondition<PropertyMember>(
                member => !member.IsVirtual,
                "not be virtual",
                "is virtual"
            );
        }
    }
}
