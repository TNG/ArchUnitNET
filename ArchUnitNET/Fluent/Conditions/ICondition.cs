using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    /// <summary>
    /// A condition that can be checked against objects of type TRuleType.
    /// </summary>
    /// <typeparam name="TRuleType">Type of objects the condition can be checked against.</typeparam>
    public interface ICondition<in TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        /// <summary>
        /// Checks the condition against the provided objects within the given architecture.
        /// </summary>
        /// <param name="objects">Objects to check the condition against.</param>
        /// <param name="architecture">The architecture context for the check.</param>
        /// <returns>A collection of ConditionResults indicating the outcome for each object.</returns>
        IEnumerable<ConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        );

        bool CheckEmpty();
    }
}
