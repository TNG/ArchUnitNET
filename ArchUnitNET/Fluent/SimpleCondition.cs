using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class SimpleCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, bool> _condition;

        public SimpleCondition(Func<TRuleType, bool> condition, string description, string failDescription)
        {
            _condition = condition;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Check(TRuleType obj, Architecture architecture)
        {
            return Evaluate(obj);
        }

        public bool CheckNull()
        {
            return true;
        }

        public bool Evaluate(TRuleType obj)
        {
            return _condition(obj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}