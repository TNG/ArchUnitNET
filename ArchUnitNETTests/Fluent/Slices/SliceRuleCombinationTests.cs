using System.Threading.Tasks;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    /// Covers <see cref="SliceRule" />'s four combinators. Each is exercised for both evaluation
    /// outcome and the composed <c>Description</c>, which
    /// <see cref="CombinedArchRule.Description" /> builds by concatenating the two rules'
    /// descriptions with the combinator's conjunction; the snapshot's <c>Query:</c> line pins that
    /// description verbatim, so no separate assertion on it is needed.
    /// </summary>
    /// <remarks>
    /// Outcomes are asserted with <see cref="AssemblyTestHelper.AssemblyTestHelper.AssertNoViolations" />
    /// and <see cref="AssemblyTestHelper.AssemblyTestHelper.AssertHasViolations" />, the two helper
    /// assertions that read a rule's verdict. The result-shaped <c>AssertAnyViolations</c> and
    /// <c>AssertOnlyViolations</c> would be wrong here: a combined rule's results are a plain
    /// concatenation of its operands', so "the results contain a failure" holds for a passing
    /// <c>Or</c> just as it does for a failing <c>And</c>, and would say nothing about the
    /// combinator.
    /// </remarks>
    public class SliceRuleCombinationTests
    {
        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        /// <summary>
        /// Deliberately narrow: the bare <c>Types().Should().Exist()</c> that reads most naturally
        /// here yields one passing result per type in the assembly, which would bury the point of
        /// each snapshot and churn whenever a fixture type is added.
        /// </summary>
        private const string SinglePassingTypeName =
            "SlicesTestAssembly.DotDotSemantics.Outer.Mid.Inner.MidInnerClass";

        private static SliceRule CyclicSliceRule =>
            SliceRuleDefinition.Slices().Matching(Root + "(**)").Should().BeFreeOfCycles();

        // MultipleSubnamespaces has a cycle under every grouping (see
        // SlicesTests.CycleDetectionTest), so the acyclic operand comes from SubnamespaceCircle,
        // whose "(**)" slices depend on each other without forming a cycle.
        private static SliceRule AcyclicSliceRule =>
            SliceRuleDefinition
                .Slices()
                .Matching("SlicesTestAssembly.SubnamespaceCircle.(**)")
                .Should()
                .BeFreeOfCycles();

        private static IArchRule SinglePassingTypeRule =>
            Types().That().HaveFullName(SinglePassingTypeName).Should().Exist();

        private static IArchRule FailingTypesRule(SlicesAssemblyTestHelper helper) =>
            Types().That().HaveName(helper.NonExistentObjectName).Should().Exist();

        [Fact]
        public async Task And_FluentDefinition_CombinesEvaluationAndDescription()
        {
            var helper = new SlicesAssemblyTestHelper();

            helper.AddSnapshotHeader("Slice operand on its own");
            AcyclicSliceRule.AssertNoViolations(helper);

            helper.AddSnapshotHeader("Both operands pass");
            AcyclicSliceRule
                .And()
                .Types()
                .That()
                .HaveFullName(SinglePassingTypeName)
                .Should()
                .Exist()
                .AssertNoViolations(helper);

            helper.AddSnapshotHeader("Slice operand fails");
            CyclicSliceRule
                .And()
                .Types()
                .That()
                .HaveFullName(SinglePassingTypeName)
                .Should()
                .Exist()
                .AssertHasViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task Or_FluentDefinition_CombinesEvaluationAndDescription()
        {
            var helper = new SlicesAssemblyTestHelper();

            helper.AddSnapshotHeader("Slice operand on its own");
            CyclicSliceRule.AssertHasViolations(helper);

            helper.AddSnapshotHeader("Second operand rescues the failing slice operand");
            CyclicSliceRule
                .Or()
                .Types()
                .That()
                .HaveFullName(SinglePassingTypeName)
                .Should()
                .Exist()
                .AssertNoViolations(helper);

            helper.AddSnapshotHeader("Both operands fail");
            CyclicSliceRule
                .Or()
                .Types()
                .That()
                .HaveName(helper.NonExistentObjectName)
                .Should()
                .Exist()
                .AssertHasViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task And_WithArchRule_CombinesEvaluationAndDescription()
        {
            var helper = new SlicesAssemblyTestHelper();

            helper.AddSnapshotHeader("Both operands pass");
            AcyclicSliceRule.And(SinglePassingTypeRule).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Second operand fails");
            AcyclicSliceRule.And(FailingTypesRule(helper)).AssertHasViolations(helper);

            helper.AddSnapshotHeader("Both operands fail");
            CyclicSliceRule.And(FailingTypesRule(helper)).AssertHasViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task Or_WithArchRule_CombinesEvaluationAndDescription()
        {
            var helper = new SlicesAssemblyTestHelper();

            // The verdict passes while the recorded results still report violations, and the
            // rendered message announces a failure for a rule that passed: CombinedArchRule
            // applies the conjunction in HasNoViolations but never in Evaluate, so the cyclic
            // operand's failures survive into the message. Pinned as current behaviour, not
            // endorsed -- a user calling Evaluate directly sees violations for a rule that Check
            // accepts. See AssemblyTestHelper.AssertNoViolations for what this forced on the
            // helper.
            helper.AddSnapshotHeader("One operand passes, and the other's violations still show");
            CyclicSliceRule.Or(SinglePassingTypeRule).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Both operands fail");
            CyclicSliceRule.Or(FailingTypesRule(helper)).AssertHasViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
