using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNETTests.AssemblyTestHelper;
using Mono.Cecil.Cil;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class GivenObjectsThatTests
{
    [Fact]
    public async Task AreTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().Are(helper.ChildClass),
            Types().That().Are([helper.ChildClass]),
            Types().That().Are(Types().That().Are(helper.ChildClass)),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass).AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates = [Types().That().Are(new List<IType> { })];
        predicates.ForEach(types =>
            types
                .Should()
                .Be(new List<IType> { })
                .WithoutRequiringPositiveResults()
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().Are(helper.ChildClass, helper.BaseClass),
            Types().That().Are([helper.ChildClass, helper.BaseClass]),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.BaseClass, helper.ChildClass).AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task CallAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        var predicates = new List<GivenMethodMembersConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            MethodMembers().That().CallAny(helper.CalledMethod),
            MethodMembers().That().CallAny([helper.CalledMethod]),
            MethodMembers().That().CallAny(MethodMembers().That().Are(helper.CalledMethod)),
        ];
        predicates.ForEach(methods =>
            methods.Should().Be(helper.MethodWithSingleDependency).AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(methods =>
            methods.Should().Be(helper.MethodWithoutDependencies).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates = [MethodMembers().That().CallAny(new List<MethodMember> { })];
        predicates.ForEach(methods =>
            methods
                .Should()
                .Be(helper.MethodWithoutDependencies)
                .WithoutRequiringPositiveResults()
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            MethodMembers().That().CallAny(helper.CalledMethod, helper.CalledMethod1),
            MethodMembers().That().CallAny([helper.CalledMethod, helper.CalledMethod1]),
        ];
        predicates.ForEach(methods =>
            methods
                .Should()
                .Be(helper.MethodWithSingleDependency, helper.MethodWithMultipleDependencies)
                .AssertNoViolations(helper)
        );
        predicates.ForEach(methods =>
            methods.Should().Be(helper.MethodWithoutDependencies).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task DependOnAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().DependOnAny(helper.BaseClass),
            Types().That().DependOnAny(helper.BaseClassSystemType),
            Types().That().DependOnAny([helper.BaseClass]),
            Types().That().DependOnAny([helper.BaseClassSystemType]),
            Types().That().DependOnAny(Types().That().Are(helper.BaseClass)),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass, helper.OtherChildClass).AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates =
        [
            Types().That().DependOnAny(new List<IType> { }),
            Types().That().DependOnAny(new List<Type> { }),
        ];
        predicates.ForEach(types =>
            types
                .Should()
                .Be(new List<IType> { })
                .WithoutRequiringPositiveResults()
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().DependOnAny(helper.BaseClass, helper.OtherBaseClass),
            Types().That().DependOnAny(helper.BaseClassSystemType, helper.OtherBaseClassSystemType),
            Types().That().DependOnAny([helper.BaseClass, helper.OtherBaseClass]),
            Types()
                .That()
                .DependOnAny([helper.BaseClassSystemType, helper.OtherBaseClassSystemType]),
            Types().That().DependOnAny(Types().That().Are(helper.BaseClass, helper.OtherBaseClass)),
        ];
        predicates.ForEach(types =>
            types
                .Should()
                .Be(helper.ChildClass, helper.OtherChildClass, helper.ClassWithMultipleDependencies)
                .AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass1).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyDependOnTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().DependOnAny(helper.BaseClass).And().OnlyDependOn(helper.BaseClass),
            Types()
                .That()
                .DependOnAny(helper.BaseClass)
                .And()
                .OnlyDependOn(helper.BaseClassSystemType),
            Types().That().DependOnAny(helper.BaseClass).And().OnlyDependOn([helper.BaseClass]),
            Types()
                .That()
                .DependOnAny(helper.BaseClass)
                .And()
                .OnlyDependOn(helper.BaseClassSystemType),
            Types()
                .That()
                .DependOnAny(helper.BaseClass)
                .And()
                .OnlyDependOn(Types().That().Are(helper.BaseClass)),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass, helper.OtherChildClass).AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates =
        [
            Types().That().OnlyDependOn(new List<IType> { }).And().Are(helper.BaseClass),
            Types().That().OnlyDependOn(new List<Type> { }).And().Are(helper.BaseClass),
        ];
        predicates.ForEach(types => types.Should().Be(helper.BaseClass).AssertNoViolations(helper));

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().OnlyDependOn(helper.BaseClassWithMember, helper.OtherBaseClass),
            Types()
                .That()
                .OnlyDependOn(
                    helper.BaseClassWithMemberSystemType,
                    helper.OtherBaseClassSystemType
                ),
            Types().That().OnlyDependOn([helper.BaseClassWithMember, helper.OtherBaseClass]),
            Types()
                .That()
                .OnlyDependOn(
                    [helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType]
                ),
            Types()
                .That()
                .OnlyDependOn(
                    Types()
                        .That()
                        .Are(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType)
                ),
        ];
        predicates.ForEach(types =>
            types
                .And()
                .Are(
                    helper.ClassWithoutDependencies,
                    helper.BaseClass,
                    helper.ChildClassWithMember,
                    helper.ClassWithMultipleDependencies,
                    helper.ChildClass
                )
                .Should()
                .Be(
                    helper.ClassWithoutDependencies,
                    helper.BaseClass,
                    helper.ChildClassWithMember,
                    helper.ClassWithMultipleDependencies
                )
                .AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass1).AssertOnlyViolations(helper)
        );
        predicates =
        [
            Types().That().OnlyDependOn(helper.BaseClassWithMember, helper.OtherBaseClass),
            Types()
                .That()
                .OnlyDependOn(
                    helper.BaseClassWithMemberSystemType,
                    helper.OtherBaseClassSystemType
                ),
            Types().That().OnlyDependOn([helper.BaseClassWithMember, helper.OtherBaseClass]),
            Types()
                .That()
                .OnlyDependOn(
                    [helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType]
                ),
            Types()
                .That()
                .OnlyDependOn(
                    Types()
                        .That()
                        .Are(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType)
                ),
        ];
        predicates.ForEach(types =>
            types
                .Should()
                .NotBe(helper.ChildClass, helper.OtherChildClass)
                .AssertNoViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    // TODO
    // [Fact]
    // public async Task FollowCustomPredicateTest()
    // {
    //
    // }

    [Fact]
    public async Task HaveAnyAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().HaveAnyAttributes(helper.OnceUsedAttribute),
            Types().That().HaveAnyAttributes(helper.OnceUsedAttributeSystemType),
            Types().That().HaveAnyAttributes([helper.OnceUsedAttribute]),
            Types().That().HaveAnyAttributes([helper.OnceUsedAttributeSystemType]),
            Types().That().HaveAnyAttributes(Attributes().That().Are(helper.OnceUsedAttribute)),
        ];
        predicates.ForEach(types => types.Should().Be(helper.ClassWithSingleUniquelyUsedAttribute));

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutAttributes).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates =
        [
            Types()
                .That()
                .HaveAnyAttributes(Attributes().That().HaveName(helper.NonExistentObjectName)),
            Types().That().HaveAnyAttributes(new List<Attribute> { }),
            Types().That().HaveAnyAttributes(new List<Type> { }),
        ];
        predicates.ForEach(types =>
            types
                .Should()
                .Be(new List<IType> { })
                .WithoutRequiringPositiveResults()
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().HaveAnyAttributes(helper.Attribute1, helper.Attribute2),
            Types()
                .That()
                .HaveAnyAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType),
            Types().That().HaveAnyAttributes([helper.Attribute1, helper.Attribute2]),
            Types()
                .That()
                .HaveAnyAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]),
            Types()
                .That()
                .HaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)),
        ];
        predicates.ForEach(types =>
            types
                .And()
                .Are(
                    helper.ClassWithoutAttributes,
                    helper.ClassWithSingleAttribute,
                    helper.ClassWithTwoAttributes,
                    helper.ClassWithThreeAttributes
                )
                .Should()
                .Be(
                    helper.ClassWithSingleAttribute,
                    helper.ClassWithTwoAttributes,
                    helper.ClassWithThreeAttributes
                )
                .AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutAttributes).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyHaveAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().OnlyHaveAttributes(helper.OnceUsedAttribute),
            Types().That().OnlyHaveAttributes(helper.OnceUsedAttributeSystemType),
            Types().That().OnlyHaveAttributes([helper.OnceUsedAttribute]),
            Types().That().OnlyHaveAttributes([helper.OnceUsedAttributeSystemType]),
            Types().That().OnlyHaveAttributes(Attributes().That().Are(helper.OnceUsedAttribute)),
        ];
        predicates.ForEach(types => types.Should().Be(helper.ClassWithSingleUniquelyUsedAttribute));

        helper.AddSnapshotHeader("Violations");
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithSingleAttribute).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates =
        [
            Types()
                .That()
                .OnlyHaveAttributes(Attributes().That().HaveName(helper.NonExistentObjectName)),
            Types().That().OnlyHaveAttributes(new List<Attribute> { }),
            Types().That().OnlyHaveAttributes(new List<Type> { }),
        ];
        predicates.ForEach(types =>
            types
                .And()
                .Are(helper.ClassWithoutAttributes, helper.ClassWithSingleAttribute)
                .Should()
                .Be([helper.ClassWithoutAttributes])
                .WithoutRequiringPositiveResults()
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().OnlyHaveAttributes(helper.Attribute1, helper.Attribute2),
            Types()
                .That()
                .OnlyHaveAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType),
            Types().That().OnlyHaveAttributes([helper.Attribute1, helper.Attribute2]),
            Types()
                .That()
                .OnlyHaveAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]),
            Types()
                .That()
                .OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)),
        ];
        predicates.ForEach(types =>
            types
                .And()
                .Are(
                    helper.ClassWithoutAttributes,
                    helper.ClassWithSingleAttribute,
                    helper.ClassWithTwoAttributes,
                    helper.ClassWithThreeAttributes
                )
                .Should()
                .Be(
                    helper.ClassWithoutAttributes,
                    helper.ClassWithSingleAttribute,
                    helper.ClassWithTwoAttributes
                )
                .AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithThreeAttributes).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        var predicates = new List<GivenTypesConjunction> { };

        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            Types().That().HaveAnyAttributesWithArguments(helper.Attribute2IntegerArgument),
            Types().That().HaveAnyAttributesWithArguments(helper.Attribute2StringArgument),
            Types().That().HaveAnyAttributesWithArguments(helper.Attribute2TypeArgument),
            Types().That().HaveAnyAttributesWithArguments(helper.Attribute2TypeArgumentSystemType),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithTwoAttributes, helper.ClassWithThreeAttributes)
        );

        await helper.AssertSnapshotMatches();
    }
}
