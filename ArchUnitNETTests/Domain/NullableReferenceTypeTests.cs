using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using TestAssembly;
using Xunit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Domain
{
    public class NullableReferenceTypeTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        [Fact]
        public void ClassUsingNullableReferenceTypeShouldDependOnlyOnItself()
        {
            var classUsingNullableReferenceType = Types().That().Are(typeof(ClassUsingNullableReferenceType));

            var classDependsOnlyOnItself = classUsingNullableReferenceType.Should().OnlyDependOn(classUsingNullableReferenceType);

            classDependsOnlyOnItself.Check(Architecture);
        }
    }
}
