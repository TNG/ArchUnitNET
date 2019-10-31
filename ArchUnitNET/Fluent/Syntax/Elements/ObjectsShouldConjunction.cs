//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        ObjectsShouldConjunction<TRuleTypeShould, TRuleTypeShouldConjunctionWithReason, TRuleType> :
            ObjectsShouldConjunctionWithDescription<TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunctionWithReason Because(string reason)
        {
            _ruleCreator.AddConditionReason(reason);
            return Create<TRuleTypeShouldConjunctionWithReason, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunctionWithReason As(string description)
        {
            _ruleCreator.SetCustomConditionDescription(description);
            return Create<TRuleTypeShouldConjunctionWithReason, TRuleType>(_ruleCreator);
        }
    }
}