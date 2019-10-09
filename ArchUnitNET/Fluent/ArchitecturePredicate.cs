using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ArchitecturePredicate<TRuleType> : IPredicate<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, Architecture, bool> _predicate;

        public ArchitecturePredicate(Func<TRuleType, Architecture, bool> predicate, string description)
        {
            _predicate = predicate;
            Description = description;
        }

        public string Description { get; }

        public bool CheckPredicate(TRuleType obj, Architecture architecture)
        {
            return _predicate(obj, architecture);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}