using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class CombinedConditionResult : IConditionResult
    {
        private readonly IConditionResult _firstConditionResult;
        private readonly List<ConditionElementResult> _conditionElements;

        public CombinedConditionResult(
            IConditionResult left,
            LogicalConjunction logicalConjunction,
            IConditionResult right
        )
        {
            _firstConditionResult = left;
            _conditionElements = new List<ConditionElementResult>
            {
                new ConditionElementResult(right, logicalConjunction),
            };
        }

        public CombinedConditionResult Add(
            LogicalConjunction logicalConjunction,
            IConditionResult conditionResult
        )
        {
            _conditionElements.Add(new ConditionElementResult(conditionResult, logicalConjunction));
            return this;
        }

        public ICanBeAnalyzed AnalyzedObject =>
            _conditionElements[0].ConditionResult.AnalyzedObject;

        public bool Pass
        {
            get
            {
                return _conditionElements.Aggregate(
                    _firstConditionResult.Pass,
                    (currentResult, conditionElement) =>
                        conditionElement.LogicalConjunction.Evaluate(
                            currentResult,
                            conditionElement.ConditionResult.Pass
                        )
                );
            }
        }

        public string Description
        {
            get
            {
                if (Pass)
                {
                    return $"{AnalyzedObject.FullName} passed";
                }
                var conditionResults = new List<IConditionResult>(_conditionElements.Count + 1)
                {
                    _firstConditionResult,
                };
                conditionResults.AddRange(
                    _conditionElements.Select(condElement => condElement.ConditionResult)
                );
                return string.Join(
                    " and ",
                    conditionResults
                        .Where(condResult => !condResult.Pass)
                        .Select(condResult => condResult.Description)
                        .Distinct()
                );
            }
        }

        private class ConditionElementResult
        {
            public readonly IConditionResult ConditionResult;
            public readonly LogicalConjunction LogicalConjunction;

            public ConditionElementResult(
                IConditionResult conditionResult,
                LogicalConjunction logicalConjunction
            )
            {
                ConditionResult = conditionResult;
                LogicalConjunction = logicalConjunction;
            }
        }
    }
}
