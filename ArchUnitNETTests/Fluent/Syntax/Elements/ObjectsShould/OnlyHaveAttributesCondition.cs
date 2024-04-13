using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldOnlyHaveAttributesTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertNoViolations(should.OnlyHaveAttributes(t.Attribute1.FullName));
        t.AssertNoViolations(should.OnlyHaveAttributes([t.Attribute1.FullName]));
        t.AssertNoViolations(should.OnlyHaveAttributes(t.Attribute1));
        t.AssertNoViolations(should.OnlyHaveAttributes([t.Attribute1]));
        t.AssertNoViolations(should.OnlyHaveAttributes(t.Attribute1SystemType));
        t.AssertNoViolations(should.OnlyHaveAttributes([t.Attribute1SystemType]));
        t.AssertNoViolations(should.OnlyHaveAttributes(Attributes().That().Are(t.Attribute1)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertViolations(should.OnlyHaveAttributes(t.UnusedAttribute.FullName));
        t.AssertViolations(should.OnlyHaveAttributes([t.UnusedAttribute.FullName]));
        t.AssertViolations(should.OnlyHaveAttributes(t.UnusedAttribute));
        t.AssertViolations(should.OnlyHaveAttributes([t.UnusedAttribute]));
        t.AssertViolations(should.OnlyHaveAttributes(t.UnusedAttributeSystemType));
        t.AssertViolations(should.OnlyHaveAttributes([t.UnusedAttributeSystemType]));
        t.AssertViolations(should.OnlyHaveAttributes(Attributes().That().Are(t.UnusedAttribute)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentAttribute()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertViolations(should.OnlyHaveAttributes(t.NonExistentObjectName));
        t.AssertViolations(should.OnlyHaveAttributes([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithSingleAttribute).Should();
        t.AssertViolations(should.OnlyHaveAttributes(new List<string>()));
        t.AssertViolations(should.OnlyHaveAttributes(new List<Attribute>()));
        t.AssertViolations(should.OnlyHaveAttributes(new List<System.Type>()));
        t.AssertViolations(
            should.OnlyHaveAttributes(Attributes().That().Are(t.NonExistentObjectName))
        );
        should = Types().That().Are(t.ClassWithoutAttributes).Should();
        t.AssertNoViolations(should.OnlyHaveAttributes(new List<string>()));
        t.AssertNoViolations(should.OnlyHaveAttributes(new List<Attribute>()));
        t.AssertNoViolations(should.OnlyHaveAttributes(new List<System.Type>()));
        t.AssertNoViolations(
            should.OnlyHaveAttributes(Attributes().That().Are(t.NonExistentObjectName))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new AttributeAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithAttributes).Should();
        t.AssertNoViolations(
            should.OnlyHaveAttributes([t.Attribute1.FullName, t.Attribute2.FullName])
        );
        t.AssertNoViolations(should.OnlyHaveAttributes(t.Attribute1, t.Attribute2));
        t.AssertNoViolations(should.OnlyHaveAttributes([t.Attribute1, t.Attribute2]));
        t.AssertNoViolations(
            should.OnlyHaveAttributes(t.Attribute1SystemType, t.Attribute2SystemType)
        );
        t.AssertNoViolations(
            should.OnlyHaveAttributes([t.Attribute1SystemType, t.Attribute2SystemType])
        );
        t.AssertNoViolations(
            should.OnlyHaveAttributes(Attributes().That().Are(t.Attribute1, t.Attribute2))
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
                .Are(t.ClassWithSingleAttribute, t.OtherClassWithSingleAttribute)
                .Should()
                .OnlyHaveAttributes(t.Attribute1)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ClassWithSingleAttribute, t.ClassWithAttributes)
                .Should()
                .OnlyHaveAttributes(t.Attribute1)
        );
        await t.AssertSnapshotMatches();
    }
}
