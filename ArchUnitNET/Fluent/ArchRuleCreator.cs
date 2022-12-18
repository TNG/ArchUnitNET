//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent
{
    public class ArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly ConditionManager<TRuleType> _conditionManager;
        private readonly PredicateManager<TRuleType> _predicateManager;
        private bool? _requirePositiveResults;

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

        public void BeginComplexCondition<TRelatedType>(IObjectProvider<TRelatedType> relatedObjects,
            RelationCondition<TRuleType, TRelatedType> relationCondition)
            where TRelatedType : ICanBeAnalyzed
        {
            _conditionManager.BeginComplexCondition(relatedObjects, relationCondition);
        }

        public void ContinueComplexCondition<TRelatedType>(IPredicate<TRelatedType> predicate)
            where TRelatedType : ICanBeAnalyzed
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

        private void SetRequirePositiveResults(bool requirePositive)
        {
            if (_requirePositiveResults != null &&
                _requirePositiveResults != requirePositive)
                throw new InvalidOperationException("conflicting positive expectation");
            _requirePositiveResults = requirePositive;
        }

        public bool RequirePositiveResults
        {
            get => _requirePositiveResults ?? true;
            set => SetRequirePositiveResults(value);
        }

        private bool HasNoViolations(IEnumerable<TRuleType> filteredObjects, Architecture architecture)
        {
            return EvaluateConditions(filteredObjects, architecture).All(result => result.Passed);
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