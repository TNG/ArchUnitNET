using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
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

        [Fact]
        public void NamespacesAreEqualByName()
        {
            var ns = new Namespace("A.B", new List<IType>());
            var sameName = new Namespace("A.B", new List<IType>());
            var otherName = new Namespace("A.C", new List<IType>());

            Assert.Equal(ns, sameName);
            Assert.Equal(ns.GetHashCode(), sameName.GetHashCode());
            Assert.NotEqual(ns, otherName);
            Assert.Equal("A.B", ns.Name);
            Assert.Equal("A.B", ns.FullName);
            Assert.Equal("A.B", ns.Description);
            Assert.Equal("A.B", ns.ToString());
        }
    }
}
