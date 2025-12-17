using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public static class ConditionExtensions
    {
        private sealed class OrderedConditionWrapper<TRuleType> : IOrderedCondition<TRuleType>
            where TRuleType : ICanBeAnalyzed
        {
            private readonly ICondition<TRuleType> _condition;

            public OrderedConditionWrapper(ICondition<TRuleType> condition)
            {
                _condition = condition;
            }

            public string Description => _condition.Description;

            public IEnumerable<ConditionResult> Check(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                var objectCollection = objects as ICollection<TRuleType> ?? objects.ToList();
                var results = Check(architecture, objectCollection, _condition);
                return objectCollection.Select(ruleType => results(ruleType));
            }

            public bool CheckEmpty() => _condition.CheckEmpty();

            private static Func<TRuleType, ConditionResult> Check(
                Architecture architecture,
                ICollection<TRuleType> objects,
                ICondition<TRuleType> condition
            )
            {
                var conditionResults = condition.Check(objects, architecture);
                if (objects.Count > 256)
                {
                    var resultDictionary = conditionResults.ToDictionary(result =>
                        result.AnalyzedObject
                    );
                    return obj => resultDictionary[obj];
                }
                var resultList = conditionResults.ToList();
                return obj => resultList.First(result => result.AnalyzedObject.Equals(obj));
            }
        }

        /// <summary>
        /// Wraps an ICondition as an IOrderedCondition if it is not already one, ensuring that the order of results
        /// corresponds to the order of input objects.
        /// </summary>
        /// <param name="condition">Condition to be wrapped.</param>
        /// <typeparam name="TRuleType">Type of objects the condition applies to.</typeparam>
        /// <returns>An IOrderedCondition that maintains the order of input objects.</returns>
        public static IOrderedCondition<TRuleType> AsOrderedCondition<TRuleType>(
            this ICondition<TRuleType> condition
        )
            where TRuleType : ICanBeAnalyzed
        {
            if (condition is IOrderedCondition<TRuleType> orderedCondition)
            {
                return orderedCondition;
            }
            return new OrderedConditionWrapper<TRuleType>(condition);
        }
    }
}
