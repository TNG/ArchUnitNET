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
    public class ArchRuleCreator<T> : IArchRuleCreator where T : ICanBeAnalyzed
    {
        private readonly ConditionManager<T> _conditionManager;
        private readonly ObjectFilterManager<T> _objectFilterManager;

        public ArchRuleCreator(ObjectProvider<T> objectProvider)
        {
            _objectFilterManager = new ObjectFilterManager<T>(objectProvider);
            _conditionManager = new ConditionManager<T>();
        }

        public virtual string Description => _objectFilterManager.Description + " " + _conditionManager.Description;

        public virtual bool Check(Architecture architecture)
        {
            return CheckConditions(GetFilteredObjects(architecture), architecture);
        }

        public virtual IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return EvaluateConditions(GetFilteredObjects(architecture), architecture);
        }

        public void AddObjectFilter(ObjectFilter<T> objectFilter)
        {
            _objectFilterManager.AddFilter(objectFilter);
        }

        public void AddObjectFilter(Func<T, bool> objectFilter, string description)
        {
            AddObjectFilter(new ObjectFilter<T>(objectFilter, description));
        }

        public void AddObjectFilterConjunction(LogicalConjunction logicalConjunction)
        {
            _objectFilterManager.SetNextLogicalConjunction(logicalConjunction);
        }

        public void AddConditionConjunction(LogicalConjunction logicalConjunction)
        {
            _conditionManager.SetNextLogicalConjunction(logicalConjunction);
        }

        public void AddSimpleCondition(SimpleCondition<T> simpleCondition)
        {
            _conditionManager.AddCondition(simpleCondition);
        }

        public void AddSimpleCondition(Func<T, bool> simpleCondition, string description, string failDescription)
        {
            AddSimpleCondition(new SimpleCondition<T>(simpleCondition, description, failDescription));
        }

        public void BeginComplexCondition<TReference>(RelationCondition<T, TReference> relationCondition)
            where TReference : ICanBeAnalyzed
        {
            _conditionManager.BeginComplexCondition(relationCondition);
        }

        public void BeginComplexCondition<TReference>(Func<T, TReference, bool> relationCondition, string description,
            string failDescription)
            where TReference : ICanBeAnalyzed
        {
            BeginComplexCondition(
                new RelationCondition<T, TReference>(relationCondition, description, failDescription));
        }

        public void ContinueComplexCondition<TReference>(ObjectProvider<TReference> referenceObjectProvider,
            ObjectFilter<TReference> objectFilter)
            where TReference : ICanBeAnalyzed
        {
            _conditionManager.ContinueComplexCondition(referenceObjectProvider, objectFilter);
        }

        public void ContinueComplexCondition<TReference>(ObjectProvider<TReference> referenceObjectProvider,
            Func<TReference, bool> filter, string description)
            where TReference : ICanBeAnalyzed
        {
            ContinueComplexCondition(referenceObjectProvider, new ObjectFilter<TReference>(filter, description));
        }

        public void AddIsNullOrEmptyCondition(bool valueIfEmpty, string description, string failDescription)
        {
            _conditionManager.AddCondition(new IsNullCondition<T>(valueIfEmpty, description, failDescription));
        }

        private IEnumerable<T> GetFilteredObjects(Architecture architecture)
        {
            return _objectFilterManager.GetFilteredObjects(architecture);
        }

        private bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
        {
            return _conditionManager.CheckConditions(filteredObjects, architecture);
        }

        private IEnumerable<EvaluationResult> EvaluateConditions(IEnumerable<T> filteredObjects,
            Architecture architecture)
        {
            return _conditionManager.EvaluateConditions(filteredObjects, architecture,
                _objectFilterManager.Description + " " + _conditionManager.Description);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ArchRuleCreator<T> other)
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

            return obj.GetType() == GetType() && Equals((ArchRuleCreator<T>) obj);
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
                    new ObjectFilterElement<T>(ForwardSecondValue, new ObjectFilter<T>(t => true, ""))
                };
            }

            public string Description => _objectFilterElements.Count < 2
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

                public ObjectFilterElement(LogicalConjunction logicalConjunction, ObjectFilter<T> objectFilter = null)
                {
                    _objectFilter = objectFilter;
                    _logicalConjunction = logicalConjunction;
                }

                public string Description => _objectFilter == null
                    ? _logicalConjunction.Description
                    : (_logicalConjunction.Description + " " + _objectFilter.Description).Trim();

                public void SetFilter(ObjectFilter<T> condition)
                {
                    _objectFilter = condition;
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

            internal void BeginComplexCondition<TReference>(RelationCondition<T, TReference> relationCondition)
                where TReference : ICanBeAnalyzed
            {
                _relationConditionTemp = relationCondition;
            }

            internal void ContinueComplexCondition<TReference>(ObjectProvider<TReference> referenceObjectProvider,
                ObjectFilter<TReference> filter)
                where TReference : ICanBeAnalyzed
            {
                AddCondition(new ComplexCondition<T, TReference>(referenceObjectProvider,
                    (RelationCondition<T, TReference>) _relationConditionTemp, filter));
            }

            internal void AddCondition(ICondition<T> condition)
            {
                _conditionElements.Last().SetCondition(condition);
            }

            internal void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _conditionElements.Add(new ConditionElement<T>(logicalConjunction));
            }

            internal bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
            {
                if (filteredObjects.IsNullOrEmpty())
                {
                    return _conditionElements.Aggregate(true,
                        (currentResult, conditionElement) => conditionElement.CheckNull(currentResult));
                }

                return filteredObjects.All(obj => CheckConditions(obj, architecture));
            }

            private bool CheckConditions(T obj, Architecture architecture)
            {
                return _conditionElements.Aggregate(true,
                    (currentResult, conditionElement) => conditionElement.Check(currentResult, obj, architecture));
            }

            public IEnumerable<EvaluationResult> EvaluateConditions(IEnumerable<T> filteredObjects,
                Architecture architecture, string archRuleDescription)
            {
                return filteredObjects.Select(obj => EvaluateConditions(obj, architecture, archRuleDescription))
                    .ToList();
            }

            private EvaluationResult EvaluateConditions(T obj, Architecture architecture, string archRuleDescription)
            {
                var passRule = CheckConditions(obj, architecture);
                var description = obj.FullName;
                if (passRule)
                {
                    description += " passed";
                }
                else
                {
                    var first = true;
                    foreach (var conditionElement in _conditionElements.Where(conditionElement =>
                        !conditionElement.Check(obj, architecture)))
                    {
                        if (first)
                        {
                            description += " " + conditionElement.ShortFailDescription;
                            first = false;
                        }
                        else
                        {
                            description += " " + conditionElement.FailDescription;
                        }
                    }
                }

                return new EvaluationResult(obj, passRule, description, archRuleDescription);
            }

            public override string ToString()
            {
                return Description;
            }

            private class ConditionElement<T> : IHasFailDescription where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ICondition<T> _condition;

                public ConditionElement(LogicalConjunction logicalConjunction)
                {
                    _condition = null;
                    _logicalConjunction = logicalConjunction;
                }

                public string ShortFailDescription => _condition.FailDescription;

                public string Description =>
                    (_logicalConjunction.Description + " should " + _condition.Description).Trim();

                public string FailDescription =>
                    (_logicalConjunction.Description + " " + _condition.FailDescription).Trim();

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