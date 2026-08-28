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

        // --- Rule evaluation driven by MatchingWithPackages (not just Matching) -------

        [Fact]
        public void BeFreeOfCycles_MatchingWithPackages_DetectsCycle()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages("SlicesTestAssembly.MultipleSubnamespaces.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        // SubnamespaceCircle has real edges (Slice1 -> Slice2, Slice2.Inner -> Slice1) but no
        // cycle, unlike DotDotSemantics whose fixtures have no members and thus no edges at all --
        // an empty graph would pass here even if BeFreeOfCycles always returned "no cycles".
        [Fact]
        public void BeFreeOfCycles_MatchingWithPackages_WhenAcyclic_Passes()
        {
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages("SlicesTestAssembly.SubnamespaceCircle.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
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

        // None of these patterns folds Slice2.Inner into Slice2, so Slice1 -> Slice2 and
        // Slice2.Inner -> Slice1 stay dependencies between three distinct slices and there is
        // genuinely no cycle to find.
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
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("SlicesTestAssembly.SubnamespaceCircle.(**)..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.SlicesTestArchitecture)
            );
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
        ///     The descriptions above only say which slices exist; this pins what ends up inside
        ///     them, which is where "(**).." is at its most surprising. Folding keeps the types
        ///     from the sub-namespaces but drops the ones sitting directly in the captured
        ///     namespace, so Slice1Class/Slice2Class/Slice3Class are in no slice at all. That is
        ///     what makes the NotDependOnEachOther snapshots report Slice2 and Slice3 as
        ///     depending on nothing, and what hides the cycle in
        ///     <see cref="SubnamespaceCycleDetection_FoldedIntoParent_MissesCycle" />.
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
    }
}
