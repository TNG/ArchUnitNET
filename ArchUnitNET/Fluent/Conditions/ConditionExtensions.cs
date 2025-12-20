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

            public IEnumerable<IConditionResult> Check(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                var objectCollection = objects as ICollection<TRuleType> ?? objects.ToList();
                var results = Check(architecture, objectCollection, _condition);
                return objectCollection.Select(ruleType => results(ruleType));
            }

            public bool CheckEmpty() => _condition.CheckEmpty();

            private static Func<TRuleType, IConditionResult> Check(
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

        private class ConditionWithDescription<TRuleType> : IOrderedCondition<TRuleType>
            where TRuleType : ICanBeAnalyzed
        {
            private readonly IOrderedCondition<TRuleType> _condition;

            public ConditionWithDescription(
                IOrderedCondition<TRuleType> condition,
                string description
            )
            {
                _condition = condition;
                Description = description;
            }

            public string Description { get; }

            public IEnumerable<IConditionResult> Check(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                return _condition.Check(objects, architecture);
            }

            public bool CheckEmpty()
            {
                return _condition.CheckEmpty();
            }
        }

        public static IOrderedCondition<TRuleType> As<TRuleType>(
            this IOrderedCondition<TRuleType> condition,
            string description
        )
            where TRuleType : ICanBeAnalyzed
        {
            return new ConditionWithDescription<TRuleType>(condition, description);
        }

        private class ConditionWithReason<TRuleType> : IOrderedCondition<TRuleType>
            where TRuleType : ICanBeAnalyzed
        {
            private readonly IOrderedCondition<TRuleType> _condition;
            private readonly string _reason;

            public ConditionWithReason(IOrderedCondition<TRuleType> condition, string reason)
            {
                _condition = condition;
                _reason = reason;
            }

            public string Description => $"{_condition.Description} because {_reason}";

            public IEnumerable<IConditionResult> Check(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                return _condition.Check(objects, architecture);
            }

            public bool CheckEmpty()
            {
                return _condition.CheckEmpty();
            }
        }

        public static IOrderedCondition<TRuleType> Because<TRuleType>(
            this IOrderedCondition<TRuleType> condition,
            string reason
        )
            where TRuleType : ICanBeAnalyzed
        {
            return new ConditionWithReason<TRuleType>(condition, reason);
        }
    }
}
