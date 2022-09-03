//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface
        IPropertyMemberPredicates<out TRuleTypeConjunction, TRuleType> : IMemberPredicates<TRuleTypeConjunction,
            TRuleType> where TRuleType : ICanBeAnalyzed
    {
        TRuleTypeConjunction HaveGetter();
        TRuleTypeConjunction HavePrivateGetter();
        TRuleTypeConjunction HavePublicGetter();
        TRuleTypeConjunction HaveProtectedGetter();
        TRuleTypeConjunction HaveInternalGetter();
        TRuleTypeConjunction HaveProtectedInternalGetter();
        TRuleTypeConjunction HavePrivateProtectedGetter();
        TRuleTypeConjunction HaveSetter();
        TRuleTypeConjunction HavePrivateSetter();
        TRuleTypeConjunction HavePublicSetter();
        TRuleTypeConjunction HaveProtectedSetter();
        TRuleTypeConjunction HaveInternalSetter();
        TRuleTypeConjunction HaveProtectedInternalSetter();
        TRuleTypeConjunction HavePrivateProtectedSetter();
        TRuleTypeConjunction HaveInitSetter();
        TRuleTypeConjunction AreVirtual();


        //Negations


        TRuleTypeConjunction HaveNoGetter();
        TRuleTypeConjunction DoNotHavePrivateGetter();
        TRuleTypeConjunction DoNotHavePublicGetter();
        TRuleTypeConjunction DoNotHaveProtectedGetter();
        TRuleTypeConjunction DoNotHaveInternalGetter();
        TRuleTypeConjunction DoNotHaveProtectedInternalGetter();
        TRuleTypeConjunction DoNotHavePrivateProtectedGetter();
        TRuleTypeConjunction HaveNoSetter();
        TRuleTypeConjunction DoNotHavePrivateSetter();
        TRuleTypeConjunction DoNotHavePublicSetter();
        TRuleTypeConjunction DoNotHaveProtectedSetter();
        TRuleTypeConjunction DoNotHaveInternalSetter();
        TRuleTypeConjunction DoNotHaveProtectedInternalSetter();
        TRuleTypeConjunction DoNotHavePrivateProtectedSetter();
        TRuleTypeConjunction DoNotHaveInitSetter();
        TRuleTypeConjunction AreNotVirtual();
    }
}