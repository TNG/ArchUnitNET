using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBeProtectedTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(Types().That().Are(t.ProtectedInnerClass).Should().BeProtected());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.PublicClass).Should().BeProtected());
        t.AssertViolations(Types().That().Are(t.PublicInnerClass).Should().BeProtected());
        t.AssertViolations(Types().That().Are(t.InternalClass).Should().BeProtected());
        t.AssertViolations(Types().That().Are(t.InternalInnerClass).Should().BeProtected());
        t.AssertViolations(Types().That().Are(t.PrivateInnerClass).Should().BeProtected());
        t.AssertViolations(Types().That().Are(t.PrivateProtectedInnerClass).Should().BeProtected());
        t.AssertViolations(
            Types().That().Are(t.ProtectedInternalInnerClass).Should().BeProtected()
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types()
                .That()
                .Are(t.ProtectedInnerClass, t.OtherProtectedInnerClass)
                .Should()
                .BeProtected()
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ProtectedInnerClass, t.ProtectedInternalInnerClass)
                .Should()
                .BeProtected()
        );
        await t.AssertSnapshotMatches();
    }
}
