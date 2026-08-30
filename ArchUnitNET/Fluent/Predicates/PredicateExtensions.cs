using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    public static class PredicateExtensions
    {
        private sealed class PredicateWithDescription<TRuleType> : IPredicate<TRuleType>
            where TRuleType : ICanBeAnalyzed
        {
            private readonly IPredicate<TRuleType> _predicate;

            public PredicateWithDescription(IPredicate<TRuleType> predicate, string description)
            {
                _predicate = predicate;
                Description = description;
            }

            public string Description { get; }

            public IEnumerable<TRuleType> GetMatchingObjects(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                return _predicate.GetMatchingObjects(objects, architecture);
            }

            private bool Equals(PredicateWithDescription<TRuleType> other)
            {
                return Equals(_predicate, other._predicate) && Description == other.Description;
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
                    && Equals((PredicateWithDescription<TRuleType>)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = _predicate != null ? _predicate.GetHashCode() : 0;
                    hashCode =
                        (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static IPredicate<TRuleType> As<TRuleType>(
            this IPredicate<TRuleType> predicate,
            string description
        )
            where TRuleType : ICanBeAnalyzed
        {
            return new PredicateWithDescription<TRuleType>(predicate, description);
        }

        private sealed class PredicateWithReason<TRuleType> : IPredicate<TRuleType>
            where TRuleType : ICanBeAnalyzed
        {
            private readonly IPredicate<TRuleType> _predicate;
            private readonly string _reason;

            public PredicateWithReason(IPredicate<TRuleType> predicate, string reason)
            {
                _predicate = predicate;
                _reason = reason;
            }

            public string Description => $"{_predicate.Description} because {_reason}";

            public IEnumerable<TRuleType> GetMatchingObjects(
                IEnumerable<TRuleType> objects,
                Architecture architecture
            )
            {
                return _predicate.GetMatchingObjects(objects, architecture);
            }

            private bool Equals(PredicateWithReason<TRuleType> other)
            {
                return Equals(_predicate, other._predicate) && _reason == other._reason;
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

                return obj.GetType() == GetType() && Equals((PredicateWithReason<TRuleType>)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = _predicate != null ? _predicate.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ (_reason != null ? _reason.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static IPredicate<TRuleType> Because<TRuleType>(
            this IPredicate<TRuleType> predicate,
            string reason
        )
            where TRuleType : ICanBeAnalyzed
        {
            return new PredicateWithReason<TRuleType>(predicate, reason);
        }
    }
}
