using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Syntax;
using static ArchUnitNET.Fluent.Syntax.LogicalConjunctionDefinition;

#pragma warning disable 693

namespace ArchUnitNET.Fluent
{
    public class ArchRuleCreator<TRuleType> : IArchRuleCreator<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly ConditionManager<TRuleType> _conditionManager;
        private readonly ObjectFilterManager<TRuleType> _objectFilterManager;

        public ArchRuleCreator(ObjectProvider<TRuleType> objectProvider)
        {
            _objectFilterManager = new ObjectFilterManager<TRuleType>(objectProvider);
            _conditionManager = new ConditionManager<TRuleType>();
        }

        public string Description => _objectFilterManager.Description + " " + _conditionManager.Description;

        public bool Check(Architecture architecture)
        {
            return CheckConditions(GetFilteredObjects(architecture), architecture);
        }

        public IEnumerable<IEvaluationResult> Evaluate(Architecture architecture)
        {
            return EvaluateConditions(GetFilteredObjects(architecture), architecture);
        }

        public void AddObjectFilter(ObjectFilter<TRuleType> objectFilter)
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

        public void ContinueComplexCondition<TReferenceType>(ObjectProvider<TReferenceType> referenceObjectProvider,
            ObjectFilter<TReferenceType> objectFilter)
            where TReferenceType : ICanBeAnalyzed
        {
            _conditionManager.ContinueComplexCondition(referenceObjectProvider, objectFilter);
        }

        private IEnumerable<TRuleType> GetFilteredObjects(Architecture architecture)
        {
            return _objectFilterManager.GetFilteredObjects(architecture);
        }

        private bool CheckConditions(IEnumerable<TRuleType> filteredObjects, Architecture architecture)
        {
            return _conditionManager.CheckConditions(filteredObjects, architecture);
        }

        private IEnumerable<EvaluationResult<TRuleType>> EvaluateConditions(IEnumerable<TRuleType> filteredObjects,
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

        private class ObjectFilterManager<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private readonly List<ObjectFilterElement<T>> _objectFilterElements;
            private readonly ObjectProvider<T> _objectProvider;

            internal ObjectFilterManager(ObjectProvider<T> objectProvider)
            {
                _objectProvider = objectProvider;
                _objectFilterElements = new List<ObjectFilterElement<T>>
                {
                    new ObjectFilterElement<T>(ForwardSecondValue, new ObjectFilter<T>(t => true, "not set"))
                };
            }

            public string Description => _objectFilterElements.First().Description == "not set"
                ? _objectProvider.Description
                : _objectProvider.Description + " that" + _objectFilterElements.Aggregate("",
                      (current, objectFilterElement) => current + " " + objectFilterElement.Description);

            internal IEnumerable<T> GetFilteredObjects(Architecture architecture)
            {
                return _objectProvider.GetObjects(architecture).Where(obj => _objectFilterElements.Aggregate(true,
                    (currentResult, objectFilterElement) => objectFilterElement.CheckFilter(currentResult, obj)));
            }

            internal void AddFilter(ObjectFilter<T> objectFilter)
            {
                _objectFilterElements.Last().SetFilter(objectFilter);
            }

            public void AddReason(string reason)
            {
                _objectFilterElements.Last().AddReason(reason);
            }

            internal void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _objectFilterElements.Add(new ObjectFilterElement<T>(logicalConjunction));
            }

            public override string ToString()
            {
                return Description;
            }

            private class ObjectFilterElement<T> : IHasDescription where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ObjectFilter<T> _objectFilter;
                private string _reason;

                public ObjectFilterElement(LogicalConjunction logicalConjunction, ObjectFilter<T> objectFilter = null)
                {
                    _objectFilter = objectFilter;
                    _logicalConjunction = logicalConjunction;
                    _reason = "";
                }

                public string Description => _objectFilter == null
                    ? _logicalConjunction.Description
                    : (_logicalConjunction.Description + " " + _objectFilter.Description + " " + _reason).Trim();

                public void AddReason(string reason)
                {
                    if (_objectFilter == null)
                    {
                        throw new InvalidOperationException(
                            "Can't add a reason to an ObjectFilterElement before the filter is set.");
                    }

                    if (_reason != "")
                    {
                        throw new InvalidOperationException(
                            "Can't add a reason to an ObjectFilterElement which already has a reason.");
                    }

                    _reason = "because " + reason;
                }

                public void SetFilter(ObjectFilter<T> objectFilter)
                {
                    _objectFilter = objectFilter;
                }

                public bool CheckFilter(bool currentResult, T obj)
                {
                    if (_objectFilter == null)
                    {
                        throw new InvalidOperationException(
                            "Can't Evaluate an ObjectFilterElement before the filter is set.");
                    }

                    return _logicalConjunction.Evaluate(currentResult, _objectFilter.CheckFilter(obj));
                }

