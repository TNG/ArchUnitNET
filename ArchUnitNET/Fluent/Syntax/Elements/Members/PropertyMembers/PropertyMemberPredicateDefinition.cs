using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberPredicateDefinition
    {
        public static Predicate<PropertyMember> HaveGetter()
        {
            return new Predicate<PropertyMember>(member => member.Visibility != NotAccessible, "have getter");
        }

        public static Predicate<PropertyMember> HaveSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != NotAccessible, "have setter");
        }

        public static Predicate<PropertyMember> HavePrivateSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == Private,
                "have private setter");
        }

        public static Predicate<PropertyMember> HavePublicSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == Public, "have public setter");
        }

        public static Predicate<PropertyMember> HaveProtectedSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == Protected,
                "have protected setter");
        }

        public static Predicate<PropertyMember> HaveInternalSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == Internal,
                "have internal setter");
        }

        public static Predicate<PropertyMember> HaveProtectedInternalSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == ProtectedInternal,
                "have protected internal setter");
        }

        public static Predicate<PropertyMember> HavePrivateProtectedSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == PrivateProtected,
                "have private protected setter");
        }

        public static Predicate<PropertyMember> AreVirtual()
        {
            return new Predicate<PropertyMember>(member => member.IsVirtual, "are virtual");
        }

        //Negations


        public static Predicate<PropertyMember> HaveNoGetter()
        {
            return new Predicate<PropertyMember>(member => member.Visibility == NotAccessible, "have no getter");
        }

        public static Predicate<PropertyMember> HaveNoSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility == NotAccessible,
                "have no setter");
        }

        public static Predicate<PropertyMember> DoNotHavePrivateSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != Private,
                "do not have private setter");
        }

        public static Predicate<PropertyMember> DoNotHavePublicSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != Public,
                "do not have public setter");
        }

        public static Predicate<PropertyMember> DoNotHaveProtectedSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != Protected,
                "do not have protected setter");
        }

        public static Predicate<PropertyMember> DoNotHaveInternalSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != Internal,
                "do not have internal setter");
        }

        public static Predicate<PropertyMember> DoNotHaveProtectedInternalSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != ProtectedInternal,
                "do not have protected internal setter");
        }

        public static Predicate<PropertyMember> DoNotHavePrivateProtectedSetter()
        {
            return new Predicate<PropertyMember>(member => member.SetterVisibility != PrivateProtected,
                "do not have private protected setter");
        }

        public static Predicate<PropertyMember> AreNotVirtual()
        {
            return new Predicate<PropertyMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}