using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldNotBeTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertNoViolations(should.NotBe(t.ClassWithoutDependencies.FullName));
        t.AssertNoViolations(should.NotBe([t.ClassWithoutDependencies.FullName]));
        t.AssertNoViolations(should.NotBe(t.ClassWithoutDependencies));
        t.AssertNoViolations(should.NotBe(t.ClassWithoutDependenciesSystemType));
        t.AssertNoViolations(should.NotBe(Classes().That().Are(t.ClassWithoutDependencies)));
        t.AssertNoViolations(should.NotBe([t.ClassWithoutDependencies]));
        t.AssertNoViolations(should.NotBe([t.ClassWithoutDependenciesSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertViolations(should.NotBe(t.ChildClass.FullName));
        t.AssertViolations(should.NotBe([t.ChildClass.FullName]));
        t.AssertViolations(should.NotBe(t.ChildClass));
        t.AssertViolations(should.NotBe(t.ChildClassSystemType));
        t.AssertViolations(should.NotBe(Classes().That().Are(t.ChildClass)));
        t.AssertViolations(should.NotBe([t.ChildClass]));
        t.AssertViolations(should.NotBe([t.ChildClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentType()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertNoViolations(should.NotBe(t.NonExistentObjectName));
        t.AssertNoViolations(should.NotBe([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertNoViolations(should.NotBe(new List<string>()));
        t.AssertNoViolations(should.NotBe(new List<IType>()));
        t.AssertNoViolations(should.NotBe(new List<System.Type>()));
        t.AssertNoViolations(should.NotBe(Classes().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertNoViolations(
            should.NotBe([t.ClassWithoutDependencies.FullName, t.BaseClass.FullName])
        );
        t.AssertNoViolations(should.NotBe(t.ClassWithoutDependencies, t.BaseClass));
        t.AssertNoViolations(should.NotBe([t.ClassWithoutDependencies, t.BaseClass]));
        t.AssertNoViolations(
            should.NotBe(t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType)
        );
        t.AssertNoViolations(
            should.NotBe([t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType])
        );
        await t.AssertSnapshotMatches();
    }
}
