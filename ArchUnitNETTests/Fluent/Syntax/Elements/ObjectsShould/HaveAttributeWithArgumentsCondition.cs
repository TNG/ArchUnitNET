using System.Collections.Generic;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldHaveAttributeWithArgumentsTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1.FullName, t.Attribute1Argument1Value)
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1.FullName, [t.Attribute1Argument1Value])
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1, t.Attribute1Argument1Value)
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1, [t.Attribute1Argument1Value])
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1SystemType, t.Attribute1Argument1Value)
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1SystemType, [t.Attribute1Argument1Value])
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1.FullName, t.Attribute2Argument2Value)
        );
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1.FullName, [t.Attribute2Argument2Value])
        );
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1, t.Attribute2Argument2Value)
        );
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1, [t.Attribute2Argument2Value])
        );
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1SystemType, t.Attribute2Argument2Value)
        );
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.Attribute1SystemType, [t.Attribute2Argument2Value])
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentAttribute()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertViolations(
            should.HaveAttributeWithArguments(t.NonExistentObjectName, t.Attribute1Argument1Value)
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1.FullName, new List<object>())
        );
        t.AssertNoViolations(should.HaveAttributeWithArguments(t.Attribute1, new List<object>()));
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(t.Attribute1SystemType, new List<object>())
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithPositionalArguments).Should();
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(
                t.Attribute1.FullName,
                [t.Attribute1Argument1Value, t.Attribute1Argument2Value]
            )
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(
                t.Attribute1,
                t.Attribute1Argument1Value,
                t.Attribute1Argument2Value
            )
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(
                t.Attribute1,
                [t.Attribute1Argument1Value, t.Attribute1Argument2Value]
            )
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(
                t.Attribute1SystemType,
                t.Attribute1Argument1Value,
                t.Attribute1Argument2Value
            )
        );
        t.AssertNoViolations(
            should.HaveAttributeWithArguments(
                t.Attribute1SystemType,
                [t.Attribute1Argument1Value, t.Attribute1Argument2Value]
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
                .HaveAttributeWithArguments(t.Attribute1, t.Attribute1Argument1Value)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ClassWithPositionalArguments, t.ClassWithAttributes)
                .Should()
                .HaveAttributeWithArguments(t.Attribute1, t.Attribute1Argument2Value)
        );
        await t.AssertSnapshotMatches();
    }
}
