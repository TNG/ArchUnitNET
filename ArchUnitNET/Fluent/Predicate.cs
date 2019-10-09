using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class Predicate<TRuleType> : IPredicate<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, bool> _predicate;

        public Predicate(Func<TRuleType, bool> predicate, string description)
        {
            _predicate = predicate;
            Description = description;
        }

        public string Description { get; }

        public bool CheckPredicate(TRuleType obj, Architecture architecture)
        {
            return _predicate(obj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}