using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ArchitectureCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, Architecture, ConditionResult> _condition;

        public ArchitectureCondition(Func<TRuleType, Architecture, bool> condition, string description,
            string failDescription)
        {
            _condition = (obj, architecture) => new ConditionResult(condition(obj, architecture), failDescription);
            Description = description;
        }

        public ArchitectureCondition(Func<TRuleType, Architecture, ConditionResult> condition, string description)
        {
            _condition = condition;
            Description = description;
        }

        public ArchitectureCondition(Func<TRuleType, Architecture, bool> condition,
            Func<TRuleType, Architecture, string> dynamicFailDescription, string description)
        {
            _condition = (obj, architecture) =>
                new ConditionResult(condition(obj, architecture), dynamicFailDescription(obj, architecture));
            Description = description;
        }

        public string Description { get; }

        public ConditionResult Check(TRuleType obj, Architecture architecture)
        {
            return _condition(obj, architecture);
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