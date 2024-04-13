using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldDependOnAnyTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.DependOnAny(t.BaseClass.FullName));
        t.AssertNoViolations(should.DependOnAny([t.BaseClass.FullName]));
        t.AssertNoViolations(should.DependOnAny(t.BaseClass));
        t.AssertNoViolations(should.DependOnAny(t.BaseClassSystemType));
        t.AssertNoViolations(should.DependOnAny(Classes().That().Are(t.BaseClass)));
        t.AssertNoViolations(should.DependOnAny([t.BaseClass]));
        t.AssertNoViolations(should.DependOnAny([t.BaseClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertViolations(should.DependOnAny(t.ClassWithoutDependencies.FullName));
        t.AssertViolations(should.DependOnAny([t.ClassWithoutDependencies.FullName]));
        t.AssertViolations(should.DependOnAny(t.ClassWithoutDependencies));
        t.AssertViolations(should.DependOnAny(t.ClassWithoutDependenciesSystemType));
        t.AssertViolations(should.DependOnAny(Classes().That().Are(t.ClassWithoutDependencies)));
        t.AssertViolations(should.DependOnAny([t.ClassWithoutDependencies]));
        t.AssertViolations(should.DependOnAny([t.ClassWithoutDependenciesSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentType()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertViolations(should.DependOnAny(t.NonExistentObjectName));
        t.AssertViolations(should.DependOnAny([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertViolations(should.DependOnAny(new List<string>()));
        t.AssertViolations(should.DependOnAny(new List<IType>()));
        t.AssertViolations(should.DependOnAny(new List<System.Type>()));
        t.AssertViolations(should.DependOnAny(Classes().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().DependOnAny(t.BaseClass.FullName).Should();
        t.AssertNoViolations(
            should.DependOnAny([t.ClassWithoutDependencies.FullName, t.BaseClass.FullName])
        );
        t.AssertNoViolations(should.DependOnAny(t.ClassWithoutDependencies, t.BaseClass));
        t.AssertNoViolations(should.DependOnAny([t.ClassWithoutDependencies, t.BaseClass]));
        t.AssertNoViolations(
            should.DependOnAny(t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType)
        );
        t.AssertNoViolations(
            should.DependOnAny([t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType])
        );
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleInputs()
    {
        var t = new DependencyAssemblyTestHelpers();
        t.AssertNoViolations(
            Types()
                .That()
                .Are(t.ChildClass1, t.ChildClass2)
                .Should()
                .DependOnAny(t.BaseClassSystemTypeWithMultipleDependencies)
        );
        t.AssertViolations(
            Types()
                .That()
                .Are(t.ChildClass, t.BaseClass)
                .Should()
                .DependOnAny(t.ClassWithoutDependencies)
        );
        await t.AssertSnapshotMatches();
    }
}
