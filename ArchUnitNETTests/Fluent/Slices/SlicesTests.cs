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
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)..")
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
                .AssertAnyViolations(helper);
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

        // None of these patterns folds Slice2.Inner into Slice2, so Slice1 -> Slice2 and
        // Slice2.Inner -> Slice1 stay dependencies between three distinct slices and there is
        // genuinely no cycle to find.
        [Fact]
        public void SubnamespaceCycleDetectionTest()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            foreach (var pattern in new[] { "(*)", "(*)..", "(**)" })
            {
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle." + pattern)
                    .Should()
                    .BeFreeOfCycles()
                    .AssertNoViolations(helper);
            }
        }

        // See: https://github.com/TNG/ArchUnitNET/issues/208 -- "(**).." is the one pattern that
        // folds Slice2.Inner into Slice2, which should surface the cycle
        // Slice1 -> Slice2 -> (Slice2.Inner) -> Slice1. It currently does not: the fold drops the
        // types sitting directly in the captured namespace (see
        // Matching_DoubleAsteriskDotDot_DropsTypesDirectlyInCapturedNamespace), so Slice1Class
        // never makes it into a slice and the cycle has no starting edge. Pinned as-is; this
        // assertion is expected to flip once the matcher is reimplemented.
        [Fact]
        public void SubnamespaceCycleDetection_FoldedIntoParent_MissesCycle()
        {
            var helper = new SlicesAssemblyTestHelper().WithoutSnapshot();
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(**)..")
                .Should()
                .BeFreeOfCycles()
                .AssertNoViolations(helper);
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

        /// <summary>
        /// The descriptions above only say which slices exist; this pins what ends up inside
        /// them, which is where "(**).." is at its most surprising. Folding keeps the types
        /// from the sub-namespaces but drops the ones sitting directly in the captured
        /// namespace, so Slice1Class/Slice2Class/Slice3Class are in no slice at all. That is
        /// what makes the NotDependOnEachOther snapshots report Slice2 and Slice3 as
        /// depending on nothing, and what hides the cycle in
        /// <see cref="SubnamespaceCycleDetection_FoldedIntoParent_MissesCycle" />.
        /// </summary>
        [Fact]
        public void Matching_DoubleAsteriskDotDot_DropsTypesDirectlyInCapturedNamespace()
        {
            var types = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)..")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToDictionary(
                    slice => slice.Description,
                    slice =>
                        slice
                            .Types.Select(type => type.FullName)
                            .OrderBy(name => name, StringComparer.Ordinal)
                            .ToArray()
                );

            Assert.Equal(new[] { Root + "Slice1.Service.Service1Class" }, types["Slice1"]);
            Assert.Equal(new[] { Root + "Slice2.Service.Service2Class" }, types["Slice2"]);
            Assert.Equal(
                new[]
                {
                    Root + "Slice3.Group1.Group1Class",
                    Root + "Slice3.Group1.Inner.Inner1Class",
                    Root + "Slice3.Group2.Group2Class",
                    Root + "Slice3.Group2.Inner.Inner2Class",
                },
                types["Slice3"]
            );
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

        // The fixture's namespace contains "DuplicatePrefix.Sub" twice
        // (SlicesTestAssembly.DuplicatePrefix.Sub.DuplicatePrefix.Sub), so a leading ".." pattern
        // for that prefix has two candidate starting positions to strip from. This pins that
        // AssignFunc's IndexOf (leftmost match) picks the first occurrence, leaving the second
        // "DuplicatePrefix.Sub" -- and the one leading dot "..DuplicatePrefix.Sub." keeps after
        // stripping only one of its two literal dots -- in the description. With an absolute
        // (non-"..") prefix, the prefix can only ever be found at index 0, so that variant of this
        // test would pass even if IndexOf were replaced by a hardcoded 0.
        [Fact]
        public void MatchingWithPackages_DuplicatePrefixSegment_UsesFirstOccurrenceAsPrefix()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages("..DuplicatePrefix.Sub.(*)")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            Assert.Single(slices);
            Assert.Equal(".DuplicatePrefix.Sub.DuplicatePrefix.Sub", slices[0].Description);
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
        public void DotDot_BeforeLiteral_GivesAlphaServiceAndAlphaDotServiceDistinctSlices()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(DotDot + "(*)..Service")
                .GetObjects(StaticTestArchitectures.SlicesTestArchitecture)
                .ToList();

            // "AlphaService" is one segment and "Alpha.Service" is two, but that distinction is
            // never actually evaluated: the "..Service" postfix is discarded (see
            // AlphaServiceClass.cs), so both namespaces just get their own unfolded slice, each
            // holding exactly the one type it's made of. This is not evidence the matcher tells
            // the two shapes apart -- it would look identical if it ignored segment boundaries
            // entirely.
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
