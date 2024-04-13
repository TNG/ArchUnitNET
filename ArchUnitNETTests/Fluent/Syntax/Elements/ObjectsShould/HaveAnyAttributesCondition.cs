using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldHaveAnyAttributesTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertNoViolations(should.HaveAnyAttributes(t.Attribute1.FullName));
        t.AssertNoViolations(should.HaveAnyAttributes([t.Attribute1.FullName]));
        t.AssertNoViolations(should.HaveAnyAttributes(t.Attribute1));
        t.AssertNoViolations(should.HaveAnyAttributes([t.Attribute1]));
        t.AssertNoViolations(should.HaveAnyAttributes(t.Attribute1SystemType));
        t.AssertNoViolations(should.HaveAnyAttributes([t.Attribute1SystemType]));
        t.AssertNoViolations(should.HaveAnyAttributes(Attributes().That().Are(t.Attribute1)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertViolations(should.HaveAnyAttributes(t.UnusedAttribute.FullName));
        t.AssertViolations(should.HaveAnyAttributes([t.UnusedAttribute.FullName]));
        t.AssertViolations(should.HaveAnyAttributes(t.UnusedAttribute));
        t.AssertViolations(should.HaveAnyAttributes([t.UnusedAttribute]));
        t.AssertViolations(should.HaveAnyAttributes(t.UnusedAttributeSystemType));
        t.AssertViolations(should.HaveAnyAttributes([t.UnusedAttributeSystemType]));
        t.AssertViolations(should.HaveAnyAttributes(Attributes().That().Are(t.UnusedAttribute)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentAttribute()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertViolations(should.HaveAnyAttributes(t.NonExistentObjectName));
        t.AssertViolations(should.HaveAnyAttributes([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertViolations(should.HaveAnyAttributes(new List<string>()));
        t.AssertViolations(should.HaveAnyAttributes(new List<Attribute>()));
        t.AssertViolations(should.HaveAnyAttributes(new List<System.Type>()));
        t.AssertViolations(
            should.HaveAnyAttributes(Attributes().That().Are(t.NonExistentObjectName))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertNoViolations(
            should.HaveAnyAttributes([t.Attribute1.FullName, t.UnusedAttribute.FullName])
        );
        t.AssertNoViolations(should.HaveAnyAttributes(t.Attribute1, t.UnusedAttribute));
        t.AssertNoViolations(should.HaveAnyAttributes([t.Attribute1, t.UnusedAttribute]));
        t.AssertNoViolations(
            should.HaveAnyAttributes(t.Attribute1SystemType, t.UnusedAttributeSystemType)
        );
        t.AssertNoViolations(
            should.HaveAnyAttributes([t.Attribute1SystemType, t.UnusedAttributeSystemType])
        );
        t.AssertNoViolations(
            should.HaveAnyAttributes(Attributes().That().Are(t.Attribute1, t.UnusedAttribute))
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
                .Are(t.ClassWithAttributes, t.OtherClassWithAttributes)
                .Should()
                .HaveAnyAttributes(t.Attribute1)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ClassWithAttributes, t.ClassWithoutAttributes)
                .Should()
                .HaveAnyAttributes(t.Attribute1)
        );
        await t.AssertSnapshotMatches();
    }
}
