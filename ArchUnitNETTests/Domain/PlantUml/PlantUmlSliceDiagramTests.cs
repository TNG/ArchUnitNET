using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            // Nothing precedes the capture group, so every slice is named after a whole
            // namespace, but it is nested where its types are: in that namespace's "Service".
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
            // PlantUmlSlice.BuildString's namespace-less branch with a colour set, which renders
            // "[Slice Slice1] as Slice_Slice1 #99ffd1". The focus string is a namespace, flat or
            // nested.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice1")
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

        // --- LimitDependencies only keeps arrows between slices at the same depth ------
        //
        // A slice's depth is that of the namespace all its types share, so Slice2.Service ->
        // Slice3 and Slice3.Group1 -> Slice3 are dropped, flat and nested alike.

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
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(*)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, "NotAPackage", Nested)
            );
            Assert.Equal("No slice lies in the namespace [NotAPackage]", ex.Message);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_NamespaceIsMatchedBySegments()
        {
            // The substring "Slice" is contained in every slice name, but no slice lies in a
            // namespace called "Slice".
            var slices = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(**)")
                .GetObjects(Architecture);
            var ex = Assert.Throws<ArgumentException>(() =>
                new PlantUmlFileBuilder().WithDependenciesFromFocusOn(slices, Root + "Slice")
            );
            Assert.Equal(
                "No slice lies in the namespace [SlicesTestAssembly.MultipleSubnamespaces.Slice]",
                ex.Message
            );
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

        // --- Which slices FocusOn focuses ------------------------------------------------
        //
        // A slice is focused when all of its types lie in the namespace focused on or below
        // it, which is a question about the types, so it gets the same answer flat and nested.

        [Fact]
        public Task BuildUmlBySlicesFocusOn_SingleAsteriskPattern()
        {
            // RemovePatternInappropriateSlices leaves the three slices "(*)" stands for, of
            // which "Slice3" lies in the namespace focused on.
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(*)"));
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3", Nested)
                .AsString();
            return VerifyUml(uml);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_SiblingWithSamePrefix_IsNotFocused()
        {
            // OrdersArchive starts with "Orders", but does not lie in the namespace Orders. It
            // is drawn as a neighbour, because it depends on Orders.
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().Matching("SlicesTestAssembly.PrefixSiblings.(*)")
            );
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, "SlicesTestAssembly.PrefixSiblings.Orders")
                .AsString();

            Assert.Contains("[Slice Orders] as Slice_Orders #99ffd1", uml);
            Assert.Contains(
                "[Slice OrdersArchive] as Slice_OrdersArchive" + Environment.NewLine,
                uml
            );
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_SliceSpanningTheNamespace_IsANeighbour()
        {
            // "Services" has types in Slice1 and Slice2, so focusing on Slice1 does not focus it,
            // and neither does focusing on Slice3, which it depends on.
            var slices = SlicesWithServices();
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3", Nested)
                .AsString();

            Assert.Contains("[Slice Slice3] as Slice_Slice3 #99ffd1", uml);
            Assert.Contains("[Slice Services] as Slice_Services" + Environment.NewLine, uml);
            Assert.Contains("[Slice_Services] --|> [Slice_Slice3]", uml);
        }

        [Fact]
        public void BuildUmlBySlicesFocusOn_FocusesTheSameSlicesFlatAndNested()
        {
            var slices = SlicesWithServices();

            var flat = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3")
                .AsString();
            var nested = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice3", Nested)
                .AsString();

            Assert.Equal(DrawnSlices(flat), DrawnSlices(nested));
            Assert.Contains("Slice Slice3.Group1 (focused)", DrawnSlices(flat));
        }

        // --- Where nested slices are drawn -----------------------------------------------
        //
        // A nested slice sits in the package of the deepest namespace that contains all of its
        // types. The packages come from those namespaces, not from the pattern.

        [Fact]
        public Task BuildUmlBySlicesNested_DuplicatePrefixSegment()
        {
            // The leading ".." matches "SlicesTestAssembly.DuplicatePrefix.Sub.", but that is
            // not what the package is made of: the slice's type lies in
            // "SlicesTestAssembly.DuplicatePrefix.Sub.DuplicatePrefix.Sub", and there is no
            // package ".DuplicatePrefix.Sub".
            return VerifySlices(
                SliceRuleDefinition.Slices().Matching("..DuplicatePrefix.Sub.(**)"),
                Nested
            );
        }

        [Fact]
        public void BuildUmlBySlicesNested_SliceSitsInItsOwnNamespace()
        {
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"));
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();

            Assert.Contains(
                Lines(
                    "  package \"Slice3\" as SlicesTestAssembly.MultipleSubnamespaces.Slice3 {",
                    "    [Slice Slice3] as Slice_Slice3",
                    "    package \"Group1\" as SlicesTestAssembly.MultipleSubnamespaces.Slice3.Group1 {",
                    "      [Slice Slice3.Group1] as Slice_Slice3.Group1"
                ),
                uml
            );
        }

        [Fact]
        public void BuildUmlBySlicesNested_PackageWithASinglePackage_IsMerged()
        {
            // Orders holds no slice of its own and only the package Domain.
            var slices = SortedSlices(
                SliceRuleDefinition.Slices().Matching(SameNamedRoot + "(**)")
            );
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();

            Assert.Contains(
                "package \"Orders.Domain\" as SlicesTestAssembly.SameNamedSubnamespaces.Orders.Domain {",
                uml
            );
            Assert.DoesNotContain("package \"Orders\"", uml);
        }

        [Fact]
        public void BuildUmlBySlicesNested_CustomSliceAssignment()
        {
            var slices = SlicesWithServices();
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();

            Assert.Contains(
                Lines(
                    "package \"SlicesTestAssembly.MultipleSubnamespaces\" as SlicesTestAssembly.MultipleSubnamespaces {",
                    "  [Slice Services] as Slice_Services"
                ),
                uml
            );
        }

        [Fact]
        public void BuildUmlBySlicesNested_SliceWithoutCommonNamespace_IsDrawnOnItsOwn()
        {
            var slice1 = Architecture.Types.Single(type =>
                type.Name == "Slice1Class" && type.Namespace.FullName == Root + "Slice1"
            );
            var objectType = Architecture.ReferencedTypes.Single(type =>
                type.FullName == "System.Object"
            );
            var slices = SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)"))
                .Concat(
                    new[] { new Slice(SliceIdentifier.Of("Mixed"), new[] { slice1, objectType }) }
                )
                .ToList();
            var uml = new PlantUmlFileBuilder().WithDependenciesFrom(slices, Nested).AsString();

            Assert.Contains(
                Environment.NewLine + "[Slice Mixed] as Slice_Mixed" + Environment.NewLine,
                uml
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
            Assert.Contains("[Slice Slice1] as Slice_Slice1", nested);
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

        /// <summary>
        /// The slices of a custom assignment that gives every namespace below <see cref="Root"/>
        /// its own slice, except Slice1.Service and Slice2.Service, which form the single slice
        /// "Services".
        /// </summary>
        private static Slice[] SlicesWithServices()
        {
            var assignment = new SliceAssignment(
                type =>
                {
                    var nameSpace = type.Namespace?.FullName ?? "";
                    if (!nameSpace.StartsWith(Root, StringComparison.Ordinal))
                    {
                        return SliceIdentifier.Ignore();
                    }

                    return nameSpace.EndsWith(".Service", StringComparison.Ordinal)
                        ? SliceIdentifier.Of("Services")
                        : SliceIdentifier.Of(nameSpace.Substring(Root.Length));
                },
                "custom"
            );
            return assignment
                .Apply(Architecture.Types)
                .Where(slice => !slice.Identifier.Ignored)
                .OrderBy(slice => slice.Description, StringComparer.Ordinal)
                .ToArray();
        }

        /// <summary>
        /// The slices a diagram declares, and whether each one is colored as focused.
        /// </summary>
        private static string[] DrawnSlices(string uml)
        {
            return uml.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(line => SliceDeclaration.Match(line))
                .Where(match => match.Success)
                .Select(match =>
                    match.Groups["name"].Value + (match.Groups["color"].Success ? " (focused)" : "")
                )
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();
        }

        private static readonly Regex SliceDeclaration = new Regex(
            @"^\s*\[(?<name>[^\]]+)\](?: as \S+)?(?<color> #99ffd1)?$"
        );

        private static string Lines(params string[] lines)
        {
            return string.Join(Environment.NewLine, lines);
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
