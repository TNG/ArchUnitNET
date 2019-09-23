using System.Collections.Generic;
using System.Linq;
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

        public string Description => _firstArchRuleCreator.Description + " " + _logicalConjunction.Description + " " +
                                     _secondArchRuleCreator.Description;

        public bool Check(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_firstArchRuleCreator.Check(architecture),
                _secondArchRuleCreator.Check(architecture));
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _firstArchRuleCreator.Evaluate(architecture).Concat(_secondArchRuleCreator.Evaluate(architecture));
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
            return Equals(_firstArchRuleCreator, other._firstArchRuleCreator) &&
                   Equals(_secondArchRuleCreator, other._secondArchRuleCreator) &&
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
                hashCode = (hashCode * 397) ^ (_firstArchRuleCreator != null ? _firstArchRuleCreator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^
                           (_secondArchRuleCreator != null ? _secondArchRuleCreator.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}