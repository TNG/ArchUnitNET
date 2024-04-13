using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldNotDependOnAnyTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.NotDependOnAny(t.ClassWithoutDependencies.FullName));
        t.AssertNoViolations(should.NotDependOnAny([t.ClassWithoutDependencies.FullName]));
        t.AssertNoViolations(should.NotDependOnAny(t.ClassWithoutDependencies));
        t.AssertNoViolations(should.NotDependOnAny(t.ClassWithoutDependenciesSystemType));
        t.AssertNoViolations(
            should.NotDependOnAny(Classes().That().Are(t.ClassWithoutDependencies))
        );
        t.AssertNoViolations(should.NotDependOnAny([t.ClassWithoutDependencies]));
        t.AssertNoViolations(should.NotDependOnAny([t.ClassWithoutDependenciesSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertViolations(should.NotDependOnAny(t.BaseClass.FullName));
        t.AssertViolations(should.NotDependOnAny([t.BaseClass.FullName]));
        t.AssertViolations(should.NotDependOnAny(t.BaseClass));
        t.AssertViolations(should.NotDependOnAny(t.BaseClassSystemType));
        t.AssertViolations(should.NotDependOnAny(Classes().That().Are(t.BaseClass)));
        t.AssertViolations(should.NotDependOnAny([t.BaseClass]));
        t.AssertViolations(should.NotDependOnAny([t.BaseClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentType()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.NotDependOnAny(t.NonExistentObjectName));
        t.AssertNoViolations(should.NotDependOnAny([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.NotDependOnAny(new List<string>()));
        t.AssertNoViolations(should.NotDependOnAny(new List<IType>()));
        t.AssertNoViolations(should.NotDependOnAny(new List<System.Type>()));
        t.AssertNoViolations(should.NotDependOnAny(Classes().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertViolations(
            should.NotDependOnAny([t.ClassWithoutDependencies.FullName, t.BaseClass.FullName])
        );
        t.AssertViolations(should.NotDependOnAny(t.ClassWithoutDependencies, t.BaseClass));
        t.AssertViolations(should.NotDependOnAny([t.ClassWithoutDependencies, t.BaseClass]));
        await t.AssertSnapshotMatches();
    }
}
