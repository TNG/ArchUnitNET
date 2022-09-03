//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public static class PropertyMemberPredicateDefinition
    {
        public static IPredicate<PropertyMember> HaveGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != NotAccessible,
                "have getter");
        }

        public static IPredicate<PropertyMember> HavePrivateGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == Private,
                "have private getter");
        }

        public static IPredicate<PropertyMember> HavePublicGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == Public,
                "have public getter");
        }

        public static IPredicate<PropertyMember> HaveProtectedGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == Protected,
                "have protected getter");
        }

        public static IPredicate<PropertyMember> HaveInternalGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == Internal,
                "have internal getter");
        }

        public static IPredicate<PropertyMember> HaveProtectedInternalGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == ProtectedInternal,
                "have protected internal getter");
        }

        public static IPredicate<PropertyMember> HavePrivateProtectedGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == PrivateProtected,
                "have private protected getter");
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

        public static IPredicate<PropertyMember> HaveInitSetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.IsInitSetter,
                "have an init setter");
        }

        public static IPredicate<PropertyMember> AreImmutable()
        {
            return new SimplePredicate<PropertyMember>(member => member.IsInitSetter || member.IsReadOnly == true,
                "are immutable");
        }

        public static IPredicate<PropertyMember> AreVirtual()
        {
            return new SimplePredicate<PropertyMember>(member => member.IsVirtual, "are virtual");
        }

        //Negations


        public static IPredicate<PropertyMember> HaveNoGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility == NotAccessible,
                "have no getter");
        }


        public static IPredicate<PropertyMember> DoNotHavePrivateGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != Private,
                "do not have private getter");
        }

        public static IPredicate<PropertyMember> DoNotHavePublicGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != Public,
                "do not have public getter");
        }

        public static IPredicate<PropertyMember> DoNotHaveProtectedGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != Protected,
                "do not have protected getter");
        }

        public static IPredicate<PropertyMember> DoNotHaveInternalGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != Internal,
                "do not have internal getter");
        }

        public static IPredicate<PropertyMember> DoNotHaveProtectedInternalGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != ProtectedInternal,
                "do not have protected internal getter");
        }

        public static IPredicate<PropertyMember> DoNotHavePrivateProtectedGetter()
        {
            return new SimplePredicate<PropertyMember>(member => member.GetterVisibility != PrivateProtected,
                "do not have private protected getter");
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

        public static IPredicate<PropertyMember> DoNotHaveInitSetter()
        {
            return new SimplePredicate<PropertyMember>(member => !member.IsInitSetter,
                "do not have an init setter");
        }

        public static IPredicate<PropertyMember> AreNotImmutable()
        {
            return new SimplePredicate<PropertyMember>(member => !member.IsInitSetter && member.IsReadOnly != true,
                "are not immutable");
        }

        public static IPredicate<PropertyMember> AreNotVirtual()
        {
            return new SimplePredicate<PropertyMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}