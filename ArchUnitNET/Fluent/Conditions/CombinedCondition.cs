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
                return leftResults.Concat(rightResults);
            }
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
    }
}
