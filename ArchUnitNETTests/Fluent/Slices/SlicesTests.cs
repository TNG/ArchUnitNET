using System.Linq;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.xUnit;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void DirectCycleDetectionTest()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.DirectCircle.(*)")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.DirectCircle.(*)..")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.DirectCircle.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        // See: https://github.com/TNG/ArchUnitNET/issues/208
        [Fact]
        public void SubnamespaceCycleDetectionTest()
        {
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(*)")
                .Should()
                .BeFreeOfCycles()
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle.(*)..")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(**)")
                .Should()
                .BeFreeOfCycles()
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
        }

        [Fact]
        public void MatchingTest()
        {
            // (*) matches only exact depth - should match only Slice1, Slice2, Slice3
            // but NOT Slice1.Service, Slice2.Service, Slice3.Group1, Slice3.Group2
            var slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(
                3,
                slices.Count
            );
            Assert.Equal(
                ["Slice1", "Slice2", "Slice3"],
                slices.Select(slice => slice.Description).OrderBy(desc => desc)
            );

            // (**) matches all depths - should match all 7 slices
            slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(
                9,
                slices.Count
            );
            Assert.Equal(
                [
                    "Slice1",
                    "Slice1.Service",
                    "Slice2",
                    "Slice2.Service",
                    "Slice3",
                    "Slice3.Group1",
                    "Slice3.Group1.Inner",
                    "Slice3.Group2",
                    "Slice3.Group2.Inner"
                ],
                slices.Select(slice => slice.Description).OrderBy(desc => desc)
            );

            // (*).. captures first segment and groups subnamespaces into parent slice
            // Should create 3 slices (Slice1, Slice2, Slice3) with subnamespace types included
            slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(3, slices.Count);
            Assert.Equal(
                ["Slice1", "Slice2", "Slice3"],
                slices.Select(slice => slice.Description).OrderBy(desc => desc)
            );

            // (**).Service.. captures all slices that have a Service subnamespace
            slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**).Service..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(2, slices.Count);
            Assert.Equal(
                ["Slice1", "Slice2"],
                slices.Select(slice => slice.Description).OrderBy(desc => desc)
            );

            // (**).[Service|Group1|Group2].. captures all slices that have Service, Group1 or Group2 subnamespaces
            slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**).[Service|Group1|Group2]..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(3, slices.Count);
            Assert.Equal(
                ["Slice1", "Slice2", "Slice3"],
                slices.Select(slice => slice.Description).OrderBy(desc => desc)
            );

            // (**).[Service|Inner] captures all slices that have Service or Inner subnamespaces
            slices = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**).[Service|Inner]")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(4, slices.Count);
            Assert.Equal(
                ["Slice1", "Slice2", "Slice3.Group1", "Slice3.Group2"],
                slices.Select(slice => slice.Description).OrderBy(desc => desc).ToList()
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
