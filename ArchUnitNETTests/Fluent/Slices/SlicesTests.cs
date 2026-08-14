using System;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.AssemblyTestHelper;
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

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";
        private const string DotDot = "SlicesTestAssembly.DotDotSemantics.";

        private static string[] Descriptions(string pattern)
        {
            return SliceRuleDefinition
                .Slices()
                .Matching(pattern)
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .Select(slice => slice.Description)
                .OrderBy(description => description, StringComparer.Ordinal)
                .ToArray();
        }

        [Fact]
        public void DirectCycleDetectionTest()
        {
            foreach (var pattern in new[] { "(*)", "(*)..", "(**)" })
            {
                Assert.Throws<FailedArchRuleException>(() =>
                    SliceRuleDefinition
                        .Slices()
                        .Matching("SlicesTestAssembly.DirectCircle." + pattern)
                        .Should()
                        .BeFreeOfCycles()
                        .Check(StaticTestArchitectures.SlicesTestArchitecture)
                );
            }
        }

        // See: https://github.com/TNG/ArchUnitNET/issues/208 -- folding a sub-namespace into its
        // parent slice should surface the cycle Slice1 -> Slice2 -> (Slice2.Inner) -> Slice1.
        [Fact]
        public void SubnamespaceCycleDetectionTest()
        {
            foreach (var pattern in new[] { "(*)", "(*)..", "(**)" })
            {
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle." + pattern)
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture);
            }
        }

        [Fact]
        public void Matching_SingleAsterisk_CapturesEveryDepth()
        {
            Assert.Equal(
                new[]
                {
                    "Slice1",
                    "Slice1.Service",
                    "Slice2",
                    "Slice2.Service",
                    "Slice3",
                    "Slice3.Group1",
                    "Slice3.Group1.Inner",
                    "Slice3.Group2",
                    "Slice3.Group2.Inner",
                },
                Descriptions(Root + "(*)")
            );
        }

        [Fact]
        public void Matching_SingleAsteriskDotDot_CapturesEveryDepth()
        {
            Assert.Equal(Descriptions(Root + "(*)"), Descriptions(Root + "(*).."));
        }

        [Fact]
        public void Matching_DoubleAsteriskDotDot_FoldsSubnamespacesIntoParent()
        {
            Assert.Equal(new[] { "Slice1", "Slice2", "Slice3" }, Descriptions(Root + "(**).."));
        }

        [Fact]
        public void Matching_TrailingLiteralAfterCapture_MatchesNothing()
        {
            Assert.Empty(Descriptions(Root + "(**).Service.."));
        }

        [Fact]
        public void Matching_Alternation_MatchesNothing()
        {
            Assert.Empty(Descriptions(Root + "(**).[Service|Inner]"));
        }

        [Fact]
        public void Matching_LeadingDotDot_CapturesEveryDepth()
        {
            Assert.Equal(Descriptions(Root + "(*)"), Descriptions("..MultipleSubnamespaces.(*)"));
        }

        [Fact]
        public void MatchingWithPackages_DuplicatePrefixSegment_UsesCorrectPrefix()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("SlicesTestAssembly.DuplicatePrefix.Sub.(*)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            Assert.Single(slices);
            Assert.Equal("SlicesTestAssembly.DuplicatePrefix.Sub.Sub", slices[0].Description);
        }

        // --- ".." semantics -------------------------------------------------------------

        [Fact]
        public void DotDot_BetweenCaptureGroups_CapturesEveryDepth()
        {
            Assert.Equal(
                new[]
                {
                    "Alpha.Service",
                    "AlphaService",
                    "Outer.Inner",
                    "Outer.Mid.Inner",
                    "Single",
                },
                Descriptions(DotDot + "(*)..(*)")
            );
        }

        [Fact]
        public void DotDot_BeforeLiteral_CapturesEveryDepth()
        {
            Assert.Equal(
                new[]
                {
                    "Alpha.Service",
                    "AlphaService",
                    "Outer.Inner",
                    "Outer.Mid.Inner",
                    "Single",
                },
                Descriptions(DotDot + "(*)..Service")
            );
        }

        [Fact]
        public void DotDot_BeforeLiteral_KeepsSegmentAndNonSegmentMatchesApart()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(DotDot + "(*)..Service")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            // "AlphaService" is one segment and "Alpha.Service" is two; each currently lands in a
            // slice of its own holding exactly one type.
            Assert.Single(slices.Single(slice => slice.Description == "AlphaService").Types);
            Assert.Single(slices.Single(slice => slice.Description == "Alpha.Service").Types);
        }

        // --- failure messages -----------------------------------------------------------

        [Fact]
        public Task BeFreeOfCycles_WithCycle_ReturnsDescriptiveCycleMessage()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.DirectCircle.(*)")
                .Should()
                .BeFreeOfCycles();
            rule.AssertOnlyViolations(helper);
            return helper.AssertSnapshotMatches();
        }

        [Fact]
        public Task BeFreeOfCycles_WhenNoCycles_ReportsAllSlicesFreeOfCycles()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.DotDotSemantics.(*)")
                .Should()
                .BeFreeOfCycles();
            rule.AssertNoViolations(helper);
            return helper.AssertSnapshotMatches();
        }

        [Fact]
        public Task NotDependOnEachOther_ReturnsDescriptiveDependencyMessage()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)..")
                .Should()
                .NotDependOnEachOther();
            // Mixed on purpose: the snapshot then shows both the passing and the failing wording.
            rule.AssertAnyViolations(helper);
            return helper.AssertSnapshotMatches();
        }
    }
}
