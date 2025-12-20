using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ArchitectureCondition<TRuleType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<
            IEnumerable<TRuleType>,
            Architecture,
            IEnumerable<ConditionResult>
        > _condition;

        public ArchitectureCondition(
            Func<IEnumerable<TRuleType>, Architecture, IEnumerable<ConditionResult>> condition,
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

            return obj.GetType() == GetType() && Equals((ArchitectureCondition<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
