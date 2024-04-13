using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldNotCallAnyTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(should.NotCallAny(t.MethodWithoutDependencies.FullName));
        t.AssertNoViolations(should.NotCallAny([t.MethodWithoutDependencies.FullName]));
        t.AssertNoViolations(should.NotCallAny(t.MethodWithoutDependencies));
        t.AssertNoViolations(should.NotCallAny([t.MethodWithoutDependencies]));
        t.AssertNoViolations(
            should.NotCallAny(MethodMembers().That().Are(t.MethodWithoutDependencies))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertViolations(should.NotCallAny(t.CalledMethod.FullName));
        t.AssertViolations(should.NotCallAny([t.CalledMethod.FullName]));
        t.AssertViolations(should.NotCallAny(t.CalledMethod));
        t.AssertViolations(should.NotCallAny([t.CalledMethod]));
        t.AssertViolations(should.NotCallAny(MethodMembers().That().Are(t.CalledMethod)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentMethodMember()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(should.NotCallAny(t.NonExistentObjectName));
        t.AssertNoViolations(should.NotCallAny([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(should.NotCallAny(new List<string>()));
        t.AssertNoViolations(should.NotCallAny(new List<MethodMember>()));
        t.AssertNoViolations(
            should.NotCallAny(MethodMembers().That().Are(t.NonExistentObjectName))
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = MethodMembers().That().Are(t.MethodWithSingleDependency).Should();
        t.AssertNoViolations(
            should.NotCallAny(
                [
                    t.MethodWithoutDependencies.FullName,
                    t.CalledMethod1.FullName,
                    t.CalledMethod2.FullName
                ]
            )
        );
        t.AssertNoViolations(
            should.NotCallAny(t.MethodWithoutDependencies, t.CalledMethod1, t.CalledMethod2)
        );
        t.AssertNoViolations(
            should.NotCallAny([t.MethodWithoutDependencies, t.CalledMethod1, t.CalledMethod2])
        );
        t.AssertNoViolations(
            should.NotCallAny(
                MethodMembers()
                    .That()
                    .Are(t.MethodWithoutDependencies, t.CalledMethod1, t.CalledMethod2)
            )
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
                .Are(t.MethodWithSingleDependency, t.MethodWithMultipleDependencies)
                .Should()
                .NotCallAny(t.MethodWithoutDependencies)
        );
        t.AssertViolations(
            MethodMembers()
                .That()
                .Are(t.MethodWithSingleDependency, t.MethodWithMultipleDependencies)
                .Should()
                .NotCallAny(t.CalledMethod, t.CalledMethod1, t.CalledMethod2)
        );
        await t.AssertSnapshotMatches();
    }
}
