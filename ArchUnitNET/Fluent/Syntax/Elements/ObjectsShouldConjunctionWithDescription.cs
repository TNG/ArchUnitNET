//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShouldConjunctionWithDescription<TRuleTypeShould, TRuleType>
        : ArchRule<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunctionWithDescription(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShould AndShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.And);
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould OrShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.Or);
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}
