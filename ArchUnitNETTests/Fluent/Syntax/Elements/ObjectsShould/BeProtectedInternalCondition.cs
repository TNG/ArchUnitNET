using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBeProtectedInternalTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types().That().Are(t.ProtectedInternalInnerClass).Should().BeProtectedInternal()
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.PublicClass).Should().BeProtectedInternal());
        t.AssertViolations(Types().That().Are(t.PublicInnerClass).Should().BeProtectedInternal());
        t.AssertViolations(
            Types().That().Are(t.ProtectedInnerClass).Should().BeProtectedInternal()
        );
        t.AssertViolations(Types().That().Are(t.InternalClass).Should().BeProtectedInternal());
        t.AssertViolations(Types().That().Are(t.InternalInnerClass).Should().BeProtectedInternal());
        t.AssertViolations(Types().That().Are(t.PrivateInnerClass).Should().BeProtectedInternal());
        t.AssertViolations(
            Types().That().Are(t.PrivateProtectedInnerClass).Should().BeProtectedInternal()
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
                .Are(t.ProtectedInternalInnerClass, t.OtherProtectedInternalInnerClass)
                .Should()
                .BeProtectedInternal()
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ProtectedInternalInnerClass, t.InternalClass)
                .Should()
                .BeProtectedInternal()
        );
        await t.AssertSnapshotMatches();
    }
}
