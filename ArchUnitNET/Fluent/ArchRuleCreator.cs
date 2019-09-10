using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

#pragma warning disable 693

namespace ArchUnitNET.Fluent
{
    public class ArchRuleCreator<T> where T : ICanBeAnalyzed
    {
        private readonly ConditionManager<T> _conditionManager;
        private readonly ObjectFilterManager<T> _objectFilterManager;
        private bool _isInConditionPhase;

        public ArchRuleCreator(Func<Architecture, IEnumerable<T>> objectsToBeAnalyzed)
        {
            _objectFilterManager = new ObjectFilterManager<T>(objectsToBeAnalyzed);
            _conditionManager = new ConditionManager<T>();
            _isInConditionPhase = false;
        }

        public void SetConditionPhase()
        {
            _isInConditionPhase = true;
        }

        public void AddConditionConjunction(Func<bool, bool, bool> logicalConjunction)
        {
            if (_isInConditionPhase)
            {
                _conditionManager.SetNextLogicalConjunction(logicalConjunction);
            }
            else
            {
                _objectFilterManager.SetNextLogicalConjunction(logicalConjunction);
            }
        }

        public void AddSimpleCondition(Func<T, bool> condition)
        {
            AddSimpleCondition(new SimpleCondition<T>(condition));
        }

        private void AddSimpleCondition(SimpleCondition<T> simpleCondition)
        {
            if (_isInConditionPhase)
            {
                _conditionManager.AddSimpleCondition(simpleCondition);
            }
            else
            {
                _objectFilterManager.AddFilter(simpleCondition.Evaluate);
            }
        }

        public void AddComplexCondition<TReference>(Func<Architecture, IEnumerable<TReference>> referenceObjectProvider,
            Func<T, TReference, bool> relationCondition, Func<TReference, bool> condition)
            where TReference : ICanBeAnalyzed
        {
            AddComplexCondition(
                new ComplexCondition<T, TReference>(referenceObjectProvider, relationCondition, condition));
        }

        private void AddComplexCondition<TReference>(ComplexCondition<T, TReference> complexCondition)
            where TReference : ICanBeAnalyzed
        {
            if (_isInConditionPhase)
            {
                _conditionManager.AddComplexCondition(complexCondition);
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't add complex condition to ArchRule before Should() was used.");
            }
        }

        private IEnumerable<T> GetFilteredObjects(Architecture architecture)
        {
            return _objectFilterManager.GetFilteredObjects(architecture);
        }

        private bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
        {
            return _conditionManager.CheckConditions(filteredObjects, architecture);
        }

        public bool CheckRule(Architecture architecture)
        {
            return CheckConditions(GetFilteredObjects(architecture), architecture);
        }

        private class ObjectFilterManager<T> where T : ICanBeAnalyzed
        {
            private readonly ObjectFilter<T> _objectFilter;
            private Func<bool, bool, bool> _nextLogicalConjunction;

            internal ObjectFilterManager(ObjectProvider<T> objectProvider)
            {
                _objectFilter = new ObjectFilter<T>(objectProvider, T => true);
                _nextLogicalConjunction = (b1, b2) => b2;
            }

            internal ObjectFilterManager(Func<Architecture, IEnumerable<T>> objectProvider)
                : this(new ObjectProvider<T>(objectProvider))
            {
            }

            internal IEnumerable<T> GetFilteredObjects(Architecture architecture)
            {
                return _objectFilter.GetFilteredObjects(architecture);
            }

            internal void AddFilter(Func<T, bool> objectFilter)
            {
                _objectFilter.AddFilter(_nextLogicalConjunction, objectFilter);
            }

            internal void SetNextLogicalConjunction(Func<bool, bool, bool> logicalConjunction)
            {
                _nextLogicalConjunction = logicalConjunction;
            }

            private class ObjectFilter<T> where T : ICanBeAnalyzed
            {
                private readonly ObjectProvider<T> _objectProvider;
                private Func<T, bool> _filter;

                internal ObjectFilter(ObjectProvider<T> objectProvider, Func<T, bool> filter)
                {
                    _objectProvider = objectProvider;
                    _filter = filter;
                }

                internal ObjectFilter(Func<Architecture, IEnumerable<T>> objectProvider, Func<T, bool> filter)
                    : this(new ObjectProvider<T>(objectProvider), filter)
                {
                }

                internal IEnumerable<T> GetFilteredObjects(Architecture architecture)
                {
                    return _objectProvider.GetObjects(architecture).Where(obj => _filter(obj));
                }

                internal void AddFilter(Func<bool, bool, bool> logicalConjunction, Func<T, bool> newFilter)
                {
                    var oldFilter = _filter;
                    _filter = obj => logicalConjunction(oldFilter(obj), newFilter(obj));
                }
            }
        }

        private class ConditionManager<T> where T : ICanBeAnalyzed
        {
            private Func<T, Architecture, bool> _currentCondition;
            private Func<bool, bool, bool> _nextLogicalConjunction;

            internal ConditionManager()
            {
                _currentCondition = (T, a) => true;
                _nextLogicalConjunction = (b1, b2) => b2;
            }

            internal bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
            {
                return filteredObjects.All(obj => _currentCondition(obj, architecture));
            }

            internal void AddSimpleCondition(SimpleCondition<T> simpleCondition)
            {
                var oldCondition = _currentCondition;
                _currentCondition = (T, a) => _nextLogicalConjunction(oldCondition(T, a), simpleCondition.Evaluate(T));
            }

            internal void AddComplexCondition<TReference>(ComplexCondition<T, TReference> complexCondition)
                where TReference : ICanBeAnalyzed
            {
                var oldCondition = _currentCondition;
                _currentCondition = (T, a) =>
                    _nextLogicalConjunction(oldCondition(T, a), complexCondition.Evaluate(T, a));
            }

            public void SetNextLogicalConjunction(Func<bool, bool, bool> logicalConjunction)
            {
                _nextLogicalConjunction = logicalConjunction;
            }
        }


        private class SimpleCondition<T> where T : ICanBeAnalyzed
        {
            private readonly Func<T, bool> _condition;

            internal SimpleCondition(Func<T, bool> condition)
            {
                _condition = condition;
            }

            internal bool Evaluate(T obj)
            {
                return _condition(obj);
            }
        }

        private class ComplexCondition<T, TReference> where T : ICanBeAnalyzed where TReference : ICanBeAnalyzed
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

            internal bool Evaluate(T obj, Architecture architecture)
            {
                return _objectProvider.GetObjects(architecture).Where(refObj => _relationCondition(obj, refObj))
                    .All(relatedObj => _condition.Evaluate(relatedObj));
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