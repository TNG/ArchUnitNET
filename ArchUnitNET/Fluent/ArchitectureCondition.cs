using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ArchitectureCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, Architecture, bool> _condition;

        public ArchitectureCondition(Func<TRuleType, Architecture, bool> condition, string description,
            string failDescription)
        {
            _condition = condition;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Check(TRuleType obj, Architecture architecture)
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