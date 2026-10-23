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

        private static GenerationOptions Nested =>
            new GenerationOptions { NestSlicesInPackages = true };

        // --- The asterisk count decides which slices the exporter renders ---------------
        //
        // Every pattern below matches the same nine slices -- SlicesTests.MatchingTest and
        // Matching_SingleAsterisk_CapturesEveryDepth assert that. What differs is how many
        // of them survive PlantUmlFileBuilder.RemovePatternInappropriateSlices, which drops
        // a slice once its depth (dots in its Description) reaches
        // Slice.CountOfAsteriskInPattern:
        //
        //     (*)       count 1     ->  3 of 9 slices rendered
        //     (*).(*)   count 2     ->  7 of 9 slices rendered
        //     (**)      count null  ->  9 of 9 slices rendered
        //
        // CountOfAsteriskInPattern has no other consumer in the library: the slice domain
        // carries it solely so the exporter can perform that deletion. So these snapshots
        // are the only record that asking for "(*)" hands the diagram two-thirds fewer
        // nodes than GetObjects returned, and a reimplementation is free to drop the field
        // from SliceIdentifier only if it also changes what these files say.

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

        // The matcher discards everything after the first "(*", so "(*).(*)" and "(*)..(*)"
        // are indistinguishable to it and the next two snapshots are byte-identical -- as are
        // their nested counterparts below. That identity is the point: it is
        // what a reimplementation giving the second capture group any meaning would have to
        // change. SlicesTests.DotDot_* cover the shapes that would tell the two patterns
        // apart.
        //
        // Identical snapshots do not make these tests redundant with "(*)", though. The
        // second group is dead to the matcher but still raises CountOfAsteriskInPattern to
        // 2, which moves the exporter's cutoff -- hence seven nodes here against three for
        // "(*)".
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
        public Task BuildUmlBySlicesNested_SingleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*)"), Nested);
        }

        [Fact]
        public Task BuildUmlBySlicesNested_DoubleAsterisk()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"), Nested);
        }

        [Fact]
        public Task BuildUmlBySlicesNested_MultipleCaptureGroups()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*).(*)"), Nested);
        }

        [Fact]
        public Task BuildUmlBySlicesNested_NonContiguousCaptureGroups()
        {
            return VerifySlices(SliceRuleDefinition.Slices().Matching(Root + "(*)..(*)"), Nested);
        }

        [Fact]
        public Task BuildUmlBySlicesNested_CaptureGroupAtStart()
        {
            // Nothing precedes the capture group, so there is no namespace to nest the slices
            // under and they are drawn flat, like those of Matching.
            return VerifySlices(SliceRuleDefinition.Slices().Matching("(**).Service"), Nested);
        }

        [Fact]
        public Task BuildUmlBySlicesNested_LimitDependencies()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(Root + "(**)"),
                new GenerationOptions { NestSlicesInPackages = true, LimitDependencies = true }
            );
        }

        [Fact]
        public Task BuildUmlBySlicesNested_C4Style()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(Root + "(**)"),
                new GenerationOptions { NestSlicesInPackages = true, C4Style = true }
            );
        }

        [Fact]
        public Task BuildUmlBySlices_C4Style()
        {
            // Slices that are not nested have no namespace prefix, so this exercises
            // PlantUmlSlice.BuildStringC4Style's no-namespace branch.
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(Root + "(**)"),
                new GenerationOptions { C4Style = true }
            );
        }

        [Fact]
        public Task BuildUmlBySlicesFocusOn()
        {
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice1", Nested)
                .AsString();
            return VerifyUml(uml);
        }

        [Fact]
        public Task BuildUmlBySlicesFocusOn_Matching_ColorsFocusedSlices()
        {
            // Slices that are not nested have no namespace, so the focused ones take
            // PlantUmlSlice.BuildString's namespace-less branch with a colour set -- the only
            // path on which the exporter renders "[Slice Slice1] #99ffd1" rather than a package.
            // They are drawn under their description, which "Slice1" is contained in.
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
        public Task BuildUmlBySlicesNested_DirectCircle()
        {
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().Matching("SlicesTestAssembly.DirectCircle.(*)")
            );
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();
            return VerifyUml(uml);
        }

        // --- Packages with the same name under different parents stay apart ----------
        //
        // Orders and Billing both contain a Domain package. PlantUML identifies a package by
        // its name, so unless each one is identified by its full path, both Domain packages
        // are drawn as one, and the arrows into Billing.Domain end in Orders.Domain.

        private const string SameNamedRoot = "SlicesTestAssembly.SameNamedSubnamespaces.";

        [Fact]
        public Task BuildUmlBySlicesNested_SameNamedPackages()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(SameNamedRoot + "(**)"),
                Nested
            );
        }

        [Fact]
        public Task BuildUmlBySlicesNested_SameNamedPackages_C4Style()
        {
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching(SameNamedRoot + "(**)"),
                new GenerationOptions { NestSlicesInPackages = true, C4Style = true }
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
            return VerifyUml(uml);
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
            return VerifyUml(uml);
        }

        // --- WithDependenciesFromFocusOn argument validation ---------------------------

        [Fact]
        public void BuildUmlBySlicesFocusOn_EmptyPackage_Throws()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, "", Nested)
            );
            Assert.Equal("Package can't be empty", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_DotOnlyPackage_Throws()
        {
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, ".", Nested)
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
                .Matching(Root + "(*)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, "NotAPackage", Nested)
            );
            Assert.Contains("is not contained in this slice", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_TrailingDotPackage_IsTrimmed()
        {
            var slicesWithTrailingDot = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            var umlWithTrailingDot = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slicesWithTrailingDot, Root + "Slice1.", Nested)
                .AsString();

            var slicesWithoutTrailingDot = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            var umlWithoutTrailingDot = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slicesWithoutTrailingDot, Root + "Slice1", Nested)
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
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(*)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3", Nested)
                .AsString();
            return VerifyUml(uml);
        }

        // --- Where nested slices are rooted ----------------------------------------------
        //
        // A nested slice's root package is the namespace its types matched before the first
        // capture group, so it has to come from the match, not from the pattern text.

        [Fact]
        public Task BuildUmlBySlicesNested_DuplicatePrefixSegment()
        {
            // The leading ".." matches "SlicesTestAssembly.DuplicatePrefix.Sub.", so that whole
            // namespace is the root package, although "DuplicatePrefix.Sub" occurs in it twice.
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching("..DuplicatePrefix.Sub.(**)"),
                Nested
            );
        }

        // --- One set of slices, drawn flat and nested ------------------------------------

        [Fact]
        public void BuildUmlBySlices_SameSlicesDrawnFlatAndNested()
        {
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));

            var flat = new PlantUmlFileBuilder().WithDependenciesFrom(slices).AsString();
            var nested = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();

            Assert.DoesNotContain("package", flat);
            Assert.Contains("[Slice Slice1] as Slice_Slice1", flat);
            Assert.Contains("package \"SlicesTestAssembly.MultipleSubnamespaces\"", nested);
            Assert.DoesNotContain("Slice Slice1", nested);
        }

        // --- Two slices may not share a node ----------------------------------------------

        [Fact]
        public void BuildUmlBySlices_SameDiagramName_Throws()
        {
            // Different slices, both described "Slice A - B".
            var slices = new[]
            {
                new Slice(SliceIdentifier.Of(new[] { "A - B" }), Array.Empty<IType>()),
                new Slice(SliceIdentifier.Of(new[] { "A", "B" }), Array.Empty<IType>()),
            };

            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFrom(slices)
            );
            Assert.Equal(
                "The slices with parts [\"A - B\"] and [\"A\", \"B\"] would both be drawn as \"Slice A - B\".",
                ex.Message
            );
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
