//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface
        IPropertyMemberConditions<out TReturnType, out TRuleType> : IMemberConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType HaveGetter();
        TReturnType HavePrivateGetter();
        TReturnType HavePublicGetter();
        TReturnType HaveProtectedGetter();
        TReturnType HaveInternalGetter();
        TReturnType HaveProtectedInternalGetter();
        TReturnType HavePrivateProtectedGetter();
        TReturnType HaveSetter();
        TReturnType HavePrivateSetter();
        TReturnType HavePublicSetter();
        TReturnType HaveProtectedSetter();
        TReturnType HaveInternalSetter();
        TReturnType HaveProtectedInternalSetter();
        TReturnType HavePrivateProtectedSetter();
        TReturnType BeVirtual();


        //Negations


        TReturnType NotHaveGetter();
        TReturnType NotHavePrivateGetter();
        TReturnType NotHavePublicGetter();
        TReturnType NotHaveProtectedGetter();
        TReturnType NotHaveInternalGetter();
        TReturnType NotHaveProtectedInternalGetter();
        TReturnType NotHavePrivateProtectedGetter();
        TReturnType NotHaveSetter();
        TReturnType NotHavePrivateSetter();
        TReturnType NotHavePublicSetter();
        TReturnType NotHaveProtectedSetter();
        TReturnType NotHaveInternalSetter();
        TReturnType NotHaveProtectedInternalSetter();
        TReturnType NotHavePrivateProtectedSetter();
        TReturnType NotBeVirtual();
    }
}