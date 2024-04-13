using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldHaveAnyAttributesWithArgumentsTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertNoViolations(should.HaveAnyAttributesWithArguments(t.Attribute1Argument1Value));
        t.AssertNoViolations(should.HaveAnyAttributesWithArguments([t.Attribute1Argument1Value]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithoutAttributes).Should();
        t.AssertViolations(should.HaveAnyAttributesWithArguments(t.Attribute1Argument2Value));
        t.AssertViolations(should.HaveAnyAttributesWithArguments([t.Attribute1Argument2Value]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        t.AssertNoViolations(
            Types()
                .That()
                .Are(t.ClassWithPositionalArguments)
                .Should()
                .HaveAnyAttributesWithArguments([])
        );
        t.AssertNoViolations(
            Types().That().Are(t.ClassWithAttributes).Should().HaveAnyAttributesWithArguments([])
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertNoViolations(
            should.HaveAnyAttributesWithArguments(
                [t.Attribute1Argument1Value, t.Attribute1Argument2Value]
            )
        );
        t.AssertNoViolations(
            should.HaveAnyAttributesWithArguments(
                t.Attribute1Argument1Value,
                t.Attribute1Argument2Value
            )
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new AttributeAssemblyTestHelpers();
        t.AssertNoViolations(
            Types()
                .That()
                .Are(t.ClassWithPositionalArguments, t.OtherClassWithPositionalArguments)
                .Should()
                .HaveAnyAttributesWithArguments(t.Attribute1Argument1Value)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ClassWithPositionalArguments, t.ClassWithAttributes)
                .Should()
                .HaveAnyAttributesWithArguments(t.Attribute1Argument2Value)
        );
        await t.AssertSnapshotMatches();
    }
}
