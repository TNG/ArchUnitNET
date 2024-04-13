using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBePrivateTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(Types().That().Are(t.PrivateInnerClass).Should().BePrivate());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.PublicClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.PublicInnerClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.InternalClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.InternalInnerClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.ProtectedInnerClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.ProtectedInternalInnerClass).Should().BePrivate());
        t.AssertViolations(Types().That().Are(t.PrivateProtectedInnerClass).Should().BePrivate());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types().That().Are(t.PrivateInnerClass, t.OtherPrivateInnerClass).Should().BePrivate()
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.PrivateInnerClass, t.PrivateProtectedInnerClass)
                .Should()
                .BePrivate()
        );
        await t.AssertSnapshotMatches();
    }
}
