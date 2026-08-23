using System.Threading.Tasks;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

// Like the other syntax element tests this keeps each fluent chain on one line, so the file is listed in
// .csharpierignore. A // csharpier-ignore comment on the class is not enough here: csharpier ignored it
// under the file-scoped namespace and reformatted the chains anyway, which is also why the two other
// file-scoped test files are in .csharpierignore rather than carrying the comment.
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

        helper.AddSnapshotHeader("No Violations");
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

    // The conjunctions are the only field member specific classes with behaviour of their own, so they get
    // their own test: And() on the predicate side reaches GivenFieldMembersConjunction and hands the rule
    // creator back to a GivenFieldMembersThat, AndShould() does the same for FieldMembersShouldConjunction
    // and FieldMembersShould.
    [Fact]
    public async Task ConjunctionTest()
    {
        var helper = new TypeAssemblyTestHelper();

        // Unlike the test above, every chain is spelled out from FieldMembers(). AndShould() appends a second
        // condition element to the shared rule creator instead of overwriting the last one, so a reused
        // should variable would carry that condition into the next assertion.
        helper.AddSnapshotHeader("No Violations");

        helper.AddSnapshotSubHeader("Conditions");
        // Of the two inputs only the static one survives the predicate conjunction, so the rule still has
        // results and the assertion is not vacuous.
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
