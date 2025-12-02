using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public interface IOrderedCondition<in TRuleType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
