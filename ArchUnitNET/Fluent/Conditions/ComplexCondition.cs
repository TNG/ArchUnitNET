using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ComplexCondition<TRuleType, TRelatedType> : IOrderedCondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRelatedType : ICanBeAnalyzed
    {
        private readonly IPredicate<TRelatedType> _predicate;
        private readonly IObjectProvider<TRelatedType> _relatedTypes;
        private readonly RelationCondition<TRuleType, TRelatedType> _relation;

        public ComplexCondition(
            IObjectProvider<TRelatedType> relatedTypes,
            RelationCondition<TRuleType, TRelatedType> relation,
            IPredicate<TRelatedType> predicate
        )
        {
            _relatedTypes = relatedTypes;
            _relation = relation;
            _predicate = predicate;
        }

        public string Description => _relation.Description + " " + _predicate.Description;

        public IEnumerable<ConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            return _relation
                .GetCondition(
                    _predicate.GetMatchingObjects(
                        _relatedTypes.GetObjects(architecture),
                        architecture
                    )
                )
                .Check(objects, architecture);
        }

        public bool CheckEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ComplexCondition<TRuleType, TRelatedType> other)
        {
            return Equals(_predicate, other._predicate) && Equals(_relation, other._relation);
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
                && Equals((ComplexCondition<TRuleType, TRelatedType>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_predicate != null ? _predicate.GetHashCode() : 0) * 397)
                    ^ (_relation != null ? _relation.GetHashCode() : 0);
            }
        }
    }
}
