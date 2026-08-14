using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberConditionsDefinition
    {
        private static readonly Dictionary<Visibility, string> VisibilityDescriptions =
            new Dictionary<Visibility, string>
            {
                { Private, "a private" },
                { Public, "a public" },
                { Protected, "a protected" },
                { Internal, "an internal" },
                { ProtectedInternal, "a protected internal" },
                { PrivateProtected, "a private protected" },
                { NotAccessible, "a not accessible" },
            };

        public static IOrderedCondition<PropertyMember> HaveGetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != NotAccessible,
                "have a getter",
                "has no getter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateGetter() =>
            HaveGetterWithVisibility(Private);

        public static IOrderedCondition<PropertyMember> HavePublicGetter() =>
            HaveGetterWithVisibility(Public);

        public static IOrderedCondition<PropertyMember> HaveProtectedGetter() =>
            HaveGetterWithVisibility(Protected);

        public static IOrderedCondition<PropertyMember> HaveInternalGetter() =>
            HaveGetterWithVisibility(Internal);

        public static IOrderedCondition<PropertyMember> HaveProtectedInternalGetter() =>
            HaveGetterWithVisibility(ProtectedInternal);

        public static IOrderedCondition<PropertyMember> HavePrivateProtectedGetter() =>
            HaveGetterWithVisibility(PrivateProtected);

        public static IOrderedCondition<PropertyMember> HaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != NotAccessible,
                "have a setter",
                "has no setter"
            );
        }

        public static IOrderedCondition<PropertyMember> HavePrivateSetter() =>
            HaveSetterWithVisibility(Private);

        public static IOrderedCondition<PropertyMember> HavePublicSetter() =>
            HaveSetterWithVisibility(Public);

        public static IOrderedCondition<PropertyMember> HaveProtectedSetter() =>
            HaveSetterWithVisibility(Protected);

        public static IOrderedCondition<PropertyMember> HaveInternalSetter() =>
            HaveSetterWithVisibility(Internal);

        public static IOrderedCondition<PropertyMember> HaveProtectedInternalSetter() =>
            HaveSetterWithVisibility(ProtectedInternal);

        public static IOrderedCondition<PropertyMember> HavePrivateProtectedSetter() =>
            HaveSetterWithVisibility(PrivateProtected);

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

        public static IOrderedCondition<PropertyMember> NotHavePrivateGetter() =>
            NotHaveGetterWithVisibility(Private);

        public static IOrderedCondition<PropertyMember> NotHavePublicGetter() =>
            NotHaveGetterWithVisibility(Public);

        public static IOrderedCondition<PropertyMember> NotHaveProtectedGetter() =>
            NotHaveGetterWithVisibility(Protected);

        public static IOrderedCondition<PropertyMember> NotHaveInternalGetter() =>
            NotHaveGetterWithVisibility(Internal);

        public static IOrderedCondition<PropertyMember> NotHaveProtectedInternalGetter() =>
            NotHaveGetterWithVisibility(ProtectedInternal);

        public static IOrderedCondition<PropertyMember> NotHavePrivateProtectedGetter() =>
            NotHaveGetterWithVisibility(PrivateProtected);

        public static IOrderedCondition<PropertyMember> NotHaveSetter()
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == NotAccessible,
                "have no setter",
                "does have a setter"
            );
        }

        public static IOrderedCondition<PropertyMember> NotHavePrivateSetter() =>
            NotHaveSetterWithVisibility(Private);

        public static IOrderedCondition<PropertyMember> NotHavePublicSetter() =>
            NotHaveSetterWithVisibility(Public);

        public static IOrderedCondition<PropertyMember> NotHaveProtectedSetter() =>
            NotHaveSetterWithVisibility(Protected);

        public static IOrderedCondition<PropertyMember> NotHaveInternalSetter() =>
            NotHaveSetterWithVisibility(Internal);

        public static IOrderedCondition<PropertyMember> NotHaveProtectedInternalSetter() =>
            NotHaveSetterWithVisibility(ProtectedInternal);

        public static IOrderedCondition<PropertyMember> NotHavePrivateProtectedSetter() =>
            NotHaveSetterWithVisibility(PrivateProtected);

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

        private static IOrderedCondition<PropertyMember> HaveGetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility == visibility,
                member =>
                    "does have " + VisibilityDescriptions[member.GetterVisibility] + " getter",
                "have " + VisibilityDescriptions[visibility] + " getter"
            );
        }

        private static IOrderedCondition<PropertyMember> HaveSetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility == visibility,
                member =>
                    "does have " + VisibilityDescriptions[member.SetterVisibility] + " setter",
                "have " + VisibilityDescriptions[visibility] + " setter"
            );
        }

        private static IOrderedCondition<PropertyMember> NotHaveGetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimpleCondition<PropertyMember>(
                member => member.GetterVisibility != visibility,
                "not have " + VisibilityDescriptions[visibility] + " getter",
                "does have " + VisibilityDescriptions[visibility] + " getter"
            );
        }

        private static IOrderedCondition<PropertyMember> NotHaveSetterWithVisibility(
            Visibility visibility
        )
        {
            return new SimpleCondition<PropertyMember>(
                member => member.SetterVisibility != visibility,
                "not have " + VisibilityDescriptions[visibility] + " setter",
                "does have " + VisibilityDescriptions[visibility] + " setter"
            );
        }
    }
}
