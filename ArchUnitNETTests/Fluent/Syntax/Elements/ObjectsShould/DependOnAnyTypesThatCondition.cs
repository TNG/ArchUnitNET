using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldDependOnAnyTypesThatTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(
            Types().That().Are(t.ChildClass).Should().DependOnAnyTypesThat().Are(t.BaseClass)
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertViolations(
            Types().That().Are(t.BaseClass).Should().DependOnAnyTypesThat().Are(t.ChildClass)
        );
        await t.AssertSnapshotMatches();
    }
}
