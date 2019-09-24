using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IObjectFilter<TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        bool CheckFilter(TRuleType obj, Architecture architecture);
    }
}