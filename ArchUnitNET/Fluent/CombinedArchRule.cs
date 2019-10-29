using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRule : IArchRule
    {
        private readonly ICanBeEvaluated _firstRule;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly ICanBeEvaluated _secondRule;

        public CombinedArchRule(ICanBeEvaluated firstRule, LogicalConjunction logicalConjunction,
            ICanBeEvaluated secondRule)
        {
            _firstRule = firstRule;
            _secondRule = secondRule;
            _logicalConjunction = logicalConjunction;
        }

        public string Description => _firstRule.Description + " " + _logicalConjunction.Description + " " +
                                     _secondRule.Description;

        public bool HasNoViolations(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_firstRule.HasNoViolations(architecture),
                _secondRule.HasNoViolations(architecture));
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _firstRule.Evaluate(architecture).Concat(_secondRule.Evaluate(architecture));
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

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(CombinedArchRule other)
        {
            return Equals(_firstRule, other._firstRule) &&
                   Equals(_secondRule, other._secondRule) &&
                   Equals(_logicalConjunction, other._logicalConjunction);
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

            return obj.GetType() == GetType() && Equals((CombinedArchRule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description != null ? Description.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_firstRule != null ? _firstRule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^
                           (_secondRule != null ? _secondRule.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}