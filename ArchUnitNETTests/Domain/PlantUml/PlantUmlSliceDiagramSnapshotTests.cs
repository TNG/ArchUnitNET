using System;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;
using ArchUnitNET.Fluent.Slices;
using VerifyXunit;
using Xunit;

namespace ArchUnitNETTests.Domain.PlantUml
{
    /// <summary>
    ///     Pins the PlantUML output for every combination of slice pattern and rendering mode.
    ///     These are characterization tests: a snapshot records what the exporter currently
    ///     produces, not necessarily what it ideally should, so a diff here is a prompt to look
    ///     rather than proof of a regression.
    /// </summary>
    public class PlantUmlSliceDiagramSnapshotTests
    {
        private static readonly ArchUnitNET.Domain.Architecture Architecture =
            StaticTestArchitectures.SlicesTestArchitecture;

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        /// <summary>
        ///     GetObjects returns slices in Mono.Cecil's type-table order, which is deterministic
        ///     per build but not contractually stable across Cecil or runtime upgrades. Sorting by
        ///     description keeps the node and dependency order in the snapshots below stable
        ///     regardless of that traversal order.
        /// </summary>
        private static Slice[] SortedSlices(GivenSlices slices)
        {
            return slices
                .GetObjects(Architecture)
                .OrderBy(slice => slice.Description, StringComparer.Ordinal)
                .ToArray();
        }

        private static Task VerifySlices(
            GivenSlices slices,
            GenerationOptions generationOptions = null
        )
        {
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(SortedSlices(slices), generationOptions)
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        [Fact]
        public Task BuildUmlBySlices_SingleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*)"));
        }

        [Fact]
        public Task BuildUmlBySlices_DoubleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
        }

        // The matcher discards everything after the first "(*", so "(*).(*)" and "(*)..(*)" are
        // indistinguishable to it and the next two snapshots are byte-identical -- as are their
        // MatchingWithPackages counterparts below. That identity is the point: it is what a
        // reimplementation giving the second capture group any meaning would have to change.
        // SlicesTests.DotDot_* cover the shapes that would tell the two patterns apart.
        [Fact]
        public Task BuildUmlBySlices_MultipleCaptureGroups()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*).(*)"));
        }

        [Fact]
        public Task BuildUmlBySlices_NonContiguousCaptureGroups()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*)..(*)"));
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_SingleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(*)"));
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_DoubleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(**)"));
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_MultipleCaptureGroups()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(*).(*)")
            );
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_NonContiguousCaptureGroups()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(*)..(*)")
            );
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_LimitDependencies()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(**)"),
                new GenerationOptions { LimitDependencies = true }
            );
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_C4Style()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(**)"),
                new GenerationOptions { C4Style = true }
            );
        }

        [Fact]
        public void BuildUmlBySlices_C4Style_Throws()
        {
            // Slices produced by Matching (as opposed to MatchingWithPackages) have no namespace
            // prefix, and PlantUmlSlice.BuildStringC4Style dereferences it unconditionally.
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            Assert.Throws<NullReferenceException>(() =>
                new PlantUmlFileBuilder()
                    .WithDependenciesFrom(slices, new GenerationOptions { C4Style = true })
                    .AsString()
            );
        }

        [Fact]
        public Task BuildUmlBySlicesFocusOn()
        {
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(**)")
            );
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice1")
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        // --- Cyclic slice dependencies are folded into a single Circle dependency ----

        [Fact]
        public Task BuildUmlBySlices_DirectCircle_RendersCircleDependency()
        {
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().Matching("SlicesTestAssembly.DirectCircle.(*)")
            );
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices).AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_DirectCircle()
        {
            var slices = SortedSlices(
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages("SlicesTestAssembly.DirectCircle.(*)")
            );
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices).AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        // --- LimitDependencies without namespaces takes the OneToOneCompact branch ----

        [Fact]
        public Task BuildUmlBySlices_LimitDependencies_Compact()
        {
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(slices, new GenerationOptions { LimitDependencies = true })
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        // --- IncludeNodesWithoutDependencies = false removes dependency-less slices ----

        [Fact]
        public Task BuildUmlBySlices_ExcludeNodesWithoutDependencies()
        {
            // A single-asterisk pattern would already collapse away the dependency-less slices
            // via RemovePatternInappropriateSlices, so use "(**)" to keep them in the slice list
            // and let IncludeNodesWithoutDependencies do the removal instead.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(
                    slices,
                    new GenerationOptions { IncludeNodesWithoutDependencies = false }
                )
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        // --- A DependencyFilter is honoured when building slice diagrams --------------

        [Fact]
        public Task BuildUmlBySlices_WithDependencyFilter()
        {
            // "(*)" would already collapse Slice3.Group1 away via RemovePatternInappropriateSlices,
            // leaving no dependency for the filter to remove, so use "(**)" instead.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(
                    slices,
                    new GenerationOptions
                    {
                        DependencyFilter = dep => !dep.Origin.FullName.Contains("Slice3.Group1"),
                    }
                )
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }

        // --- WithDependenciesFromFocusOn argument validation ---------------------------

        [Fact]
        public void BuildUmlBySlicesFocusOn_EmptyPackage_Throws()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(**)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, "")
            );
            Assert.Equal("Package can't be empty", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_DotOnlyPackage_Throws()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(**)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, ".")
            );
            Assert.Equal("Package can't contain a single dot only", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_UnknownPackage_Throws()
        {
            // A single-asterisk pattern gives every slice a non-null CountOfAsteriskInPattern,
            // which is required to reach RemovePatternInappropriateSlices' early return for a
            // focus string that no slice contains, on the way to this exception.
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(*)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, "NotAPackage")
            );
            Assert.Contains("is not contained in this slice", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_TrailingDotPackage_IsTrimmed()
        {
            var slicesWithTrailingDot = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(**)")
                .GetObjects(Architecture);
            var umlWithTrailingDot = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slicesWithTrailingDot, Root + "Slice1.")
                .AsString();

            var slicesWithoutTrailingDot = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(**)")
                .GetObjects(Architecture);
            var umlWithoutTrailingDot = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slicesWithoutTrailingDot, Root + "Slice1")
                .AsString();

            Assert.Equal(umlWithoutTrailingDot, umlWithTrailingDot);
        }

        // --- RemovePatternInappropriateSlices' per-slice "continue" for FocusOn ------

        [Fact]
        public Task BuildUmlBySlicesFocusOn_SingleAsteriskPattern_SkipsSlicesWithoutFocusString()
        {
            // Focusing on the single-segment slice "Slice3" means slices whose Description
            // doesn't contain it (e.g. "Slice1") skip the pattern-appropriateness check via
            // RemovePatternInappropriateSlices' per-slice "continue", while "Slice3" itself is
            // shallow enough to survive that check and keep the package reachable.
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(*)")
            );
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3")
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }
    }
}
