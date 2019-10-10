using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICondition<in TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        ConditionResult Check(TRuleType obj, Architecture architecture);
        bool CheckEmpty();
    }
}