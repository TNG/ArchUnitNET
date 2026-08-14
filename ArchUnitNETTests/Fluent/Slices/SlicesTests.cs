using System;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void CycleDetectionTest()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                .Should()
                .BeFreeOfCycles()
                .AssertOnlyViolations(helper);
            // Slice3 depends back on Slice1, so the cycle survives every regrouping of the whole
            // namespace -- including "(**)..", which used to skip the top-level classes and
            // therefore missed the edge that closes the loop.
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                .Should()
                .BeFreeOfCycles()
                .AssertOnlyViolations(helper);
            // Restricting the slices to the "Service" sub-namespaces drops that edge again.
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**).Service..")
                .Should()
                .BeFreeOfCycles()
                .AssertNoViolations(helper);
        }

        // --- Rule evaluation driven by MatchingWithPackages (not just Matching) -------

        [Fact]
        public void BeFreeOfCycles_MatchingWithPackages_DetectsCycle()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                .Should()
                .BeFreeOfCycles()
                .AssertOnlyViolations(helper);
        }

        // SubnamespaceCircle has real edges (Slice1 -> Slice2, Slice2.Inner -> Slice1) but no
        // cycle, unlike DotDotSemantics whose fixtures have no members and thus no edges at all --
        // an empty graph would pass here even if BeFreeOfCycles always returned "no cycles".
        [Fact]
        public void BeFreeOfCycles_MatchingWithPackages_WhenAcyclic_Passes()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("SlicesTestAssembly.SubnamespaceCircle.(**)")
                .Should()
                .BeFreeOfCycles()
                .AssertNoViolations(helper);
        }

        [Fact]
        public Task NotDependOnEachOther_MatchingWithPackages_ReportsViolations()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                .Should()
                .NotDependOnEachOther();
            rule.AssertAnyViolations(helper);
            return helper.AssertSnapshotMatches();
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
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice3.(*)")
                .Should()
                .NotDependOnEachOther()
                .AssertNoViolations(helper);
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.Slice1.(*)")
                .Should()
                .NotDependOnEachOther()
                .AssertNoViolations(helper);
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                .Should()
                .NotDependOnEachOther()
                .AssertAnyViolations(helper);
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
                .Should()
                .NotDependOnEachOther()
                .AssertOnlyViolations(helper);
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
                .Should()
                .NotDependOnEachOther()
                .AssertAnyViolations(helper);
        }

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";
        private const string DotDot = "SlicesTestAssembly.DotDotSemantics.";

        [Fact]
        public void DirectCycleDetectionTest()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            foreach (var pattern in new[] { "(*)", "(*)..", "(**)" })
            {
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.DirectCircle." + pattern)
                    .Should()
                    .BeFreeOfCycles()
                    .AssertOnlyViolations(helper);
            }
        }

        // Neither of these patterns folds Slice2.Inner into Slice2 -- "(*)" ignores it altogether
        // and "(**)" gives it a slice of its own -- so Slice1 -> Slice2 and Slice2.Inner -> Slice1
        // stay dependencies between distinct slices and there is genuinely no cycle to find.
        [Fact]
        public void SubnamespaceCycleDetectionTest()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            foreach (var pattern in new[] { "(*)", "(**)" })
            {
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle." + pattern)
                    .Should()
                    .BeFreeOfCycles()
                    .AssertNoViolations(helper);
            }
        }

        // See: https://github.com/TNG/ArchUnitNET/issues/208 -- "(*).." is the one pattern that
        // folds Slice2.Inner into Slice2, which surfaces the cycle
        // Slice1 -> Slice2 -> (Slice2.Inner) -> Slice1.
        [Fact]
        public void SubnamespaceCycleDetection_FoldedIntoParent_DetectsCycle()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(*)..")
                .Should()
                .BeFreeOfCycles()
                .AssertOnlyViolations(helper);
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

        // As in ArchUnit, "(A|B)" is a capture group like "(*)": the alternative that matched is
        // part of the slice's identity and its name, whereas "[A|B]" only selects.
        [Fact]
        public void Matching_ParenthesisedAlternation_IsACaptureGroup()
        {
            Assert.Equal(new[] { "Slice1", "Slice2" }, Descriptions(Root + "(Slice1|Slice2).."));
            Assert.Equal(
                new[] { "Slice1.Service", "Slice2.Service" },
                Descriptions(Root + "(Slice1|Slice2).(*)")
            );
            Assert.Equal(
                new[] { "Slice1.Service", "Slice2.Service", "Slice3.Group1" },
                Descriptions(Root + "(*).(Service|Group1)")
            );
        }

        [Fact]
        public void MatchingWithPackages_ParenthesisedAlternationAsFirstGroup_KeepsThePrefix()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(Slice1|Slice2)..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .Select(slice => (slice.Description, slice.NameSpace))
                .OrderBy(slice => slice.Description, StringComparer.Ordinal)
                .ToArray();

            Assert.Equal(new[] { (Root + "Slice1", Root), (Root + "Slice2", Root) }, slices);
        }

        [Fact]
        public void Matching_LeadingDotDot_SkipsWholeLeadingSegments()
        {
            Assert.Equal(Descriptions(Root + "(*)"), Descriptions("..MultipleSubnamespaces.(*)"));
        }

        // The fixture's namespace contains "DuplicatePrefix.Sub" twice
        // (SlicesTestAssembly.DuplicatePrefix.Sub.DuplicatePrefix.Sub), so a leading ".." pattern
        // for that prefix has two candidate positions to match at. The ".." is greedy, but
        // skipping as far as the second occurrence leaves nothing for the capture group, so the
        // match backtracks to the first: the captured slice is the *second* "DuplicatePrefix.Sub".
        [Fact]
        public void DuplicatePrefixSegment_CapturesAfterTheFirstOccurrenceOfThePrefix()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching("..DuplicatePrefix.Sub.(**)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            Assert.Single(slices);
            Assert.Equal("DuplicatePrefix.Sub", slices[0].Description);
        }

        // MatchingWithPackages keeps everything in front of the capture group, so the same match
        // names the slice after the whole namespace.
        [Fact]
        public void MatchingWithPackages_DuplicatePrefixSegment_KeepsBothOccurrences()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("..DuplicatePrefix.Sub.(**)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            Assert.Single(slices);
            Assert.Equal(
                "SlicesTestAssembly.DuplicatePrefix.Sub.DuplicatePrefix.Sub",
                slices[0].Description
            );
        }

        // --- ".." semantics -------------------------------------------------------------

        [Fact]
        public void DotDot_BetweenCaptureGroups_SkipsWholeSegments()
        {
            // "Single" has nothing for the second group to capture and "AlphaService" is a single
            // segment, so neither matches; "Outer.Mid.Inner" has its middle segment skipped.
            Assert.Equal(
                new[] { "Alpha.Service", "Outer.Inner" },
                Descriptions(DotDot + "(*)..(*)")
            );
        }

        [Fact]
        public void DotDot_BeforeLiteral_MatchesOnlyWholeSegments()
        {
            Assert.Equal(new[] { "Alpha" }, Descriptions(DotDot + "(*)..Service"));
        }

        [Fact]
        public void DotDot_BeforeLiteral_DoesNotSplitWithinASegment()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(DotDot + "(*)..Service")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            // Only "Alpha.Service" matches. "AlphaService" is one segment and must not be split
            // into "Alpha" + "Service".
            var alpha = Assert.Single(slices);
            Assert.Equal("Alpha", alpha.Description);
            Assert.Equal(
                "SlicesTestAssembly.DotDotSemantics.Alpha.Service.AlphaServiceSegmentClass",
                Assert.Single(alpha.Types).FullName
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

        // MultipleSubnamespaces is the only fixture whose failing results are asserted purely via
        // HasNoViolations, which short-circuits on the first violation and therefore never forces
        // the rest of the lazily evaluated sequence. These two pin the complete result set.
        // DirectCircle needs no equivalent: its three patterns collapse to the same cycle, whose
        // wording BeFreeOfCycles_WithCycle_ReturnsDescriptiveCycleMessage already pins.
        [Fact]
        public Task BeFreeOfCycles_MultipleSubnamespaces_ReportsEveryCycle()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .Should()
                .BeFreeOfCycles();
            rule.AssertOnlyViolations(helper);
            return helper.AssertSnapshotMatches();
        }

        [Fact]
        public Task NotDependOnEachOther_MultipleSubnamespaces_ReportsEveryDependency()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .Should()
                .NotDependOnEachOther();
            rule.AssertAnyViolations(helper);
            return helper.AssertSnapshotMatches();
        }

        // SubnamespaceCircle has real edges (Slice1 -> Slice2, Slice2.Inner -> Slice1) but no
        // cycle, unlike DotDotSemantics whose fixtures have no members and thus no edges at all --
        // an empty graph would pass here even if BeFreeOfCycles always returned "no cycles".
        [Fact]
        public Task BeFreeOfCycles_WhenNoCycles_ReportsAllSlicesFreeOfCycles()
        {
            var helper = new SlicesAssemblyTestHelper();
            var rule = SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(**)")
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
    }
}
