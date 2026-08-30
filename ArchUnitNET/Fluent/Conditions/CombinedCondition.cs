using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ArchUnitNET.Fluent.Conditions
{
    internal sealed class CombinedCondition<TRuleType> : IOrderedCondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly IOrderedCondition<TRuleType> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IOrderedCondition<TRuleType> _rightCondition;

        public CombinedCondition(
            IOrderedCondition<TRuleType> leftCondition,
            LogicalConjunction logicalConjunction,
            IOrderedCondition<TRuleType> rightCondition
        )
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
            _rightCondition = rightCondition;
        }

        public string Description =>
            $"{_leftCondition.Description} {_logicalConjunction.Description} should {_rightCondition.Description}";

        public IEnumerable<IConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            if (!(objects is ICollection<TRuleType> objectCollection))
            {
                objectCollection = objects.ToList();
            }
            var leftResults = _leftCondition.Check(objectCollection, architecture);
            var rightResults = _rightCondition.Check(objectCollection, architecture);
            if (objectCollection.Count == 0)
            {
                var leftResultsList = leftResults.ToList();
                var rightResultsList = rightResults.ToList();
                if (leftResultsList.Count == 0)
                {
                    return rightResultsList;
                }
                if (rightResultsList.Count == 0)
                {
                    return leftResultsList;
                }
                return Combine(leftResultsList, rightResultsList);
            }
            return Combine(leftResults, rightResults);
        }

        private IEnumerable<IConditionResult> Combine(
            IEnumerable<IConditionResult> leftResults,
            IEnumerable<IConditionResult> rightResults
        )
        {
            return leftResults.Zip(
                rightResults,
                (leftResult, rightResult) =>
                {
                    if (leftResult is CombinedConditionResult combinedLeftResult)
                    {
                        return combinedLeftResult.Add(_logicalConjunction, rightResult);
                    }
                    return new CombinedConditionResult(
                        leftResult,
                        _logicalConjunction,
                        rightResult
                    );
                }
            );
        }

        public bool CheckEmpty()
        {
            return _logicalConjunction.Evaluate(
                _leftCondition.CheckEmpty(),
                _rightCondition.CheckEmpty()
            );
        }

        private bool Equals(CombinedCondition<TRuleType> other)
        {
            return Equals(_leftCondition, other._leftCondition)
                && Equals(_logicalConjunction, other._logicalConjunction)
                && Equals(_rightCondition, other._rightCondition);
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

            return obj.GetType() == GetType() && Equals((CombinedCondition<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _leftCondition != null ? _leftCondition.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397)
                    ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                hashCode =
                    (hashCode * 397)
                    ^ (_rightCondition != null ? _rightCondition.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
