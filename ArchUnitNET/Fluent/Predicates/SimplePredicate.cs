using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    public class SimplePredicate<TRuleType> : IPredicate<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, bool> _predicate;

        public SimplePredicate(Func<TRuleType, bool> predicate, string description)
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
            return objects.Where(_predicate);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(SimplePredicate<TRuleType> other)
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

            return obj.GetType() == GetType() && Equals((SimplePredicate<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}
