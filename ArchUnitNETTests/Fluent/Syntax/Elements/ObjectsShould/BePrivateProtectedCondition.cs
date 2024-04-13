using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBePrivateProtectedTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types().That().Are(t.PrivateProtectedInnerClass).Should().BePrivateProtected()
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.PublicClass).Should().BePrivateProtected());
        t.AssertViolations(Types().That().Are(t.PublicInnerClass).Should().BePrivateProtected());
        t.AssertViolations(Types().That().Are(t.ProtectedInnerClass).Should().BePrivateProtected());
        t.AssertViolations(
            Types().That().Are(t.ProtectedInternalInnerClass).Should().BePrivateProtected()
        );
        t.AssertViolations(Types().That().Are(t.InternalClass).Should().BePrivateProtected());
        t.AssertViolations(Types().That().Are(t.InternalInnerClass).Should().BePrivateProtected());
        t.AssertViolations(Types().That().Are(t.PrivateInnerClass).Should().BePrivateProtected());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types()
                .That()
                .Are(t.PrivateProtectedInnerClass, t.OtherPrivateProtectedInnerClass)
                .Should()
                .BePrivateProtected()
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.PrivateProtectedInnerClass, t.PrivateInnerClass)
                .Should()
                .BePrivateProtected()
        );
        await t.AssertSnapshotMatches();
    }
}
