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
            FailDescription = failDescription;
        }

        public SimpleCondition(Func<TRuleType, ConditionResult> condition, string description, string failDescription)
        {
            _condition = condition;
            Description = description;
            FailDescription = failDescription;
        }

        public SimpleCondition(Func<TRuleType, bool> condition, Func<TRuleType, string> dynamicFailDescription,
            string description, string failDescription)
        {
            _condition = obj => new ConditionResult(condition(obj), dynamicFailDescription(obj));
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

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