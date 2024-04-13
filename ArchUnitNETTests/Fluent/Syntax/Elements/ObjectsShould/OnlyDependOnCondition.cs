using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldOnlyDependOnTests
{
    [Fact]
    public async void NoViolations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();
        t.AssertNoViolations(should.OnlyDependOn(t.BaseClass.FullName));
        t.AssertNoViolations(should.OnlyDependOn([t.BaseClass.FullName]));
        t.AssertNoViolations(should.OnlyDependOn(t.BaseClass));
        t.AssertNoViolations(should.OnlyDependOn(t.BaseClassSystemType));
        t.AssertNoViolations(should.OnlyDependOn(Classes().That().Are(t.BaseClass)));
        t.AssertNoViolations(should.OnlyDependOn([t.BaseClass]));
        t.AssertNoViolations(should.OnlyDependOn([t.BaseClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void Violations()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithMultipleDependencies).Should();
        t.AssertViolations(should.OnlyDependOn(t.BaseClass.FullName));
        t.AssertViolations(should.OnlyDependOn([t.BaseClass.FullName]));
        t.AssertViolations(should.OnlyDependOn(t.BaseClass));
        t.AssertViolations(should.OnlyDependOn(t.BaseClassSystemType));
        t.AssertViolations(should.OnlyDependOn(Classes().That().Are(t.BaseClass)));
        t.AssertViolations(should.OnlyDependOn([t.BaseClass]));
        t.AssertViolations(should.OnlyDependOn([t.BaseClassSystemType]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void NonExistentType()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithMultipleDependencies).Should();
        t.AssertViolations(should.OnlyDependOn(t.NonExistentObjectName));
        t.AssertViolations(should.OnlyDependOn([t.NonExistentObjectName]));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void EmptyArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithMultipleDependencies).Should();
        t.AssertViolations(should.OnlyDependOn(new List<string>()));
        t.AssertViolations(should.OnlyDependOn(new List<IType>()));
        t.AssertViolations(should.OnlyDependOn(new List<System.Type>()));
        t.AssertViolations(should.OnlyDependOn(Classes().That().Are(t.NonExistentObjectName)));
        await t.AssertSnapshotMatches();
    }

    [Fact]
    public async void MultipleArguments()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ClassWithMultipleDependencies).Should();
        t.AssertViolations(should.OnlyDependOn([t.BaseClass.FullName, t.OtherBaseClass.FullName]));
        t.AssertViolations(should.OnlyDependOn(t.BaseClass, t.OtherBaseClass));
        t.AssertViolations(should.OnlyDependOn([t.BaseClass, t.OtherBaseClass]));
        t.AssertViolations(should.OnlyDependOn(t.BaseClassSystemType, t.OtherBaseClassSystemType));
        t.AssertViolations(
            should.OnlyDependOn([t.BaseClassSystemType, t.OtherBaseClassSystemType])
        );
        t.AssertViolations(
            should.OnlyDependOn(Classes().That().Are(t.BaseClass, t.OtherBaseClass))
        );
        await t.AssertSnapshotMatches();
    }
}
