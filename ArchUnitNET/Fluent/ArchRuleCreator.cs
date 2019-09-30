using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public partial class ArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly ConditionManager<TRuleType> _conditionManager;
        private readonly ObjectFilterManager<TRuleType> _objectFilterManager;

        public ArchRuleCreator(ObjectProvider<TRuleType> objectProvider)
        {
            _objectFilterManager = new ObjectFilterManager<TRuleType>(objectProvider);
            _conditionManager = new ConditionManager<TRuleType>();
        }

        public string Description => _objectFilterManager.Description + " " + _conditionManager.Description;

        public bool HasViolations(Architecture architecture)
        {
            return HasViolations(GetFilteredObjects(architecture), architecture);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return EvaluateConditions(GetFilteredObjects(architecture), architecture);
        }

        public void AddObjectFilter(IObjectFilter<TRuleType> objectFilter)
        {
            _objectFilterManager.AddFilter(objectFilter);
        }

        public void AddObjectFilterConjunction(LogicalConjunction logicalConjunction)
        {
            _objectFilterManager.SetNextLogicalConjunction(logicalConjunction);
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

        public void AddFilterReason(string reason)
        {
            _objectFilterManager.AddReason(reason);
        }

        public void BeginComplexCondition<TReferenceType>(
            RelationCondition<TRuleType, TReferenceType> relationCondition)
            where TReferenceType : ICanBeAnalyzed
        {
            _conditionManager.BeginComplexCondition(relationCondition);
        }

        public void ContinueComplexCondition<TReferenceType>(IObjectFilter<TReferenceType> objectFilter)
            where TReferenceType : ICanBeAnalyzed
        {
            _conditionManager.ContinueComplexCondition(objectFilter);
        }

        public IEnumerable<TRuleType> GetFilteredObjects(Architecture architecture)
        {
            return _objectFilterManager.GetFilteredObjects(architecture);
        }

        private bool HasViolations(IEnumerable<TRuleType> filteredObjects, Architecture architecture)
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

            return obj.GetType() == GetType() && Equals((ArchRuleCreator<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}