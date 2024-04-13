using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBePublicTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(Types().That().Are(t.PublicClass).Should().BePublic());
        t.AssertNoViolations(Types().That().Are(t.PublicInnerClass).Should().BePublic());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.InternalClass).Should().BePublic());
        t.AssertViolations(Types().That().Are(t.InternalInnerClass).Should().BePublic());
        t.AssertViolations(Types().That().Are(t.ProtectedInnerClass).Should().BePublic());
        t.AssertViolations(Types().That().Are(t.ProtectedInternalInnerClass).Should().BePublic());
        t.AssertViolations(Types().That().Are(t.PrivateInnerClass).Should().BePublic());
        t.AssertViolations(Types().That().Are(t.PrivateProtectedInnerClass).Should().BePublic());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types().That().Are(t.PublicClass, t.OtherPublicClass).Should().BePublic()
        );
        t.AssertViolations(Types().That().Are(t.PublicClass, t.InternalClass).Should().BePublic());
        await t.AssertSnapshotMatches();
    }
}
