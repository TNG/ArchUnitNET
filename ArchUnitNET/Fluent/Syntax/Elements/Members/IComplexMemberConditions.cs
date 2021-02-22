//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface
        IComplexMemberConditions<TRuleTypeShouldConjunction, TRuleType> :
            IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>,
            IMemberConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> BeDeclaredInTypesThat();


        //Negations


        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotBeDeclaredInTypesThat();
    }
}