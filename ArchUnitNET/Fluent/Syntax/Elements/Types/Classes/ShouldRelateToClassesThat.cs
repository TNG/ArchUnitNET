//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ShouldRelateToClassesThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, Class, TRuleType>,
        IClassPredicates<TRuleTypeShouldConjunction, Class> where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToClassesThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(ClassPredicatesDefinition.AreAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreSealed()
        {
            _ruleCreator.ContinueComplexCondition(ClassPredicatesDefinition.AreSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(ClassPredicatesDefinition.AreNotAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotSealed()
        {
            _ruleCreator.ContinueComplexCondition(ClassPredicatesDefinition.AreNotSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}