using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldCallAnyTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(should.CallAny(t.CalledMethod.FullName));
        t.AssertNoViolations(should.CallAny([t.CalledMethod.FullName]));
        t.AssertNoViolations(should.CallAny(t.CalledMethod));
        t.AssertNoViolations(should.CallAny([t.CalledMethod]));
        t.AssertNoViolations(should.CallAny(MethodMembers().That().Are(t.CalledMethod)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertViolations(should.CallAny(t.MethodWithoutDependencies.FullName));
        t.AssertViolations(should.CallAny([t.MethodWithoutDependencies.FullName]));
        t.AssertViolations(should.CallAny(t.MethodWithoutDependencies));
        t.AssertViolations(should.CallAny([t.MethodWithoutDependencies]));
        t.AssertViolations(should.CallAny(MethodMembers().That().Are(t.MethodWithoutDependencies)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentMethodMember()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertViolations(should.CallAny(t.NonExistentObjectName));
        t.AssertViolations(should.CallAny([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertViolations(should.CallAny(new List<string>()));
        t.AssertViolations(should.CallAny(new List<MethodMember>()));
        t.AssertViolations(should.CallAny(MethodMembers().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(
            should.CallAny([t.MethodWithoutDependencies.FullName, t.CalledMethod.FullName])
        );
        t.AssertNoViolations(should.CallAny(t.MethodWithoutDependencies, t.CalledMethod));
        t.AssertNoViolations(should.CallAny([t.MethodWithoutDependencies, t.CalledMethod]));
        t.AssertNoViolations(
            should.CallAny(MethodMembers().That().Are(t.MethodWithoutDependencies, t.CalledMethod))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(
            MethodMembers()
                .That()
                .Are(t.MethodWithMultipleDependencies)
                .Should()
                .CallAny(t.CalledMethod1, t.MethodWithoutDependencies)
        );
        t.AssertViolations(
            MethodMembers()
                .That()
                .Are(t.MethodWithMultipleDependencies)
                .Should()
                .CallAny(t.MethodWithoutDependencies)
        );
        await t.AssertSnapshotMatches();
    }
}
