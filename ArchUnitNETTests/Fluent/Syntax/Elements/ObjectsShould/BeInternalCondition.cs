using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBeInternalTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(Types().That().Are(t.InternalClass).Should().BeInternal());
        t.AssertNoViolations(Types().That().Are(t.InternalInnerClass).Should().BeInternal());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertViolations(Types().That().Are(t.PublicClass).Should().BeInternal());
        t.AssertViolations(Types().That().Are(t.PublicInnerClass).Should().BeInternal());
        t.AssertViolations(Types().That().Are(t.ProtectedInnerClass).Should().BeInternal());
        t.AssertViolations(Types().That().Are(t.ProtectedInternalInnerClass).Should().BeInternal());
        t.AssertViolations(Types().That().Are(t.PrivateInnerClass).Should().BeInternal());
        t.AssertViolations(Types().That().Are(t.PrivateProtectedInnerClass).Should().BeInternal());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new VisibilityAssemblyTestHelper();
        t.AssertNoViolations(
            Types().That().Are(t.InternalClass, t.OtherInternalClass).Should().BeInternal()
        );
        t.AssertViolations(
            Types().That().Are(t.InternalClass, t.ProtectedInternalInnerClass).Should().BeInternal()
        );
        await t.AssertSnapshotMatches();
    }
}
