using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceRule : IArchRule
    {
        private readonly SliceRuleCreator _ruleCreator;

        public SliceRule(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public string Description => _ruleCreator.Description;

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
