using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using Xunit;

// These tests cover the obsolete method itself.
#pragma warning disable CS0618

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    /// <see cref="SliceRuleInitializer.MatchingWithPackages"/> is obsolete. It creates exactly the
    /// slices <see cref="SliceRuleInitializer.Matching"/> creates, so its rules behave the same,
    /// and the only thing it still does differently is nest its slices in diagrams by default.
    /// </summary>
    public class MatchingWithPackagesTests
    {
        private static Architecture Architecture => StaticTestArchitectures.SlicesTestArchitecture;

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        public static TheoryData<string> Patterns =>
            new TheoryData<string>
            {
                Root + "(*)",
                Root + "(**)",
                Root + "(**)..",
                Root + "(*).(*)",
                Root + "(*)..(*)",
                "..DuplicatePrefix.Sub.(**)",
                "(**).Service",
                "SlicesTestAssembly.SubnamespaceCircle.(**)",
            };

        [Theory]
        [MemberData(nameof(Patterns))]
        public void Slices_AreThoseOfMatching(string pattern)
        {
            var withPackages = SliceRuleDefinition
                .Slices()
                .MatchingWithPackages(pattern)
                .GetObjects(Architecture)
                .ToList();
            var matching = SliceRuleDefinition.Slices().Matching(pattern).GetObjects(Architecture);

            Assert.Equal(matching.Select(Describe), withPackages.Select(Describe));
        }

        [Theory]
        [MemberData(nameof(Patterns))]
        public void Rules_EvaluateLikeThoseOfMatching(string pattern)
        {
            AssertSameResults(
                SliceRuleDefinition.Slices().Matching(pattern).Should().BeFreeOfCycles(),
                SliceRuleDefinition.Slices().MatchingWithPackages(pattern).Should().BeFreeOfCycles()
            );
            AssertSameResults(
                SliceRuleDefinition.Slices().Matching(pattern).Should().NotDependOnEachOther(),
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages(pattern)
                    .Should()
                    .NotDependOnEachOther()
            );
        }

        [Fact]
        public void BeFreeOfCycles_DetectsCycle()
        {
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages(Root + "(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(Architecture)
            );
        }

        // SubnamespaceCircle has real edges (Slice1 -> Slice2, Slice2.Inner -> Slice1) but no
        // cycle, so an empty graph is not what makes this pass.
        [Fact]
        public void BeFreeOfCycles_WhenAcyclic_Passes()
        {
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .MatchingWithPackages("SlicesTestAssembly.SubnamespaceCircle.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(Architecture)
            );
        }

        [Theory]
        [MemberData(nameof(Patterns))]
        public void Diagram_IsThatOfMatchingNestedInPackages(string pattern)
        {
            foreach (
                var options in new[]
                {
                    new GenerationOptions(),
                    new GenerationOptions { C4Style = true },
                    new GenerationOptions { LimitDependencies = true },
                }
            )
            {
                var expected = new PlantUmlFileBuilder()
                    .WithDependenciesFrom(
                        SortedSlices(SliceRuleDefinition.Slices().Matching(pattern)),
                        new GenerationOptions
                        {
                            NestSlicesInPackages = true,
                            C4Style = options.C4Style,
                            LimitDependencies = options.LimitDependencies,
                        }
                    )
                    .AsString();
                var actual = new PlantUmlFileBuilder()
                    .WithDependenciesFrom(
                        SortedSlices(SliceRuleDefinition.Slices().MatchingWithPackages(pattern)),
                        options
                    )
                    .AsString();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Diagram_FocusOn_IsThatOfMatchingNestedInPackages()
        {
            var expected = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(
                    SortedSlices(SliceRuleDefinition.Slices().Matching(Root + "(**)")),
                    Root + "Slice1",
                    new GenerationOptions { NestSlicesInPackages = true }
                )
                .AsString();
            var actual = new PlantUmlFileBuilder()
                .WithDependenciesFromFocusOn(
                    SortedSlices(SliceRuleDefinition.Slices().MatchingWithPackages(Root + "(**)")),
                    Root + "Slice1"
                )
                .AsString();

            Assert.Equal(expected, actual);
            Assert.Contains("package \"SlicesTestAssembly.MultipleSubnamespaces\"", actual);
        }

        private static string Describe(Slice slice)
        {
            return slice.Description
                + ": "
                + string.Join(
                    ", ",
                    slice
                        .Types.Select(type => type.FullName)
                        .OrderBy(n => n, StringComparer.Ordinal)
                );
        }

        private static void AssertSameResults(IArchRule matching, IArchRule withPackages)
        {
            Assert.Equal(matching.Description, withPackages.Description);
            Assert.Equal(
                matching.Evaluate(Architecture).Select(Summarize),
                withPackages.Evaluate(Architecture).Select(Summarize)
            );
        }

        private static string Summarize(EvaluationResult result)
        {
            return result.Passed
                + " "
                + result.EvaluatedObjectIdentifier.Identifier
                + " "
                + result.Description;
        }

        private static Slice[] SortedSlices(GivenSlices slices)
        {
            return slices
                .GetObjects(Architecture)
                .OrderBy(slice => slice.Description, StringComparer.Ordinal)
                .ToArray();
        }
    }
}
