using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberPredicatesDefinition
    {
        public static IPredicate<PropertyMember> HaveGetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.GetterVisibility != NotAccessible,
                "have getter"
            );
        }

        public static IPredicate<PropertyMember> HavePrivateGetter() =>
            HaveGetterWithVisibility(Private);

        public static IPredicate<PropertyMember> HavePublicGetter() =>
            HaveGetterWithVisibility(Public);

        public static IPredicate<PropertyMember> HaveProtectedGetter() =>
            HaveGetterWithVisibility(Protected);

        public static IPredicate<PropertyMember> HaveInternalGetter() =>
            HaveGetterWithVisibility(Internal);

        public static IPredicate<PropertyMember> HaveProtectedInternalGetter() =>
            HaveGetterWithVisibility(ProtectedInternal);

        public static IPredicate<PropertyMember> HavePrivateProtectedGetter() =>
            HaveGetterWithVisibility(PrivateProtected);

        public static IPredicate<PropertyMember> HaveSetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.SetterVisibility != NotAccessible,
                "have setter"
            );
        }

        public static IPredicate<PropertyMember> HavePrivateSetter() =>
            HaveSetterWithVisibility(Private);

        public static IPredicate<PropertyMember> HavePublicSetter() =>
            HaveSetterWithVisibility(Public);

        public static IPredicate<PropertyMember> HaveProtectedSetter() =>
            HaveSetterWithVisibility(Protected);

        public static IPredicate<PropertyMember> HaveInternalSetter() =>
            HaveSetterWithVisibility(Internal);

        public static IPredicate<PropertyMember> HaveProtectedInternalSetter() =>
            HaveSetterWithVisibility(ProtectedInternal);

        public static IPredicate<PropertyMember> HavePrivateProtectedSetter() =>
            HaveSetterWithVisibility(PrivateProtected);

        public static IPredicate<PropertyMember> HaveInitSetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.Writability == Writability.InitOnly,
                "have an init only setter"
            );
        }

        public static IPredicate<PropertyMember> AreVirtual()
        {
            return new SimplePredicate<PropertyMember>(member => member.IsVirtual, "are virtual");
        }

        //Negations

        public static IPredicate<PropertyMember> HaveNoGetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.GetterVisibility == NotAccessible,
                "have no getter"
            );
        }

        public static IPredicate<PropertyMember> DoNotHavePrivateGetter() =>
            DoNotHaveGetterWithVisibility(Private);

        public static IPredicate<PropertyMember> DoNotHavePublicGetter() =>
            DoNotHaveGetterWithVisibility(Public);

        public static IPredicate<PropertyMember> DoNotHaveProtectedGetter() =>
            DoNotHaveGetterWithVisibility(Protected);

        public static IPredicate<PropertyMember> DoNotHaveInternalGetter() =>
            DoNotHaveGetterWithVisibility(Internal);

        public static IPredicate<PropertyMember> DoNotHaveProtectedInternalGetter() =>
            DoNotHaveGetterWithVisibility(ProtectedInternal);

        public static IPredicate<PropertyMember> DoNotHavePrivateProtectedGetter() =>
            DoNotHaveGetterWithVisibility(PrivateProtected);

        public static IPredicate<PropertyMember> HaveNoSetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.SetterVisibility == NotAccessible,
                "have no setter"
            );
        }

        public static IPredicate<PropertyMember> DoNotHavePrivateSetter() =>
            DoNotHaveSetterWithVisibility(Private);

        public static IPredicate<PropertyMember> DoNotHavePublicSetter() =>
            DoNotHaveSetterWithVisibility(Public);

        public static IPredicate<PropertyMember> DoNotHaveProtectedSetter() =>
            DoNotHaveSetterWithVisibility(Protected);

        public static IPredicate<PropertyMember> DoNotHaveInternalSetter() =>
            DoNotHaveSetterWithVisibility(Internal);

        public static IPredicate<PropertyMember> DoNotHaveProtectedInternalSetter() =>
            DoNotHaveSetterWithVisibility(ProtectedInternal);

        public static IPredicate<PropertyMember> DoNotHavePrivateProtectedSetter() =>
            DoNotHaveSetterWithVisibility(PrivateProtected);

        public static IPredicate<PropertyMember> DoNotHaveInitSetter()
        {
            return new SimplePredicate<PropertyMember>(
                member => member.Writability != Writability.InitOnly,
                "do not have an init only setter"
            );
        }

        public static IPredicate<PropertyMember> AreNotVirtual()
        {
            return new SimplePredicate<PropertyMember>(
                member => !member.IsVirtual,
                "are not virtual"
            );
        }

        private static IPredicate<PropertyMember> HaveGetterWithVisibility(Visibility visibility)
        {
            return new SimplePredicate<PropertyMember>(
                member => member.GetterVisibility == visibility,
                "have " + VisibilityStrings.ToString(visibility) + " getter"
            );
        }

        private static IPredicate<PropertyMember> HaveSetterWithVisibility(Visibility visibility)
        {
            return new SimplePredicate<PropertyMember>(
                member => member.SetterVisibility == visibility,
                "have " + VisibilityStrings.ToString(visibility) + " setter"
            );
        }

        private static IPredicate<PropertyMember> DoNotHaveGetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimplePredicate<PropertyMember>(
                member => member.GetterVisibility != visibility,
                "do not have " + VisibilityStrings.ToString(visibility) + " getter"
            );
        }

        private static IPredicate<PropertyMember> DoNotHaveSetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimplePredicate<PropertyMember>(
                member => member.SetterVisibility != visibility,
                "do not have " + VisibilityStrings.ToString(visibility) + " setter"
            );
        }
    }
}
