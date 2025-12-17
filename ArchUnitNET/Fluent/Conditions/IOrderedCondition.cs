using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    /// <summary>
    /// An IOrderedCondition is a Condition that has the semantic requirement that the order of the results
    /// corresponds to the order of the input objects.
    /// </summary>
    /// <typeparam name="TRuleType">Type of objects the condition can be checked against.</typeparam>
    public interface IOrderedCondition<in TRuleType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
