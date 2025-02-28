using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Exceptions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent
{
    public class CombinedArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly ArchRuleCreator<TRuleType> _currentArchRuleCreator;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly ICanBeEvaluated _oldRule;
        private bool? _requirePositiveResults;

        public CombinedArchRuleCreator(
            ICanBeEvaluated oldRule,
            LogicalConjunction logicalConjunction,
            BasicObjectProvider<TRuleType> basicObjectProvider
        )
        {
            _oldRule = oldRule;
            _logicalConjunction = logicalConjunction;
            _currentArchRuleCreator = new ArchRuleCreator<TRuleType>(basicObjectProvider);
        }

        public string Description =>
            _oldRule.Description
            + " "
            + _logicalConjunction.Description
            + " "
            + _currentArchRuleCreator.Description;

        public bool HasNoViolations(Architecture architecture)
        {
            return _logicalConjunction.Evaluate(
                _oldRule.HasNoViolations(architecture),
                _currentArchRuleCreator.HasNoViolations(architecture)
            );
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return _oldRule
                .Evaluate(architecture)
                .Concat(_currentArchRuleCreator.Evaluate(architecture));
        }

        public void AddPredicate(IPredicate<TRuleType> predicate)
        {
            _currentArchRuleCreator.AddPredicate(predicate);
        }

        public void AddPredicateConjunction(LogicalConjunction logicalConjunction)
        {
            _currentArchRuleCreator.AddPredicateConjunction(logicalConjunction);
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

        public void AddPredicateReason(string reason)
        {
            _currentArchRuleCreator.AddPredicateReason(reason);
        }

        public void BeginComplexCondition<TRelatedType>(
            IObjectProvider<TRelatedType> relatedObjects,
            RelationCondition<TRuleType, TRelatedType> relationCondition
        )
            where TRelatedType : ICanBeAnalyzed
        {
            _currentArchRuleCreator.BeginComplexCondition(relatedObjects, relationCondition);
        }

        public void ContinueComplexCondition<TRelatedType>(IPredicate<TRelatedType> predicate)
            where TRelatedType : ICanBeAnalyzed
        {
            _currentArchRuleCreator.ContinueComplexCondition(predicate);
        }

        public IEnumerable<TRuleType> GetAnalyzedObjects(Architecture architecture)
        {
            throw new CannotGetObjectsOfCombinedArchRuleCreatorException(
                "GetFilteredObjects() can't be used with CombinedArchRuleCreators because the analyzed objects might be of different type."
            );
        }

        public void SetCustomPredicateDescription(string description)
        {
            _currentArchRuleCreator.SetCustomPredicateDescription(description);
        }

        public void SetCustomConditionDescription(string description)
        {
            _currentArchRuleCreator.SetCustomConditionDescription(description);
        }

        private void SetRequirePositiveResults(bool requirePositive)
        {
            if (_requirePositiveResults != null && _requirePositiveResults != requirePositive)
                throw new InvalidOperationException("conflicting positive expectation");
            _requirePositiveResults = requirePositive;
        }

        public bool RequirePositiveResults
        {
            get => _requirePositiveResults ?? true;
            set => SetRequirePositiveResults(value);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(CombinedArchRuleCreator<TRuleType> other)
        {
            return Equals(_oldRule, other._oldRule)
                && Equals(_logicalConjunction, other._logicalConjunction)
                && Equals(_currentArchRuleCreator, other._currentArchRuleCreator);
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

            return obj.GetType() == GetType() && Equals((CombinedArchRuleCreator<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _oldRule != null ? _oldRule.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397)
                    ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                hashCode =
                    (hashCode * 397)
                    ^ (_currentArchRuleCreator != null ? _currentArchRuleCreator.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
