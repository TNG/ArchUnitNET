using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

// ReSharper disable once CheckNamespace
namespace ArchUnitNET.TUnit
{
    public static class ArchRuleAssert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        public static void CheckRule(Architecture architecture, IArchRule archRule)
        {
            if (!archRule.HasNoViolations(architecture))
            {
                var results = archRule.Evaluate(architecture);
                throw new FailedArchRuleException(architecture, archRule);
            }
        }
    }
}
