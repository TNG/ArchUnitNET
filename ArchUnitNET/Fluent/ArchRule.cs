using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class ArchRule<TRuleType> : SyntaxElement<TRuleType>, IArchRule where TRuleType : ICanBeAnalyzed
    {
        protected ArchRule(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
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

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ArchRule<TRuleType> other)
        {
            return string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((ArchRule<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}