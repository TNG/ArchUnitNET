using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    public static class ArchRuleExtensions
    {
        public static void Check(this IArchRule archRule, Architecture architecture)
        {
            Assert.ArchRule(architecture, archRule);
        }
    }
}