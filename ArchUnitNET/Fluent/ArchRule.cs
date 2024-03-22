//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class ArchRule<TRuleType> : SyntaxElement<TRuleType>, IArchRule
        where TRuleType : ICanBeAnalyzed
    {
        protected ArchRule(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        /// <summary>
        /// By default, rules are evaluated so positive results are required to be present.
        /// This call defeats this check on the rule.
        /// </summary>
        public ArchRule<TRuleType> WithoutRequiringPositiveResults()
        {
            _ruleCreator.RequirePositiveResults = false;
            return this;
        }

        public bool HasNoViolations(Architecture architecture)
        {
            if (_ruleCreator.RequirePositiveResults)
            {
                return Evaluate(architecture).All(e => e.Passed);
            }
            else
            {
                return _ruleCreator.HasNoViolations(architecture);
            }
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            var result = _ruleCreator.Evaluate(architecture).ToList();

            // To require positives, we only ever need to add
            // a non-passing result if there are no results.
            if (_ruleCreator.RequirePositiveResults && result.Count == 0)
            {
                result.Add(
                    new EvaluationResult(
                        this,
                        new StringIdentifier(Description),
                        false,
                        $"The rule requires positive evaluation, not just absence of violations. Use {nameof(WithoutRequiringPositiveResults)}() or improve your rule's predicates.",
                        this,
                        architecture
                    )
                );
            }

            return result;
        }

        public CombinedArchRuleDefinition And()
        {
            return new CombinedArchRuleDefinition(_ruleCreator, LogicalConjunctionDefinition.And);
        }

        public CombinedArchRuleDefinition Or()
        {
            return new CombinedArchRuleDefinition(_ruleCreator, LogicalConjunctionDefinition.Or);
        }

        public IArchRule And(IArchRule archRule)
        {
            return new CombinedArchRule(_ruleCreator, LogicalConjunctionDefinition.And, archRule);
        }

        public IArchRule Or(IArchRule archRule)
        {
            return new CombinedArchRule(_ruleCreator, LogicalConjunctionDefinition.Or, archRule);
        }
    }
}
