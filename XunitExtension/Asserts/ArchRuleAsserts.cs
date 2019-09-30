using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit.Sdk;


// ReSharper disable once CheckNamespace
namespace Xunit
{
    partial class Assert
    {
        public static void ArchRule(Architecture architecture, IArchRule archRule)
        {
            if (!architecture.ViolatesRule(archRule))
            {
                throw new FailedArchRuleException(architecture, archRule);
            }
        }
    }
}