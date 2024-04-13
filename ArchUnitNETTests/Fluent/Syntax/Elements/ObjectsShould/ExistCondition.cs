using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldExistTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(Types().That().Are(t.BaseClass).Should().Exist());
        t.AssertNoViolations(Types().That().Are(t.BaseClassSystemType).Should().Exist());
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertViolations(Types().That().Are(t.NonExistentObjectName).Should().Exist());
        await t.AssertSnapshotMatches();
    }
}
