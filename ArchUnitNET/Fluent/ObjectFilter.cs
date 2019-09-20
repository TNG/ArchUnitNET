using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ObjectFilter<TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, bool> _filter;

        public ObjectFilter(Func<TRuleType, bool> filter, string description)
        {
            _filter = filter;
            Description = description;
        }

        public string Description { get; }

        public bool CheckFilter(TRuleType obj)
        {
            return _filter(obj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}