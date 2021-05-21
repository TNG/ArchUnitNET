using System.Linq;
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
            Assert.True(Architecture.Types.Where(type => type.Name.Equals(nameof(AbstractRecord))).Any());
            Assert.True(Architecture.Types.Where(type => type.Name.Equals(nameof(Record1))).Any());
        }
    }
}