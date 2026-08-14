using System;
using System.Threading.Tasks;
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

        private static Task VerifySlices(
            GivenSlices slices,
            GenerationOptions generationOptions = null
        )
        {
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFrom(slices.GetObjects(Architecture), generationOptions)
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
            var slices = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(Root + "(**)")
                .GetObjects(Architecture);
            var uml = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(slices, Root + "Slice1")
                .AsString();
            return Verifier.Verify(uml).DisableDiff().UseDirectory("Snapshots");
        }
    }
}
