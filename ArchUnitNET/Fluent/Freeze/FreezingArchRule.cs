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
                var frozenViolations = new HashSet<string>(
                    _violationStore
                        .GetFrozenViolations(_frozenRule)
                        .Select(violation => violation.Identifier)
                );
                var stillUnresolvedViolations = new List<StringIdentifier>();
                foreach (var evalResult in evalResults)
                {
                    if (!evalResult.Passed && IsFrozen(evalResult, frozenViolations))
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

        /// <summary>
        /// Decides whether a failing result is covered by the frozen violations.
        /// </summary>
        /// <remarks>
        /// Rules frozen before assembly-aware identifiers contain only the full name of a type or
        /// member. Such a legacy entry freezes every failing object with that full name, also if
        /// there are several of them in different assemblies; the old identifiers could not tell
        /// them apart either. The next write replaces the entry with the new identifiers of all
        /// these objects, so each rule migrates on its first run.
        ///
        /// Only results whose identifier is exactly the new form of their evaluated object take
        /// part in this fallback; see <see cref="LegacyFullName"/>.
        /// Custom rules may pair an analyzed object with an identifier of their own, whose earlier
        /// form was not necessarily the full name.
        ///
        /// Legacy entries need no marker: an identifier in the new form always contains
        /// <c>", "</c> before the assembly name, while full names never do (Mono.Cecil and the
        /// loader separate parameters and generic arguments with a bare <c>","</c>). So an entry
        /// can only equal a full name if it was written in the legacy form. This also migrates
        /// entries of custom stores.
        /// </remarks>
        private static bool IsFrozen(EvaluationResult result, HashSet<string> frozenViolations)
        {
            if (frozenViolations.Contains(result.EvaluatedObjectIdentifier.Identifier))
            {
                return true;
            }

            var fullName = LegacyFullName(result);
            return fullName != null && frozenViolations.Contains(fullName);
        }

        /// <summary>
        /// The identifier the result would have had in a legacy store: the full name of the
        /// evaluated object, if the result identifies it in the new form <c>FullName, Assembly</c>
        /// that the fluent rules use.
        /// Otherwise <c>null</c>, and the result can only match a frozen entry exactly.
        /// </summary>
        private static string LegacyFullName(EvaluationResult result)
        {
            return
                result.EvaluatedObject is ICanBeAnalyzed analyzedObject
                && result.EvaluatedObjectIdentifier.Identifier
                    == analyzedObject.FullName + ", " + (analyzedObject.Assembly?.Name ?? "")
                ? analyzedObject.FullName
                : null;
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
