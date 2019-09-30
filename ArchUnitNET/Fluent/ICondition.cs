using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICondition<TRuleType> : IHasFailDescription where TRuleType : ICanBeAnalyzed
    {
        bool Check(TRuleType obj, Architecture architecture);
        bool CheckEmpty();
    }
}