using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ArchitectureCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<IEnumerable<TRuleType>, Architecture, IEnumerable<ConditionResult>> _condition;

        public ArchitectureCondition(Func<TRuleType, Architecture, bool> condition, string description,
            string failDescription)
        {
            _condition = (ruleTypes, architecture) => ruleTypes.Select(type =>
                new ConditionResult(type, condition(type, architecture), failDescription));
            Description = description;
        }

        public ArchitectureCondition(Func<TRuleType, Architecture, ConditionResult> condition, string description)
        {
            _condition = (ruleTypes, architecture) => ruleTypes.Select(type => condition(type, architecture));
            Description = description;
        }

        public ArchitectureCondition(Func<IEnumerable<TRuleType>, Architecture, IEnumerable<ConditionResult>> condition, string description)
        {
            _condition = condition;
            Description = description;
        }

        public ArchitectureCondition(Func<TRuleType, Architecture, bool> condition,
            Func<TRuleType, Architecture, string> dynamicFailDescription, string description)
        {
            _condition = (ruleTypes, architecture) => ruleTypes.Select(type =>
                new ConditionResult(type, condition(type, architecture), dynamicFailDescription(type, architecture)));
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<ConditionResult> Check(IEnumerable<TRuleType> objects, Architecture architecture)
        {
            return _condition(objects, architecture);
        }

        public bool CheckEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ArchitectureCondition<TRuleType> other)
        {
            return Description == other.Description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((ArchitectureCondition<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}