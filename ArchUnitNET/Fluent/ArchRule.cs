using System.Collections.Generic;
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
            return _ruleCreator.HasNoViolations(architecture);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _ruleCreator.Evaluate(architecture);
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
    }
}
