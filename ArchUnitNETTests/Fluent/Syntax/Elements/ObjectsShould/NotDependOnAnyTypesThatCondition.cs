using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldNotDependOnAnyTypesThatTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(
            Types().That().Are(t.BaseClass).Should().NotDependOnAnyTypesThat().Are(t.ChildClass)
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertViolations(
            Types().That().Are(t.ChildClass).Should().NotDependOnAnyTypesThat().Are(t.BaseClass)
        );
        await t.AssertSnapshotMatches();
    }
}
