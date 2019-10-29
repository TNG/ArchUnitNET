using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class EnumerableCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<IEnumerable<TRuleType>, IEnumerable<ConditionResult>> _condition;

        public EnumerableCondition(Func<IEnumerable<TRuleType>, IEnumerable<ConditionResult>> condition,
            string description)
        {
            _condition = condition;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<ConditionResult> Check(IEnumerable<TRuleType> objects, Architecture architecture)
        {
            return _condition(objects);
        }

        public bool CheckEmpty()
        {
            return true;
        }
    }
}