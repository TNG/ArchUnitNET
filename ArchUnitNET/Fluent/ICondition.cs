using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICondition<in TRuleType> : IHasFailDescription where TRuleType : ICanBeAnalyzed
    {
        bool Check(TRuleType obj, Architecture architecture);
        bool CheckEmpty();
    }
}