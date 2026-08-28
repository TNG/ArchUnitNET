using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    ///     Covers <see cref="SliceRule" />'s four combinators. Each is exercised for both
    ///     evaluation outcome and the composed <c>Description</c>, since the description is what
    ///     <see cref="ArchUnitNET.Fluent.Slices.SliceRuleCreator.AddToDescription" /> feeds.
    /// </summary>
    public class SliceRuleCombinationTests
    {
        private static ArchUnitNET.Domain.Architecture Architecture =>
            StaticTestArchitectures.SlicesTestArchitecture;

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        private static SliceRule CyclicSliceRule =>
            SliceRuleDefinition.Slices().Matching(Root + "(**)").Should().BeFreeOfCycles();

        private static SliceRule AcyclicSliceRule =>
            SliceRuleDefinition.Slices().Matching(Root + "(**)..").Should().BeFreeOfCycles();

        private static IArchRule PassingTypesRule => Types().Should().Exist();

        private static IArchRule FailingTypesRule =>
            Types().That().HaveName("ThisNameDoesNotExistAnywhere12345").Should().Exist();

        [Fact]
        public void And_FluentDefinition_CombinesEvaluationAndDescription()
        {
            var passing = AcyclicSliceRule.And().Types().Should().Exist();
            var failing = CyclicSliceRule.And().Types().Should().Exist();

            Assert.True(AcyclicSliceRule.HasNoViolations(Architecture));
            Assert.True(passing.HasNoViolations(Architecture));
            Assert.False(failing.HasNoViolations(Architecture));

            Assert.StartsWith(AcyclicSliceRule.Description, passing.Description);
            Assert.Contains(" and ", passing.Description);
            Assert.EndsWith("Types should exist", passing.Description);
        }

        [Fact]
        public void Or_FluentDefinition_CombinesEvaluationAndDescription()
        {
            var stillPasses = CyclicSliceRule.Or().Types().Should().Exist();
            var stillFails = CyclicSliceRule
                .Or()
                .Types()
                .That()
                .HaveName("ThisNameDoesNotExistAnywhere12345")
                .Should()
                .Exist();

            Assert.False(CyclicSliceRule.HasNoViolations(Architecture));
            Assert.True(stillPasses.HasNoViolations(Architecture));
            Assert.False(stillFails.HasNoViolations(Architecture));

            Assert.StartsWith(CyclicSliceRule.Description, stillPasses.Description);
            Assert.Contains(" or ", stillPasses.Description);
            Assert.EndsWith("Types should exist", stillPasses.Description);
        }

        [Fact]
        public void And_WithArchRule_CombinesEvaluationAndDescription()
        {
            var passing = AcyclicSliceRule.And(PassingTypesRule);
            var failing = AcyclicSliceRule.And(FailingTypesRule);
            var bothFail = CyclicSliceRule.And(FailingTypesRule);

            Assert.True(passing.HasNoViolations(Architecture));
            Assert.False(failing.HasNoViolations(Architecture));
            Assert.False(bothFail.HasNoViolations(Architecture));

            Assert.Equal(
                AcyclicSliceRule.Description + " and " + PassingTypesRule.Description,
                passing.Description
            );
        }

        [Fact]
        public void Or_WithArchRule_CombinesEvaluationAndDescription()
        {
            var eitherPasses = CyclicSliceRule.Or(PassingTypesRule);
            var bothFail = CyclicSliceRule.Or(FailingTypesRule);

            Assert.True(eitherPasses.HasNoViolations(Architecture));
            Assert.False(bothFail.HasNoViolations(Architecture));

            Assert.Equal(
                CyclicSliceRule.Description + " or " + PassingTypesRule.Description,
                eitherPasses.Description
            );
        }
    }
}