                public override string ToString()
                {
                    return Description;
                }
            }
        }

        private class ConditionManager<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private readonly List<ConditionElement<T>> _conditionElements;
            private object _relationConditionTemp;

            internal ConditionManager()
            {
                _conditionElements = new List<ConditionElement<T>>
                {
                    new ConditionElement<T>(ForwardSecondValue)
                };
            }

            public string Description => _conditionElements.Aggregate("",
                (current, conditionElement) => current + " " + conditionElement.Description).Trim();

            internal void BeginComplexCondition<TReferenceType>(RelationCondition<T, TReferenceType> relationCondition)
                where TReferenceType : ICanBeAnalyzed
            {
                _relationConditionTemp = relationCondition;
            }

            internal void ContinueComplexCondition<TReferenceType>(
                ObjectProvider<TReferenceType> referenceObjectProvider,
                ObjectFilter<TReferenceType> filter)
                where TReferenceType : ICanBeAnalyzed
            {
                AddCondition(new ComplexCondition<T, TReferenceType>(referenceObjectProvider,
                    (RelationCondition<T, TReferenceType>) _relationConditionTemp, filter));
            }

            internal void AddCondition(ICondition<T> condition)
            {
                _conditionElements.Last().SetCondition(condition);
            }

            public void AddReason(string reason)
            {
                _conditionElements.Last().AddReason(reason);
            }

            internal void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _conditionElements.Add(new ConditionElement<T>(logicalConjunction));
            }

            internal bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
            {
                var filteredObjectsList = filteredObjects.ToList();
                if (filteredObjectsList.IsNullOrEmpty())
                {
                    return _conditionElements.Aggregate(true,
                        (currentResult, conditionElement) => conditionElement.CheckNull(currentResult));
                }

                return filteredObjectsList.All(obj => CheckConditions(obj, architecture));
            }

            private bool CheckConditions(T obj, Architecture architecture)
            {
                return _conditionElements.Aggregate(true,
                    (currentResult, conditionElement) => conditionElement.Check(currentResult, obj, architecture));
            }

            public IEnumerable<EvaluationResult<T>> EvaluateConditions(IEnumerable<T> filteredObjects,
                Architecture architecture, IArchRuleCreator<T> archRuleCreator)
            {
                return filteredObjects.Select(obj => EvaluateConditions(obj, architecture, archRuleCreator));
            }

            private EvaluationResult<T> EvaluateConditions(T obj, Architecture architecture,
                IArchRuleCreator<T> archRuleCreator)
            {
                var passRule = CheckConditions(obj, architecture);
                var description = obj.FullName + " ";
                if (passRule)
                {
                    description += "passed";
                }
                else
                {
                    var first = true;
                    foreach (var conditionElement in _conditionElements.Where(conditionElement =>
                        !conditionElement.Check(obj, architecture)))
                    {
                        if (first)
                        {
                            description += conditionElement.FailDescription;
                            first = false;
                        }
                        else
                        {
                            description += " and " + conditionElement.FailDescription;
                        }
                    }
                }

                return new EvaluationResult<T>(obj, passRule, description, archRuleCreator, architecture);
            }

            public override string ToString()
            {
                return Description;
            }

            private class ConditionElement<T> : IHasFailDescription where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ICondition<T> _condition;
                private string _reason;

                public ConditionElement(LogicalConjunction logicalConjunction)
                {
                    _condition = null;
                    _logicalConjunction = logicalConjunction;
                    _reason = "";
                }

                public string Description =>
                    (_logicalConjunction.Description + " should " + _condition.Description + " " + _reason).Trim();

                public string FailDescription => _condition.FailDescription;

                public void AddReason(string reason)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't add a reason to a ConditionElement before the condition is set.");
                    }

                    if (_reason != "")
                    {
                        throw new InvalidOperationException(
                            "Can't add a reason to a ConditionElement which already has a reason.");
                    }

                    _reason = "because " + reason;
                }

                public void SetCondition(ICondition<T> condition)
                {
                    _condition = condition;
                }

                public bool Check(bool currentResult, T obj, Architecture architecture)
                {
                    return _logicalConjunction.Evaluate(currentResult, Check(obj, architecture));
                }

                public bool Check(T obj, Architecture architecture)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't check a ConditionElement before the condition is set.");
                    }

                    return _condition.Check(obj, architecture);
                }

                public bool CheckNull(bool currentResult)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't check a ConditionElement before the condition is set.");
                    }

                    return _logicalConjunction.Evaluate(currentResult, _condition.CheckNull());
                }

                public override string ToString()
                {
                    return Description;
                }
            }
        }
    }
}