//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        : IObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > DependOnAnyTypesThat();
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > OnlyDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAnyAttributesThat();
        ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > OnlyHaveAttributesThat();

        //Negations

        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotDependOnAnyTypesThat();
        ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > NotHaveAnyAttributesThat();
    }
}
