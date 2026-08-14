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
    /// Pins the PlantUML output for every combination of slice pattern and rendering mode.
    /// These are characterization tests: a snapshot records what the exporter currently
    /// produces, not necessarily what it ideally should, so a diff here is a prompt to look
    /// rather than proof of a regression.
    /// </summary>
    public class PlantUmlSliceDiagramTests
    {
        private static readonly ArchUnitNET.Domain.Architecture Architecture =
            StaticTestArchitectures.SlicesTestArchitecture;

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        // --- Each pattern shape groups MultipleSubnamespaces differently ----------------
        //
        // SlicesTests pins which slices each pattern produces; these snapshots pin how the
        // exporter draws them.

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
        public Task BuildUmlBySlices_C4Style()
        {
            // Slices produced by Matching (as opposed to MatchingWithPackages) have no namespace
            // prefix, so this exercises PlantUmlSlice.BuildStringC4Style's no-namespace branch.
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(Root + "(**)"),
                new GenerationOptions { C4Style = true }
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
            return VerifyUml(uml);
        }

        [Fact]
        public Task BuildUmlBySlicesFocusOn_Matching_ColorsFocusedSlices()
        {
            // Slices produced by Matching have no namespace, so the focused ones take
            // PlantUmlSlice.BuildString's namespace-less branch with a colour set -- the only
            // path on which the exporter renders "[Slice1] #99ffd1" rather than a package.
            // Their Description carries no Root prefix either, hence the bare focus string.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, "Slice1")
                .AsString();
            return VerifyUml(uml);
        }

        // --- Cyclic slice dependencies are folded into a single Circle dependency ----

        [Fact]
        public Task BuildUmlBySlices_DirectCircle_RendersCircleDependency()
        {
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().Matching("SlicesTestAssembly.DirectCircle.(*)")
            );
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices).AsString();
            return VerifyUml(uml);
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
            return VerifyUml(uml);
        }

        // --- Packages with the same name under different parents stay apart ----------
        //
        // Orders and Billing both contain a Domain package. PlantUML identifies a package by
        // its name, so unless each one is identified by its full path, both Domain packages
        // are drawn as one, and the arrows into Billing.Domain end in Orders.Domain.

        private const string SameNamedRoot = "SlicesTestAssembly.SameNamedSubnamespaces.";

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_SameNamedPackages()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(SameNamedRoot + "(**)")
            );
        }

        [Fact]
        public Task BuildUmlBySlicesMatchingWithPackages_SameNamedPackages_C4Style()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(SameNamedRoot + "(**)"),
                new GenerationOptions { C4Style = true }
            );
        }

        // --- LimitDependencies without namespaces takes the OneToOneCompact branch ----

        [Fact]
        public Task BuildUmlBySlices_LimitDependencies_Compact()
        {
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(slices, new GenerationOptions { LimitDependencies = true })
                .AsString();
            return VerifyUml(uml);
        }

        // --- IncludeNodesWithoutDependencies = false removes dependency-less slices ----

        [Fact]
        public Task BuildUmlBySlices_ExcludeNodesWithoutDependencies()
        {
            // "(**)" keeps every namespace as a slice of its own, including ones without any
            // dependency, so there is something for IncludeNodesWithoutDependencies to remove.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(
                    slices,
                    new GenerationOptions { IncludeNodesWithoutDependencies = false }
                )
                .AsString();
            return VerifyUml(uml);
        }

        // --- A DependencyFilter is honoured when building slice diagrams --------------

        [Fact]
        public Task BuildUmlBySlices_WithDependencyFilter()
        {
            // "(**)" gives Slice3.Group1 a slice of its own, so there is a dependency for the
            // filter to remove.
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
            return VerifyUml(uml);
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

        // --- FocusOn with a single-asterisk pattern ----------------------------------

        [Fact]
        public Task BuildUmlBySlicesFocusOn_SingleAsteriskPattern_SkipsSlicesWithoutFocusString()
        {
            // Only "Slice3" contains the focus string, so it alone is coloured; the other slices
            // are drawn because they depend on it or it depends on them.
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(*)")
            );
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3")
                .AsString();
            return VerifyUml(uml);
        }

        /// <summary>
        /// GetObjects returns slices in Mono.Cecil's type-table order, which is deterministic
        /// per build but not contractually stable across Cecil or runtime upgrades. Sorting by
        /// description keeps the node and dependency order in the snapshots below stable
        /// regardless of that traversal order.
        /// </summary>
        private static Slice[] SortedSlices(GivenSlices slices)
        {
            return slices
                .GetObjects(Architecture)
                .OrderBy(slice => slice.Description, StringComparer.Ordinal)
                .ToArray();
        }

        private static Task VerifyUml(string uml)
        {
            return Verifier
                .Verify(uml)
                .DisableDiff() // Don't open diff tool during the test
                .UseDirectory("Snapshots");
        }

        private static Task VerifySlices(
            GivenSlices slices,
            GenerationOptions generationOptions = null
        )
        {
            return VerifyUml(
                new PlantUmlFileBuilder()
                    .WithDependenciesFrom(SortedSlices(slices), generationOptions)
                    .AsString()
            );
        }
    }
}
