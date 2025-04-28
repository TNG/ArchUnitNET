using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNETTests.AssemblyTestHelper;
using Mono.Cecil.Cil;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

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
            Types().That().DependOnAny([helper.BaseClass]),
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
        predicates = [Types().That().DependOnAny(new List<IType> { })];
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
            Types().That().DependOnAny([helper.BaseClass, helper.OtherBaseClass]),
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
            Types().That().DependOnAny(helper.BaseClass).And().OnlyDependOn([helper.BaseClass]),
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
        predicates = [Types().That().OnlyDependOn(new List<IType> { })];
        predicates.ForEach(types =>
            types
                .Should()
                .Be(
                    helper.BaseClass,
                    helper.BaseClassWithMember,
                    helper.BaseClassWithMultipleDependencies,
                    helper.OtherBaseClass,
                    helper.GenericBaseClass,
                    helper.ClassWithoutDependencies,
                    helper.OtherClassWithoutDependencies
                )
                .AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            Types().That().OnlyDependOn(helper.BaseClassWithMember, helper.OtherBaseClass),
            Types().That().OnlyDependOn([helper.BaseClassWithMember, helper.OtherBaseClass]),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithMultipleDependencies).AssertNoViolations(helper)
        );
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass1).AssertOnlyViolations(helper)
        );

        await helper.AssertSnapshotMatches();
    }
}
