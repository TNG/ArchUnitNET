using System.Linq;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.xUnit;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void CycleDetectionTest()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        [Fact]
        public void MatchingTest()
        {
            Assert.Equal(
                9,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                9,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                9,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                3,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                4,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice3.(*)")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.Service.(*)")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Any()
            );
        }

        [Fact]
        public void NotDependOnEachOtherTest()
        {
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice3.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice1.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice1.(*)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .NotDependOnEachOther()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }
    }
}
