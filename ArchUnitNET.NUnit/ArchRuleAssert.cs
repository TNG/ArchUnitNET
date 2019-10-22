using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using NUnit.Framework;

namespace ArchUnitNET.NUnit
{
    public static class ArchRuleAssert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        public static void FulfilsRule(Architecture architecture, IArchRule archRule)
        {
            if (!architecture.FulfilsRule(archRule))
            {
                Assert.Fail(architecture.EvaluateRule(archRule).ToErrorMessage());
            }
        }
    }
}