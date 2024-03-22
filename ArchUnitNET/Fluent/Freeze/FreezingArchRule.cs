//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Freeze
{
    public class FreezingArchRule : IArchRule
    {
        private readonly IArchRule _frozenRule;
        private readonly IViolationStore _violationStore;

        private FreezingArchRule(IArchRule rule, IViolationStore violationStore)
        {
            _frozenRule = rule;
            _violationStore = violationStore;
        }

        public string Description => "Freeze(" + _frozenRule.Description + ")";

        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            var evalResults = _frozenRule.Evaluate(architecture).ToList();
            var evalResultsIgnoringFrozen = new List<EvaluationResult>();
            var identifiersOfFailedResults = evalResults
                .Where(result => !result.Passed)
                .Select(result => result.EvaluatedObjectIdentifier);

            if (!_violationStore.RuleAlreadyFrozen(_frozenRule))
            {
                _violationStore.StoreCurrentViolations(_frozenRule, identifiersOfFailedResults);
                evalResultsIgnoringFrozen.AddRange(evalResults.Select(MarkAsPassed));
            }
            else
            {
                var frozenViolations = _violationStore.GetFrozenViolations(_frozenRule).ToList();
                var stillUnresolvedViolations = new List<StringIdentifier>();
                foreach (var evalResult in evalResults)
                {
                    if (
                        frozenViolations.Contains(
                            evalResult.EvaluatedObjectIdentifier,
                            new StringIdentifierComparer()
                        ) && !evalResult.Passed
                    )
                    {
                        evalResultsIgnoringFrozen.Add(MarkAsPassed(evalResult));
                        stillUnresolvedViolations.Add(evalResult.EvaluatedObjectIdentifier);
                    }
                    else
                    {
                        evalResultsIgnoringFrozen.Add(evalResult);
                    }
                }

                _violationStore.StoreCurrentViolations(_frozenRule, stillUnresolvedViolations);
            }

            return evalResultsIgnoringFrozen;
        }

        public CombinedArchRuleDefinition And()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.And);
        }

        public CombinedArchRuleDefinition Or()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.Or);
        }

        public IArchRule And(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.And, archRule);
        }

        public IArchRule Or(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.Or, archRule);
        }

        public static FreezingArchRule Freeze(IArchRule rule, IViolationStore violationStore)
        {
            return new FreezingArchRule(rule, violationStore);
        }

        public static FreezingArchRule Freeze(IArchRule rule, string storagePath)
        {
            return new FreezingArchRule(rule, new JsonViolationStore(storagePath));
        }

        public static FreezingArchRule Freeze(IArchRule rule)
        {
            return new FreezingArchRule(rule, new JsonViolationStore());
        }

        private static EvaluationResult MarkAsPassed(EvaluationResult evaluationResult)
        {
            return new EvaluationResult(
                evaluationResult.EvaluatedObject,
                evaluationResult.EvaluatedObjectIdentifier,
                true,
                evaluationResult.Description,
                evaluationResult.ArchRule,
                evaluationResult.Architecture
            );
        }
    }
}
