using System.Linq;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNETTests.Fluent.Extensions.StaticTestArchitectures;

namespace ArchUnitNETTests.Core
{
    public class ArchLoaderTests
    {
        private static readonly Assembly ArchUnitNETAssembly =
            new Assembly("ArchUnitNET, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null",
                "ArchUnitNET, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null", false);

        private static readonly Assembly ArchUnitNETTestsAssembly =
            new Assembly("ArchUnitNETTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "ArchUnitNETTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false);

        private static readonly Assembly ArchUnitTestAssemblyAssembly =
            new Assembly("TestAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "TestAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false);

        private static readonly Assembly XunitExtensionAssembly =
            new Assembly("XunitExtension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "XunitExtension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false);

        [Fact]
        public void LoadAssembliesCorrectly()
        {
            Assert.Equal(4, FullArchUnitNETArchitecture.Assemblies.Count());
            Assert.Contains(ArchUnitNETAssembly, FullArchUnitNETArchitecture.Assemblies);
            Assert.Contains(ArchUnitNETTestsAssembly, FullArchUnitNETArchitecture.Assemblies);
            Assert.Contains(ArchUnitTestAssemblyAssembly, FullArchUnitNETArchitecture.Assemblies);
            Assert.Contains(XunitExtensionAssembly, FullArchUnitNETArchitecture.Assemblies);

            Assert.Single(ArchUnitNETTestArchitecture.Assemblies);
            Assert.Contains(ArchUnitNETTestsAssembly, ArchUnitNETTestArchitecture.Assemblies);

            Assert.Single(ArchUnitNETTestAssemblyArchitecture.Assemblies);
            Assert.Contains(ArchUnitTestAssemblyAssembly, ArchUnitNETTestAssemblyArchitecture.Assemblies);
        }
    }
}