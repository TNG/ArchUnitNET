//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class ShouldRelateToFieldMembersThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToMembersThat<TRuleTypeShouldConjunction, FieldMember, TRuleType>,
        IFieldMemberPredicates<TRuleTypeShouldConjunction, FieldMember>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToFieldMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}