using System.Linq;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class NamespaceTests
    {
        [Fact]
        public void NoNamespacesWithEmptyNameTest()
        {
            Assert.True(
                StaticTestArchitectures.ArchUnitNETTestArchitecture.Namespaces.All(ns =>
                    ns.FullName != ""
                )
            );
            //this currently fails with StaticTestArchitectures.FullArchUnitNETArchitectureWithDependencies because of some weird Interop, Guard, Consts,... types
        }

        [Fact]
        public void NoEmptyNamespacesTest()
        {
            Assert.True(
                StaticTestArchitectures.FullArchUnitNETArchitectureWithDependencies.Namespaces.All(
                    ns => ns.Types.Any()
                )
            );
        }
    }
}
