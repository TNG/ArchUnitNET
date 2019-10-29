using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Predicates
{
    public interface IPredicate<TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        IEnumerable<TRuleType> GetMatchingObjects(IEnumerable<TRuleType> objects, Architecture architecture);
    }
}