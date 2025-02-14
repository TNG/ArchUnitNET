using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class RelationCondition<TRuleType, TRelatedType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
        where TRelatedType : ICanBeAnalyzed
    {
        private readonly Func<IEnumerable<TRelatedType>, ICondition<TRuleType>> _relation;

        public RelationCondition(
            Func<IEnumerable<TRelatedType>, ICondition<TRuleType>> relation,
            string description,
            string failDescription
        )
        {
            _relation = relation;
            Description = description;
            FailDescription = failDescription;
        }

        public string FailDescription { get; }

        public string Description { get; }

        public ICondition<TRuleType> GetCondition(IEnumerable<TRelatedType> objectProvider)
        {
            return _relation(objectProvider);
        }

        private bool Equals(RelationCondition<TRuleType, TRelatedType> other)
        {
            return FailDescription == other.FailDescription && Description == other.Description;
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

            return obj.GetType() == GetType()
                && Equals((RelationCondition<TRuleType, TRelatedType>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FailDescription != null ? FailDescription.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
