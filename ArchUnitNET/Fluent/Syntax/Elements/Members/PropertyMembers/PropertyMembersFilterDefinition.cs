using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMembersFilterDefinition
    {
        public static ObjectFilter<PropertyMember> HaveGetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.Visibility != NotAccessible, "have getter");
        }

        public static ObjectFilter<PropertyMember> HaveSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != NotAccessible, "have setter");
        }

        public static ObjectFilter<PropertyMember> HavePrivateSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == Private, "have private setter");
        }

        public static ObjectFilter<PropertyMember> HavePublicSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == Public, "have public setter");
        }

        public static ObjectFilter<PropertyMember> HaveProtectedSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == Protected, "have protected setter");
        }

        public static ObjectFilter<PropertyMember> HaveInternalSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == Internal, "have internal setter");
        }

        public static ObjectFilter<PropertyMember> HaveProtectedInternalSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == ProtectedInternal,
                "have protected internal setter");
        }

        public static ObjectFilter<PropertyMember> HavePrivateProtectedSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == PrivateProtected,
                "have private protected setter");
        }

        public static ObjectFilter<PropertyMember> AreVirtual()
        {
            return new ObjectFilter<PropertyMember>(member => member.IsVirtual, "are virtual");
        }

        //Negations


        public static ObjectFilter<PropertyMember> HaveNoGetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.Visibility == NotAccessible, "have no getter");
        }

        public static ObjectFilter<PropertyMember> HaveNoSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility == NotAccessible,
                "have no setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHavePrivateSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != Private,
                "do not have private setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHavePublicSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != Public, "do not have public setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHaveProtectedSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != Protected,
                "do not have protected setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHaveInternalSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != Internal,
                "do not have internal setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHaveProtectedInternalSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != ProtectedInternal,
                "do not have protected internal setter");
        }

        public static ObjectFilter<PropertyMember> DoNotHavePrivateProtectedSetter()
        {
            return new ObjectFilter<PropertyMember>(member => member.SetterVisibility != PrivateProtected,
                "do not have private protected setter");
        }

        public static ObjectFilter<PropertyMember> AreNotVirtual()
        {
            return new ObjectFilter<PropertyMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}