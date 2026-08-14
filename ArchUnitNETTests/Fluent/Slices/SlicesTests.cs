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
            // Slice3 depends back on Slice1, so the cycle survives every regrouping of the whole
            // namespace -- including "(**)..", which used to skip the top-level classes and
            // therefore missed the edge that closes the loop.
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
            // Restricting the slices to the "Service" sub-namespaces drops that edge again.
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**).Service..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        [Fact]
        public void MatchingTest()
        {
            Assert.Equal(
                3,
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
                3,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                9,
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                    .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                    .Count()
            );
            Assert.Equal(
                2,
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

        // See: https://github.com/TNG/ArchUnitNET/issues/208 -- only "Slice2.Inner" depends back on
        // "Slice1", so the cycle exists exactly when that sub-namespace is folded into "Slice2".
        [Fact]
        public void SubnamespaceCycleDetectionTest()
        {
            // "(*)" ignores Slice2.Inner altogether, "(**)" gives it a slice of its own; neither
            // closes the loop.
            foreach (var pattern in new[] { "(*)", "(**)" })
            {
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle." + pattern)
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture);
            }

            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle.(*)..")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        [Fact]
        public void Matching_SingleAsterisk_CapturesExactlyOneSegment()
        {
            // A namespace deeper than the pattern does not match at all, so the types below
            // "Slice1.Service" and friends are left out entirely.
            Assert.Equal(new[] { "Slice1", "Slice2", "Slice3" }, Descriptions(Root + "(*)"));
        }

        [Fact]
        public void Matching_SingleAsteriskDotDot_FoldsSubnamespacesIntoParent()
        {
            Assert.Equal(new[] { "Slice1", "Slice2", "Slice3" }, Descriptions(Root + "(*).."));

            // Same names as "(*)", but the trailing ".." pulls the deeper types in as well.
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(*)..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();
            Assert.Equal(5, slices.Single(slice => slice.Description == "Slice3").Types.Count());
        }

        [Fact]
        public void Matching_DoubleAsterisk_CapturesEveryDepth()
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
                Descriptions(Root + "(**)")
            );
        }

        [Fact]
        public void Matching_DoubleAsteriskDotDot_IsRedundant()
        {
            // "(**)" is already greedy, so a trailing ".." has nothing left to skip.
            Assert.Equal(Descriptions(Root + "(**)"), Descriptions(Root + "(**).."));
        }

        [Fact]
        public void Matching_TrailingLiteralAfterCapture_MatchesOnlyThatLiteral()
        {
            Assert.Equal(new[] { "Slice1", "Slice2" }, Descriptions(Root + "(**).Service.."));
        }

        [Fact]
        public void Matching_Alternation_MatchesEitherAlternative()
        {
            Assert.Equal(
                new[] { "Slice1", "Slice2", "Slice3.Group1", "Slice3.Group2" },
                Descriptions(Root + "(**).[Service|Inner]")
            );
        }

        [Fact]
        public void Matching_LeadingDotDot_SkipsWholeLeadingSegments()
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

        // ".." is meant to skip whole namespace segments, but the regex it expands to is entirely
        // optional and carries its own dots, so it can also match empty in the middle of a
        // segment. Only the first capture group names the slice so far, which is why the names
        // below are truncated. Both warts are pinned here on purpose.

        [Fact]
        public void DotDot_BetweenCaptureGroups_MaySplitWithinASegment()
        {
            Assert.Equal(
                new[]
                {
                    "Alpha", // "Alpha.Service" -- as intended
                    "AlphaServic", // "AlphaService" split after "AlphaServic"
                    "Outer", // "Outer.Inner" and "Outer.Mid.Inner" -- as intended
                    "Singl", // "Single" split after "Singl"
                },
                Descriptions(DotDot + "(*)..(*)")
            );
        }

        [Fact]
        public void DotDot_BeforeLiteral_MaySplitWithinASegment()
        {
            Assert.Equal(new[] { "Alpha" }, Descriptions(DotDot + "(*)..Service"));
        }

        [Fact]
        public void DotDot_BeforeLiteral_MergesSegmentAndNonSegmentMatches()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(DotDot + "(*)..Service")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            // "Alpha.Service" is two segments and "AlphaService" is one, yet both end up in the
            // same slice because ".." matched empty inside the latter.
            var alpha = Assert.Single(slices);
            Assert.Equal("Alpha", alpha.Description);
            Assert.Equal(
                new[]
                {
                    "SlicesTestAssembly.DotDotSemantics.Alpha.Service.AlphaServiceSegmentClass",
                    "SlicesTestAssembly.DotDotSemantics.AlphaService.AlphaServiceClass",
                },
                alpha
                    .Types.Select(type => type.FullName)
                    .OrderBy(name => name, StringComparer.Ordinal)
            );
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
