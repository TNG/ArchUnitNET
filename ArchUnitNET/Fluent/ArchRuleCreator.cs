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

        public ArchRuleCreator(Func<Architecture, IEnumerable<T>> objectsToBeAnalyzed, string description)
        {
            _objectFilterManager = new ObjectFilterManager<T>(objectsToBeAnalyzed);
            _conditionManager = new ConditionManager<T>();
            Description = description;
        }

        public string Description { get; private set; }

        public virtual bool Check(Architecture architecture)
        {
            return CheckConditions(GetFilteredObjects(architecture), architecture);
        }

        public void AddObjectFilter(Func<T, bool> objectFilter, string description)
        {
            _objectFilterManager.AddFilter(objectFilter);
            AddDescription(description);
        }

        public void AddObjectFilterConjunction(LogicalConjunction logicalConjunction)
        {
            _objectFilterManager.SetNextLogicalConjunction(logicalConjunction);
            AddLogicalConjunctionDescription(logicalConjunction);
        }

        public void AddConditionConjunction(LogicalConjunction logicalConjunction)
        {
            _conditionManager.SetNextLogicalConjunction(logicalConjunction);
            AddLogicalConjunctionDescription(logicalConjunction);
            AddShouldDescription();
        }

        public void AddSimpleCondition(Func<T, bool> simpleCondition, string description)
        {
            _conditionManager.AddSimpleCondition(simpleCondition);
            AddDescription(description);
        }

        public void BeginComplexCondition<TReference>(Func<T, TReference, bool> relationCondition, string description)
            where TReference : ICanBeAnalyzed
        {
            _conditionManager.BeginComplexCondition(relationCondition);
            AddDescription(description);
        }

        public void ContinueComplexCondition<TReference>(
            Func<Architecture, IEnumerable<TReference>> referenceObjectProvider, Func<TReference, bool> condition,
            string description)
            where TReference : ICanBeAnalyzed
        {
            _conditionManager.ContinueComplexCondition(referenceObjectProvider, condition);
            AddDescription(description);
        }

        public void AddIsNullOrEmptyCondition(bool valueIfEmpty, string description)
        {
            _conditionManager.AddIsNullOrEmptyCondition(valueIfEmpty);
            AddDescription(description);
        }

        private IEnumerable<T> GetFilteredObjects(Architecture architecture)
        {
            return _objectFilterManager.GetFilteredObjects(architecture);
        }

        private bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
        {
            return _conditionManager.CheckConditions(filteredObjects, architecture);
        }

        public void AddThatDescription()
        {
            AddDescription("that");
        }

        public void AddShouldDescription()
        {
            AddDescription("should");
        }

        private void AddLogicalConjunctionDescription(LogicalConjunction logicalConjunction)
        {
            AddDescription(logicalConjunction.Description);
        }

        private void AddDescription(string description)
        {
            Description += " " + description;
        }

        public override string ToString()
        {
            return Description;
        }

        private class ObjectFilterManager<T> where T : ICanBeAnalyzed
        {
            private readonly List<ObjectFilterElement<T>> _objectFilterElements;
            private readonly ObjectProvider<T> _objectProvider;

            internal ObjectFilterManager(Func<Architecture, IEnumerable<T>> objectProvider)
            {
                _objectProvider = new ObjectProvider<T>(objectProvider);
                _objectFilterElements = new List<ObjectFilterElement<T>>
                {
                    new ObjectFilterElement<T>(ForwardSecondValue, new ObjectFilter<T>(t => true))
                };
            }

            internal IEnumerable<T> GetFilteredObjects(Architecture architecture)
            {
                return _objectProvider.GetObjects(architecture).Where(obj => _objectFilterElements.Aggregate(true,
                    (currentResult, objectFilterElement) => objectFilterElement.CheckFilter(currentResult, obj)));
            }

            internal void AddFilter(Func<T, bool> objectFilter)
            {
                _objectFilterElements.Last().SetFilter(new ObjectFilter<T>(objectFilter));
            }

            internal void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _objectFilterElements.Add(new ObjectFilterElement<T>(logicalConjunction));
            }

            private class ObjectFilter<T> where T : ICanBeAnalyzed
            {
                private readonly Func<T, bool> _filter;

                internal ObjectFilter(Func<T, bool> filter)
                {
                    _filter = filter;
                }

                internal bool CheckFilter(T obj)
                {
                    return _filter(obj);
                }
            }

            private class ObjectFilterElement<T> where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ObjectFilter<T> _objectFilter;

                internal ObjectFilterElement(LogicalConjunction logicalConjunction, ObjectFilter<T> objectFilter = null)
                {
                    _objectFilter = objectFilter;
                    _logicalConjunction = logicalConjunction;
                }

                internal void SetFilter(ObjectFilter<T> condition)
                {
                    _objectFilter = condition;
                }

                internal bool CheckFilter(bool currentResult, T obj)
                {
                    if (_objectFilter == null)
                    {
                        throw new InvalidOperationException(
                            "Can't Evaluate an ObjectFilterElement before the filter is set.");
                    }

                    return _logicalConjunction.Evaluate(currentResult, _objectFilter.CheckFilter(obj));
                }
            }
        }

        private class ConditionManager<T> where T : ICanBeAnalyzed
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

            internal void AddSimpleCondition(Func<T, bool> simpleCondition)
            {
                AddCondition(new SimpleCondition<T>(simpleCondition));
            }

            internal void BeginComplexCondition<TReference>(Func<T, TReference, bool> relationCondition)
                where TReference : ICanBeAnalyzed
            {
                _relationConditionTemp = relationCondition;
            }

            internal void ContinueComplexCondition<TReference>(
                Func<Architecture, IEnumerable<TReference>> referenceObjectProvider, Func<TReference, bool> condition)
                where TReference : ICanBeAnalyzed
            {
                AddCondition(new ComplexCondition<T, TReference>(referenceObjectProvider,
                    (Func<T, TReference, bool>) _relationConditionTemp, condition));
            }

            internal void AddIsNullOrEmptyCondition(bool valueIfEmpty)
            {
                AddCondition(new IsNullCondition<T>(valueIfEmpty));
            }

            private void AddCondition(ICondition<T> condition)
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
                        (currentResult, conditionElement) => conditionElement.EvaluateNull(currentResult));
                }

                return filteredObjects.All(obj => _conditionElements.Aggregate(true,
                    (currentResult, conditionElement) => conditionElement.Evaluate(currentResult, obj, architecture)));
            }

            private interface ICondition<T> where T : ICanBeAnalyzed
            {
                bool Evaluate(T obj, Architecture architecture);
                bool EvaluateNull();
            }

            private class SimpleCondition<T> : ICondition<T> where T : ICanBeAnalyzed
            {
                private readonly Func<T, bool> _condition;

                internal SimpleCondition(Func<T, bool> condition)
                {
                    _condition = condition;
                }

                public bool Evaluate(T obj, Architecture architecture)
                {
                    return Evaluate(obj);
                }

                public bool EvaluateNull()
                {
                    return true;
                }

                internal bool Evaluate(T obj)
                {
                    return _condition(obj);
                }
            }

            private class ComplexCondition<T, TReference> : ICondition<T>
                where T : ICanBeAnalyzed where TReference : ICanBeAnalyzed
            {
                private readonly SimpleCondition<TReference> _condition;
                private readonly ObjectProvider<TReference> _objectProvider;
                private readonly Func<T, TReference, bool> _relationCondition;

                internal ComplexCondition(ObjectProvider<TReference> objectProvider,
                    Func<T, TReference, bool> relationCondition, SimpleCondition<TReference> condition)
                {
                    _objectProvider = objectProvider;
                    _relationCondition = relationCondition;
                    _condition = condition;
                }

                internal ComplexCondition(Func<Architecture, IEnumerable<TReference>> objectProvider,
                    Func<T, TReference, bool> relationCondition, Func<TReference, bool> condition)
                    : this(new ObjectProvider<TReference>(objectProvider),
                        relationCondition, new SimpleCondition<TReference>(condition))
                {
                }

                public bool Evaluate(T obj, Architecture architecture)
                {
                    return _objectProvider.GetObjects(architecture).Where(refObj => _relationCondition(obj, refObj))
                        .All(relatedObj => _condition.Evaluate(relatedObj));
                }

                public bool EvaluateNull()
                {
                    return true;
                }
            }

            private class IsNullCondition<T> : ICondition<T> where T : ICanBeAnalyzed
            {
                private readonly bool _valueIfNull;

                internal IsNullCondition(bool valueIfNull)
                {
                    _valueIfNull = valueIfNull;
                }

                public bool Evaluate(T obj, Architecture architecture)
                {
                    return !_valueIfNull;
                }

                public bool EvaluateNull()
                {
                    return _valueIfNull;
                }
            }

            private class ConditionElement<T> where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ICondition<T> _condition;

                internal ConditionElement(LogicalConjunction logicalConjunction)
                {
                    _condition = null;
                    _logicalConjunction = logicalConjunction;
                }

                internal void SetCondition(ICondition<T> condition)
                {
                    _condition = condition;
                }

                internal bool Evaluate(bool currentResult, T obj, Architecture architecture)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't Evaluate a ConditionElement before the condition is set.");
                    }

                    return _logicalConjunction.Evaluate(currentResult, _condition.Evaluate(obj, architecture));
                }

                internal bool EvaluateNull(bool currentResult)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't Evaluate a ConditionElement before the condition is set.");
                    }

                    return _logicalConjunction.Evaluate(currentResult, _condition.EvaluateNull());
                }
            }
        }

        private class ObjectProvider<T> where T : ICanBeAnalyzed
        {
            private readonly Func<Architecture, IEnumerable<T>> _objects;

            internal ObjectProvider(Func<Architecture, IEnumerable<T>> objects)
            {
                _objects = objects;
            }

            internal IEnumerable<T> GetObjects(Architecture architecture)
            {
                return _objects(architecture);
            }
        }
    }
}