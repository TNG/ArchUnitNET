using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldBeTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.Be(t.ChildClass.FullName));
        t.AssertNoViolations(should.Be([t.ChildClass.FullName]));
        t.AssertNoViolations(should.Be(t.ChildClass));
        t.AssertNoViolations(should.Be(t.ChildClassSystemType));
        t.AssertNoViolations(should.Be(Classes().That().Are(t.ChildClass)));
        t.AssertNoViolations(should.Be([t.ChildClass]));
        t.AssertNoViolations(should.Be([t.ChildClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertViolations(should.Be(t.ClassWithoutDependencies.FullName));
        t.AssertViolations(should.Be([t.ClassWithoutDependencies.FullName]));
        t.AssertViolations(should.Be(t.ClassWithoutDependencies));
        t.AssertViolations(should.Be(t.ClassWithoutDependenciesSystemType));
        t.AssertViolations(should.Be(Classes().That().Are(t.ClassWithoutDependencies)));
        t.AssertViolations(should.Be([t.ClassWithoutDependencies]));
        t.AssertViolations(should.Be([t.ClassWithoutDependenciesSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentType()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.BaseClass).Should();
        t.AssertViolations(should.Be(t.NonExistentObjectName));
        t.AssertViolations(should.Be([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.BaseClass).Should();
        t.AssertViolations(should.Be(new List<string>()));
        t.AssertViolations(should.Be(new List<IType>()));
        t.AssertViolations(should.Be(new List<System.Type>()));
        t.AssertViolations(should.Be(Classes().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertViolations(should.Be([t.ClassWithoutDependencies.FullName, t.BaseClass.FullName]));
        t.AssertViolations(should.Be(t.ClassWithoutDependencies, t.BaseClass));
        t.AssertViolations(should.Be([t.ClassWithoutDependencies, t.BaseClass]));
        t.AssertViolations(should.Be(t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType));
        t.AssertViolations(
            should.Be([t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType])
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(
            Types().That().Are(t.ChildClass, t.BaseClass).Should().Be(t.ChildClass, t.BaseClass)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ChildClass, t.BaseClass)
                .Should()
                .Be(t.ChildClass, t.ClassWithoutDependencies)
        );
        await t.AssertSnapshotMatches();
    }
}
