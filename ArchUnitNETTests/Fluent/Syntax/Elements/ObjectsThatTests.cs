using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNETTests.AssemblyTestHelper;
using Mono.Cecil.Cil;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsThatTests
{
    [Fact]
    public async Task AreTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        var predicates = new List<GivenTypesConjunction> { };
        var that = Types().That();
        helper.AddSnapshotHeader("No violations");
        predicates =
        [
            that.Are(helper.ChildClass.FullName),
            that.Are("^.*ChildClass$", true),
            that.Are([helper.ChildClass.FullName]),
            that.Are(["^.*ChildClass$"], true),
            that.Are(helper.ChildClass),
            that.Are([helper.ChildClass]),
            that.Are(Types().That().Are(helper.ChildClass)),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass).AssertNoViolations(helper)
        );

        helper.AddSnapshotHeader("Violations");
        predicates =
        [
            that.Are(helper.ChildClass.FullName),
            that.Are("^.*ChildClass$", true),
            that.Are([helper.ChildClass.FullName]),
            that.Are(["^.*ChildClass$"], true),
            that.Are(helper.ChildClass),
            that.Are([helper.ChildClass]),
            that.Are(Types().That().Are(helper.ChildClass)),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Non-existent type");
        predicates =
        [
            that.Are(helper.NonExistentObjectName),
            that.Are([helper.NonExistentObjectName]),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.NonExistentObjectName).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Empty arguments");
        predicates = [that.Are(new string[] { }), that.Are(new List<IType> { })];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass).AssertOnlyViolations(helper)
        );

        helper.AddSnapshotHeader("Multiple arguments");
        predicates =
        [
            that.Are([helper.ChildClass.FullName, helper.BaseClass.FullName]),
            that.Are(helper.ChildClass, helper.BaseClass),
            that.Are([helper.ChildClass, helper.BaseClass]),
        ];
        predicates.ForEach(types =>
            types.Should().Be(helper.ChildClass).AssertAnyViolations(helper)
        );
        await helper.AssertSnapshotMatches();
    }
}
