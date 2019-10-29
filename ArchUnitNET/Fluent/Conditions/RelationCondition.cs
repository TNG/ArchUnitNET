using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Conditions.EnumerableOperator;

namespace ArchUnitNET.Fluent.Conditions
{
    public class RelationCondition<TRuleType, TRelatedType> : IHasDescription
        where TRuleType : ICanBeAnalyzed where TRelatedType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, IEnumerable<TRelatedType>> _relation;
        private readonly EnumerableOperator _shouldBeTrueFor;

        public RelationCondition(Func<TRuleType, IEnumerable<TRelatedType>> relation,
            EnumerableOperator shouldBeTrueFor,
            string description,
            string failDescription)
        {
            _relation = relation;
            _shouldBeTrueFor = shouldBeTrueFor;
            Description = description;
            FailDescription = failDescription;
        }

        public string FailDescription { get; }

        public string Description { get; }

        public bool
            CheckRelation(TRuleType obj, IPredicate<TRelatedType> predicate, Architecture architecture)
        {
            switch (_shouldBeTrueFor)
            {
                case All:
                    return predicate.GetMatchingObjects(_relation(obj).Distinct(), architecture).Count() ==
                           _relation(obj).Distinct().Count();
                case Any:
                    return predicate.GetMatchingObjects(_relation(obj), architecture).Any();
                case None:
                    return !predicate.GetMatchingObjects(_relation(obj), architecture).Any();
                default:
                    throw new IndexOutOfRangeException("The ShouldBeTrueFor Operator does not have a valid value.");
            }
        }

        private bool Equals(RelationCondition<TRuleType, TRelatedType> other)
        {
            return _shouldBeTrueFor == other._shouldBeTrueFor &&
                   FailDescription == other.FailDescription &&
                   Description == other.Description;
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

            return obj.GetType() == GetType() && Equals((RelationCondition<TRuleType, TRelatedType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) _shouldBeTrueFor;
                hashCode = (hashCode * 397) ^ (FailDescription != null ? FailDescription.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public enum EnumerableOperator
    {
        All,
        Any,
        None
    }
}