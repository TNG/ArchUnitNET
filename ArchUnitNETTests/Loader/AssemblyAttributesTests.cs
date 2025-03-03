using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using TestAssembly;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class AssemblyAttributesTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;
        private readonly Attribute _testAttribute;
        private const string TestParameter = "TestParameter";

        public AssemblyAttributesTests()
        {
            _testAttribute = Architecture.GetAttributeOfType(typeof(AssemblyTestAttribute));
        }

        [Fact]
        public void CollectAssemblyAttributesTest()
        {
            Assert.Single(Architecture.Assemblies);
            var assembly = Architecture.Assemblies.First();
            Assert.Contains(_testAttribute, assembly.Attributes);
        }

        [Fact]
        public void CollectAssemblyAttributeParametersTest()
        {
            Assert.Single(Architecture.Assemblies);
            var assembly = Architecture.Assemblies.First();
            Assert.Contains(
                assembly.AttributeInstances,
                instance =>
                    Equals(instance.Type, _testAttribute)
                    && instance.AttributeArguments.Any(arg =>
                        arg.Value is string val && val == TestParameter
                    )
            );
        }
    }
}
