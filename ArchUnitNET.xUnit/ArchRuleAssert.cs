using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

// ReSharper disable once CheckNamespace
namespace ArchUnitNET.xUnit
{
    public static class ArchRuleAssert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        /// <exception cref="FailedArchRuleException">Thrown if the rule is violated</exception>
        public static void CheckRule(Architecture architecture, IArchRule archRule)
        {
            if (!archRule.HasNoViolations(architecture))
            {
                throw new FailedArchRuleException(architecture, archRule);
            }
        }
    }
}
