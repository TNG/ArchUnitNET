using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICondition<in TRuleType> : IHasFailDescription where TRuleType : ICanBeAnalyzed
    {
        ConditionResult Check(TRuleType obj, Architecture architecture);
        bool CheckEmpty();
    }
}