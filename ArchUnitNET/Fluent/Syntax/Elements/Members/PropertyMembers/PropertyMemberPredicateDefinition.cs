using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberPredicateDefinition
    {
        public static IPredicate<PropertyMember> HaveGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.Visibility != NotAccessible, "have getter");
        }

        public static IPredicate<PropertyMember> HaveSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != NotAccessible,
                "have setter");
        }

        public static IPredicate<PropertyMember> HavePrivateSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == Private,
                "have private setter");
        }

        public static IPredicate<PropertyMember> HavePublicSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == Public,
                "have public setter");
        }

        public static IPredicate<PropertyMember> HaveProtectedSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == Protected,
                "have protected setter");
        }

        public static IPredicate<PropertyMember> HaveInternalSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == Internal,
                "have internal setter");
        }

        public static IPredicate<PropertyMember> HaveProtectedInternalSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == ProtectedInternal,
                "have protected internal setter");
        }

        public static IPredicate<PropertyMember> HavePrivateProtectedSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == PrivateProtected,
                "have private protected setter");
        }

        public static IPredicate<PropertyMember> AreVirtual()
        {
            return new SimplePredicate<PropertyMember>(member => member.IsVirtual, "are virtual");
        }

        //Negations


        public static IPredicate<PropertyMember> HaveNoGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.Visibility == NotAccessible, "have no getter");
        }

        public static IPredicate<PropertyMember> HaveNoSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility == NotAccessible,
                "have no setter");
        }

        public static IPredicate<PropertyMember> DoNotHavePrivateSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != Private,
                "do not have private setter");
        }

        public static IPredicate<PropertyMember> DoNotHavePublicSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != Public,
                "do not have public setter");
        }

        public static IPredicate<PropertyMember> DoNotHaveProtectedSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != Protected,
                "do not have protected setter");
        }

        public static IPredicate<PropertyMember> DoNotHaveInternalSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != Internal,
                "do not have internal setter");
        }

        public static IPredicate<PropertyMember> DoNotHaveProtectedInternalSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != ProtectedInternal,
                "do not have protected internal setter");
        }

        public static IPredicate<PropertyMember> DoNotHavePrivateProtectedSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.SetterVisibility != PrivateProtected,
                "do not have private protected setter");
        }

        public static IPredicate<PropertyMember> AreNotVirtual()
        {
            return new SimplePredicate<PropertyMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}