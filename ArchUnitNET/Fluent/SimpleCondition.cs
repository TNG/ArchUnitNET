using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class SimpleCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, ConditionResult> _condition;

        public SimpleCondition(Func<TRuleType, bool> condition, string description, string failDescription)
        {
            _condition = obj => new ConditionResult(condition(obj), failDescription);
            Description = description;
        }

        public SimpleCondition(Func<TRuleType, ConditionResult> condition, string description)
        {
            _condition = condition;
            Description = description;
        }

        public SimpleCondition(Func<TRuleType, bool> condition, Func<TRuleType, string> dynamicFailDescription,
            string description)
        {
            _condition = obj => new ConditionResult(condition(obj), dynamicFailDescription(obj));
            Description = description;
        }

        public string Description { get; }

        public ConditionResult Check(TRuleType obj, Architecture architecture)
        {
            return _condition(obj);
        }

        public bool CheckEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}