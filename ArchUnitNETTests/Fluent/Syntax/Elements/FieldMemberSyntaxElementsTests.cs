using System.Threading.Tasks;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class FieldMemberSyntaxElementsTests
    {
        // This class is deliberately short. There are no field-specific syntax elements: FieldMembers() only
        // adds the member and object syntax, both of which have their own test files. So this is a routing
        // smoke test proving that the FieldMembers() chain reaches those shared elements — it is not, and does
        // not need to be, a per-element suite.

        [Fact]
        public async Task BeStaticTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No violations");
            var should = FieldMembers().That().Are(helper.StaticField).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeStatic().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(FieldMembers().That().AreStatic()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = FieldMembers().That().Are(helper.NonStaticField).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeStatic().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(FieldMembers().That().AreStatic()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            FieldMembers().That().Are(helper.StaticField, helper.OtherStaticField).Should().BeStatic().AssertNoViolations(helper);
            FieldMembers().That().Are(helper.StaticField, helper.NonStaticField).Should().BeStatic().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task ConjunctionTest()
        {
            var helper = new TypeAssemblyTestHelper();

            helper.AddSnapshotHeader("No violations");

            helper.AddSnapshotSubHeader("Conditions");
            FieldMembers().That().Are(helper.StaticField, helper.ReadOnlyField).And().AreStatic().Should().BeStatic().AndShould().NotBeReadOnly().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            FieldMembers().That().Are(helper.StaticField, helper.ReadOnlyField).And().AreStatic().Should().Be(FieldMembers().That().AreStatic().And().AreNotReadOnly()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");

            helper.AddSnapshotSubHeader("Conditions");
            FieldMembers().That().Are(helper.StaticField, helper.ReadOnlyField).And().AreNotStatic().Should().BeStatic().AndShould().NotBeReadOnly().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            FieldMembers().That().Are(helper.StaticField, helper.ReadOnlyField).And().AreNotStatic().Should().Be(FieldMembers().That().AreStatic().And().AreNotReadOnly()).AssertOnlyViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
