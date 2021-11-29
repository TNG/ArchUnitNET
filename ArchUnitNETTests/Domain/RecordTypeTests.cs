using ArchUnitNET.Domain;
using TestAssembly;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class RecordTypeTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        [Fact]
        public void RecordTypeExists()
        {
            Assert.Contains(Architecture.Types, type => type.Name.Equals(nameof(AbstractRecord)));
            Assert.Contains(Architecture.Types, type => type.Name.Equals(nameof(Record1)));
        }
    }
}