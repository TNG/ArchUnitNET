//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsConjunction<
        TGivenRuleTypeThat,
        TRuleTypeShould,
        TGivenRuleTypeConjunctionWithReason,
        TRuleType
    > : GivenObjectsConjunctionWithDescription<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TGivenRuleTypeConjunctionWithReason Because(string reason)
        {
            _ruleCreator.AddPredicateReason(reason);
            return Create<TGivenRuleTypeConjunctionWithReason, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunctionWithReason As(string description)
        {
            _ruleCreator.SetCustomPredicateDescription(description);
            return Create<TGivenRuleTypeConjunctionWithReason, TRuleType>(_ruleCreator);
        }
    }
}
