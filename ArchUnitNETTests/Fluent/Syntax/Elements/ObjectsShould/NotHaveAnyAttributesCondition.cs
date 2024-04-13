using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldNotHaveAnyAttributesTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertNoViolations(should.NotHaveAnyAttributes(t.UnusedAttribute.FullName));
        t.AssertNoViolations(should.NotHaveAnyAttributes([t.UnusedAttribute.FullName]));
        t.AssertNoViolations(should.NotHaveAnyAttributes(t.UnusedAttribute));
        t.AssertNoViolations(should.NotHaveAnyAttributes([t.UnusedAttribute]));
        t.AssertNoViolations(should.NotHaveAnyAttributes(t.UnusedAttributeSystemType));
        t.AssertNoViolations(should.NotHaveAnyAttributes([t.UnusedAttributeSystemType]));
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(Attributes().That().Are(t.UnusedAttribute))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertViolations(should.NotHaveAnyAttributes(t.Attribute1.FullName));
        t.AssertViolations(should.NotHaveAnyAttributes([t.Attribute1.FullName]));
        t.AssertViolations(should.NotHaveAnyAttributes(t.Attribute1));
        t.AssertViolations(should.NotHaveAnyAttributes([t.Attribute1]));
        t.AssertViolations(should.NotHaveAnyAttributes(t.Attribute1SystemType));
        t.AssertViolations(should.NotHaveAnyAttributes([t.Attribute1SystemType]));
        t.AssertViolations(should.NotHaveAnyAttributes(Attributes().That().Are(t.Attribute1)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentAttribute()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithoutAttributes).Should();
        t.AssertNoViolations(should.NotHaveAnyAttributes(t.NonExistentObjectName));
        t.AssertNoViolations(should.NotHaveAnyAttributes([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithoutAttributes).Should();
        t.AssertNoViolations(should.NotHaveAnyAttributes(new List<string>()));
        t.AssertNoViolations(should.NotHaveAnyAttributes(new List<Attribute>()));
        t.AssertNoViolations(should.NotHaveAnyAttributes(new List<System.Type>()));
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(Attributes().That().Are(t.NonExistentObjectName))
        );
        should = Types().That().Are(t.NonExistentObjectName).Should();
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(new List<string>()).WithoutRequiringPositiveResults()
        );
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(new List<Attribute>()).WithoutRequiringPositiveResults()
        );
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(new List<System.Type>()).WithoutRequiringPositiveResults()
        );
        t.AssertNoViolations(
            should
                .NotHaveAnyAttributes(Attributes().That().Are(t.NonExistentObjectName))
                .WithoutRequiringPositiveResults()
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertNoViolations(
            should.NotHaveAnyAttributes([t.UnusedAttribute.FullName, t.Attribute2.FullName])
        );
        t.AssertNoViolations(should.NotHaveAnyAttributes(t.UnusedAttribute, t.Attribute2));
        t.AssertNoViolations(should.NotHaveAnyAttributes([t.UnusedAttribute, t.Attribute2]));
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(t.UnusedAttributeSystemType, t.Attribute2SystemType)
        );
        t.AssertNoViolations(
            should.NotHaveAnyAttributes([t.UnusedAttributeSystemType, t.Attribute2SystemType])
        );
        t.AssertNoViolations(
            should.NotHaveAnyAttributes(Attributes().That().Are(t.UnusedAttribute, t.Attribute2))
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
                .Are(t.ClassWithoutAttributes, t.OtherClassWithoutAttributes)
                .Should()
                .NotHaveAnyAttributes(t.Attribute2)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ClassWithoutAttributes, t.OtherClassWithAttributes)
                .Should()
                .NotHaveAnyAttributes(t.Attribute1)
        );
        await t.AssertSnapshotMatches();
    }
}
