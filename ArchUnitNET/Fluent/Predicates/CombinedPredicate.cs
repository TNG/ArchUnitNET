using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    internal sealed class CombinedPredicate<TRuleType> : IPredicate<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly IPredicate<TRuleType> _leftPredicate;
        private readonly LogicalConjunction _logicalConjunction;
        private readonly IPredicate<TRuleType> _rightPredicate;

        public CombinedPredicate(
            IPredicate<TRuleType> leftPredicate,
            LogicalConjunction logicalConjunction,
            IPredicate<TRuleType> rightPredicate
        )
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
            _rightPredicate = rightPredicate;
        }

        public string Description =>
            $"{_leftPredicate.Description} {_logicalConjunction.Description} {_rightPredicate.Description}";

        public IEnumerable<TRuleType> GetMatchingObjects(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            if (!(objects is ICollection<TRuleType> objectCollection))
            {
                objectCollection = objects.ToList();
            }
            return _logicalConjunction.Evaluate(
                _leftPredicate.GetMatchingObjects(objectCollection, architecture),
                _rightPredicate.GetMatchingObjects(objectCollection, architecture)
            );
        }

        private bool Equals(CombinedPredicate<TRuleType> other)
        {
            return Equals(_leftPredicate, other._leftPredicate)
                && Equals(_logicalConjunction, other._logicalConjunction)
                && Equals(_rightPredicate, other._rightPredicate);
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

            return obj.GetType() == GetType() && Equals((CombinedPredicate<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _leftPredicate != null ? _leftPredicate.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397)
                    ^ (_logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0);
                hashCode =
                    (hashCode * 397)
                    ^ (_rightPredicate != null ? _rightPredicate.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
