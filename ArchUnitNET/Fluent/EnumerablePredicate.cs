using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class EnumerablePredicate<TRuleType> : IPredicate<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<IEnumerable<TRuleType>, IEnumerable<TRuleType>> _predicate;

        public EnumerablePredicate(Func<IEnumerable<TRuleType>, IEnumerable<TRuleType>> predicate,
            string description)
        {
            _predicate = predicate;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<TRuleType> GetMatchingObjects(IEnumerable<TRuleType> objects, Architecture architecture)
        {
            return _predicate(objects);
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(EnumerablePredicate<TRuleType> other)
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

            return obj.GetType() == GetType() && Equals((EnumerablePredicate<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}