using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ArchitectureObjectFilter<TRuleType> : IObjectFilter<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, Architecture, bool> _filter;

        public ArchitectureObjectFilter(Func<TRuleType, Architecture, bool> filter, string description)
        {
            _filter = filter;
            Description = description;
        }

        public string Description { get; }

        public bool CheckFilter(TRuleType obj, Architecture architecture)
        {
            return _filter(obj, architecture);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}