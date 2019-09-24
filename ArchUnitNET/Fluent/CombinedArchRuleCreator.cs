using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly ArchRuleCreator<TRuleType> _currentArchRuleCreator;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly ICanBeEvaluated _oldRule;

        public CombinedArchRuleCreator(ICanBeEvaluated oldRule, LogicalConjunction logicalConjunction,
            ObjectProvider<TRuleType> objectProvider)
        {
            _oldRule = oldRule;
            _logicalConjunction = logicalConjunction;
            _currentArchRuleCreator = new ArchRuleCreator<TRuleType>(objectProvider);
        }

        public string Description => _oldRule.Description + " " + _logicalConjunction.Description +
                                     " " + _currentArchRuleCreator.Description;

        public bool Check(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(_oldRule.Check(architecture),
                _currentArchRuleCreator.Check(architecture));
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _oldRule.Evaluate(architecture).Concat(_currentArchRuleCreator.Evaluate(architecture));
        }

        public void AddObjectFilter(IObjectFilter<TRuleType> objectFilter)
        {
            _currentArchRuleCreator.AddObjectFilter(objectFilter);
        }

        public void AddObjectFilterConjunction(LogicalConjunction logicalConjunction)
        {
            _currentArchRuleCreator.AddObjectFilterConjunction(logicalConjunction);
        }

        public void AddCondition(ICondition<TRuleType> condition)
        {
            _currentArchRuleCreator.AddCondition(condition);
        }

        public void AddConditionConjunction(LogicalConjunction logicalConjunction)
        {
            _currentArchRuleCreator.AddConditionConjunction(logicalConjunction);
        }

        public void AddConditionReason(string reason)
        {
            _currentArchRuleCreator.AddConditionReason(reason);
        }

        public void AddFilterReason(string reason)
        {
            _currentArchRuleCreator.AddFilterReason(reason);
        }

        public void BeginComplexCondition<TReferenceType>(
            RelationCondition<TRuleType, TReferenceType> relationCondition) where TReferenceType : ICanBeAnalyzed
        {
            _currentArchRuleCreator.BeginComplexCondition(relationCondition);
        }

        public void ContinueComplexCondition<TReferenceType>(ObjectProvider<TReferenceType> referenceObjectProvider,
            IObjectFilter<TReferenceType> objectFilter) where TReferenceType : ICanBeAnalyzed
        {
            _currentArchRuleCreator.ContinueComplexCondition(referenceObjectProvider, objectFilter);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(CombinedArchRuleCreator<TRuleType> other)
        {
            return string.Equals(Description, other.Description) &&
                   Equals(_oldRule, other._oldRule) &&
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

            return obj.GetType() == GetType() && Equals((CombinedArchRuleCreator<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description != null ? Description.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_oldRule != null ? _oldRule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}