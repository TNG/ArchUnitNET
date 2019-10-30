//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface
        IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType> : IObjectConditions<TRuleTypeShouldConjunction>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyClassesThat();
        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> DependOnAnyInterfacesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> OnlyDependOnInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnAnyTypesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat();


        //Negations


        ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyClassesThat();
        ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> NotDependOnAnyInterfacesThat();
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnAnyTypesThat();
        ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat();
    }
}