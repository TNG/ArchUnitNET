using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit.Sdk;


// ReSharper disable once CheckNamespace
namespace Xunit
{
    partial class Assert
    {
        /// <summary>
        ///     Verifies that the architecture meets the criteria of the archrule.
        /// </summary>
        /// <param name="architecture">The architecture to be tested</param>
        /// <param name="archRule">The rule to test the architecture with</param>
        /// <exception cref="FailedArchRuleException">Thrown if the rule is violated</exception>
        public static void ArchRule(Architecture architecture, IArchRule archRule)
        {
            if (!architecture.FulfilsRule(archRule))
            {
                throw new FailedArchRuleException(architecture, archRule);
            }
        }
    }
}