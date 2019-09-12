using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRule : IArchRule
    {
        private readonly IArchRuleCreator _firstArchRuleCreator;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IArchRuleCreator _secondArchRuleCreator;

        public CombinedArchRule(IArchRuleCreator firstArchRuleCreator, LogicalConjunction logicalConjunction,
            IArchRuleCreator secondArchRuleCreator)
        {
            _firstArchRuleCreator = firstArchRuleCreator;
            _secondArchRuleCreator = secondArchRuleCreator;
            _logicalConjunction = logicalConjunction;
        }

        public bool Check(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_firstArchRuleCreator.Check(architecture),
                _secondArchRuleCreator.Check(architecture));
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