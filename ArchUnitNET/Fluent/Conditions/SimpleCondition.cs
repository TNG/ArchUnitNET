using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class SimpleCondition<TRuleType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, ConditionResult> _condition;

        public SimpleCondition(
            Func<TRuleType, bool> condition,
            string description,
            string failDescription
        )
        {
            _condition = obj => new ConditionResult(obj, condition(obj), failDescription);
            Description = description;
        }

        public SimpleCondition(Func<TRuleType, ConditionResult> condition, string description)
        {
            _condition = condition;
            Description = description;
        }

        public SimpleCondition(
            Func<TRuleType, bool> condition,
            Func<TRuleType, string> dynamicFailDescription,
            string description
        )
        {
            _condition = obj => new ConditionResult(
                obj,
                condition(obj),
                dynamicFailDescription(obj)
            );
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<ConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            return objects.Select(obj => _condition(obj));
        }

        public bool CheckEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(SimpleCondition<TRuleType> other)
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

            return obj.GetType() == GetType() && Equals((SimpleCondition<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
