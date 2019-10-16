using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public partial class ArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly ConditionManager<TRuleType> _conditionManager;
        private readonly PredicateManager<TRuleType> _predicateManager;

        public ArchRuleCreator(BasicObjectProvider<TRuleType> basicObjectProvider)
        {
            _predicateManager = new PredicateManager<TRuleType>(basicObjectProvider);
            _conditionManager = new ConditionManager<TRuleType>();
        }

        public string Description => (_predicateManager.Description + " " + _conditionManager.Description).Trim();

        public bool HasNoViolations(Architecture architecture)
        {
            return HasNoViolations(GetAnalyzedObjects(architecture), architecture);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return EvaluateConditions(GetAnalyzedObjects(architecture), architecture);
        }

        public void AddPredicate(IPredicate<TRuleType> predicate)
        {
            _predicateManager.AddPredicate(predicate);
        }

        public void AddPredicateConjunction(LogicalConjunction logicalConjunction)
        {
            _predicateManager.SetNextLogicalConjunction(logicalConjunction);
        }

        public void AddCondition(ICondition<TRuleType> condition)
        {
            _conditionManager.AddCondition(condition);
        }

        public void AddConditionConjunction(LogicalConjunction logicalConjunction)
        {
            _conditionManager.SetNextLogicalConjunction(logicalConjunction);
        }

        public void AddConditionReason(string reason)
        {
            _conditionManager.AddReason(reason);
        }

        public void AddPredicateReason(string reason)
        {
            _predicateManager.AddReason(reason);
        }

        public void BeginComplexCondition<TReferenceType>(
            RelationCondition<TRuleType, TReferenceType> relationCondition)
            where TReferenceType : ICanBeAnalyzed
        {
            _conditionManager.BeginComplexCondition(relationCondition);
        }

        public void ContinueComplexCondition<TReferenceType>(IPredicate<TReferenceType> predicate)
            where TReferenceType : ICanBeAnalyzed
        {
            _conditionManager.ContinueComplexCondition(predicate);
        }

        public IEnumerable<TRuleType> GetAnalyzedObjects(Architecture architecture)
        {
            return _predicateManager.GetObjects(architecture);
        }

        public void SetCustomPredicateDescription(string description)
        {
            _predicateManager.SetCustomDescription(description);
        }

        public void SetCustomConditionDescription(string description)
        {
            _conditionManager.SetCustomDescription(description);
        }

        private bool HasNoViolations(IEnumerable<TRuleType> filteredObjects, Architecture architecture)
        {
            return _conditionManager.CheckConditions(filteredObjects, architecture);
        }

        private IEnumerable<EvaluationResult> EvaluateConditions(IEnumerable<TRuleType> filteredObjects,
            Architecture architecture)
        {
            return _conditionManager.EvaluateConditions(filteredObjects, architecture, this);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ArchRuleCreator<TRuleType> other)
        {
            return _conditionManager.Equals(other._conditionManager) &&
                   _predicateManager.Equals(other._predicateManager);
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

            return obj.GetType() == GetType() && Equals((ArchRuleCreator<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (_conditionManager != null ? _conditionManager.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_predicateManager != null ? _predicateManager.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}