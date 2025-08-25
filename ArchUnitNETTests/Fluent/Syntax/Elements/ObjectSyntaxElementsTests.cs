using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

// csharpier-ignore
public class ObjectSyntaxElementsTests
{
    [Fact]
    public async Task AreTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        // Single argument
        var predicates = new List<GivenTypesConjunction> {
            Types().That().Are(helper.ChildClass),
            Types().That().Are(helper.ChildClassSystemType),
            Types().That().Are(Classes().That().Are(helper.ChildClass)),
            Types().That().Are([helper.ChildClass]),
            Types().That().Are([helper.ChildClassSystemType]),
        };
        foreach (var predicate in predicates)
        {
            Assert.Equal([.. predicate.GetObjects(helper.Architecture)], new List<IType> { helper.ChildClass });
        }

        // Multiple arguments
        predicates = new List<GivenTypesConjunction> {
            Types().That().Are(helper.BaseClass, helper.ChildClass),
            Types().That().Are(helper.BaseClassSystemType, helper.ChildClassSystemType),
            Types().That().Are(Classes().That().Are(helper.BaseClass, helper.ChildClass)),
            Types().That().Are([helper.BaseClass, helper.ChildClass]),
            Types().That().Are([helper.BaseClassSystemType, helper.ChildClassSystemType]),
        };
        foreach (var predicate in predicates)
        {
            Assert.Equal([.. predicate.GetObjects(helper.Architecture)], new List<IType> { helper.BaseClass, helper.ChildClass });
        }

        // Empty arguments
        Assert.Equal([.. Types().That().Are(new List<IType>()).GetObjects(helper.Architecture)], new List<IType>());
        Assert.Equal([.. Types().That().Are([]).GetObjects(helper.Architecture)], new List<IType>());

        // Empty inputs
        Types().That().Are([]).Should().BeTypesThat().Are(helper.ChildClass).AssertOnlyViolations(helper);

        // Predicates as conditions
        var should = Types().That().Are(helper.ChildClass).Should();
        should.BeTypesThat().Are(helper.ChildClass).AssertNoViolations(helper);
        should.BeTypesThat().Are(helper.ChildClassSystemType).AssertNoViolations(helper);
        should.BeTypesThat().Are(Classes().That().Are(helper.ChildClass)).AssertNoViolations(helper);
        should.BeTypesThat().Are([helper.ChildClass]).AssertNoViolations(helper);
        should.BeTypesThat().Are([helper.ChildClassSystemType]).AssertNoViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.Be(helper.ChildClass).AssertNoViolations(helper);
        should.Be(helper.ChildClassSystemType).AssertNoViolations(helper);
        should.Be(Classes().That().Are(helper.ChildClass)).AssertNoViolations(helper);
        should.Be([helper.ChildClass]).AssertNoViolations(helper);
        should.Be([helper.ChildClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ChildClass).Should();
        should.Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.Be(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should.Be(Classes().That().Are(helper.ClassWithoutDependencies)).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependenciesSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.BaseClass).Should();
        should.Be(new List<IType>()).AssertOnlyViolations(helper);
        should.Be(new List<System.Type>()).AssertOnlyViolations(helper);
        should.Be(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ChildClass).Should();
        should.Be(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should.Be(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.ChildClass, helper.BaseClass).Should().Be(helper.ChildClass, helper.BaseClass).AssertNoViolations(helper);
        Types().That().Are(helper.ChildClass, helper.BaseClass).Should().Be(helper.ChildClass, helper.ClassWithoutDependencies).AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.BeTypesThat().Are(helper.ChildClass).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();
        should.BeTypesThat().Are(helper.ChildClass).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeInternalTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.InternalClass).Should().BeInternal().AssertNoViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BeInternal().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateInnerClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BeInternal().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.InternalClass, helper.OtherInternalClass).Should().BeInternal().AssertNoViolations(helper);
        Types().That().Are(helper.InternalClass, helper.ProtectedInternalInnerClass).Should().BeInternal().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BePrivateTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.PrivateInnerClass).Should().BePrivate().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BePrivate().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.PrivateInnerClass, helper.OtherPrivateInnerClass).Should().BePrivate().AssertNoViolations(helper);
        Types().That().Are(helper.PrivateInnerClass, helper.PrivateProtectedInnerClass).Should().BePrivate().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BePrivateProtectedTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BePrivateProtected().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BePrivateProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateInnerClass).Should().BePrivateProtected().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.PrivateProtectedInnerClass, helper.OtherPrivateProtectedInnerClass).Should().BePrivateProtected().AssertNoViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass, helper.PrivateInnerClass).Should().BePrivateProtected().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeProtectedTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.ProtectedInnerClass).Should().BeProtected().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateInnerClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BeProtected().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.ProtectedInnerClass, helper.OtherProtectedInnerClass).Should().BeProtected().AssertNoViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass, helper.ProtectedInternalInnerClass).Should().BeProtected().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeProtectedInternalTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BeProtectedInternal().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateInnerClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BeProtectedInternal().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.ProtectedInternalInnerClass, helper.OtherProtectedInternalInnerClass).Should().BeProtectedInternal().AssertNoViolations(helper);

        Types().That().Are(helper.ProtectedInternalInnerClass, helper.InternalClass).Should().BeProtectedInternal().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BePublicTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.PublicClass).Should().BePublic().AssertNoViolations(helper);
        Types().That().Are(helper.PublicInnerClass).Should().BePublic().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.InternalClass).Should().BePublic().AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalInnerClass).Should().BePublic().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInnerClass).Should().BePublic().AssertOnlyViolations(helper);
        Types().That().Are(helper.ProtectedInternalInnerClass).Should().BePublic().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateInnerClass).Should().BePublic().AssertOnlyViolations(helper);
        Types().That().Are(helper.PrivateProtectedInnerClass).Should().BePublic().AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types().That().Are(helper.PublicClass, helper.OtherPublicClass).Should().BePublic().AssertNoViolations(helper);
        Types().That().Are(helper.PublicClass, helper.InternalClass).Should().BePublic().AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task CallAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        
        helper.AddSnapshotSubHeader("Conditions");
        should.CallAny(helper.CalledMethod).AssertNoViolations(helper);
        should.CallAny([helper.CalledMethod]).AssertNoViolations(helper);
        should.CallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().CallAny(helper.CalledMethod)).AssertNoViolations(helper);
        should.Be(MethodMembers().That().CallAny([helper.CalledMethod])).AssertNoViolations(helper);
        should.Be(MethodMembers().That().CallAny(MethodMembers().That().Are(helper.CalledMethod))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().CallAny(helper.CalledMethod).AssertNoViolations(helper);
        should.BeMethodMembersThat().CallAny([helper.CalledMethod]).AssertNoViolations(helper);
        should.BeMethodMembersThat().CallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = MethodMembers().That().Are(helper.MethodWithoutDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.CallAny(helper.CalledMethod).AssertOnlyViolations(helper);
        should.CallAny([helper.CalledMethod]).AssertOnlyViolations(helper);
        should.CallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().CallAny(helper.CalledMethod)).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().CallAny([helper.CalledMethod])).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().CallAny(MethodMembers().That().Are(helper.CalledMethod))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().CallAny(helper.CalledMethod).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().CallAny([helper.CalledMethod]).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().CallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.CallAny(new List<MethodMember>()).AssertOnlyViolations(helper);
        should.CallAny(MethodMembers().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().CallAny(new List<MethodMember>())).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().CallAny(MethodMembers().That().HaveFullName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().CallAny(new List<MethodMember>()).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().CallAny(MethodMembers().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = MethodMembers().That().Are(helper.MethodWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.CallAny(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies).AssertOnlyViolations(helper);
        should.CallAny([helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies]).AssertOnlyViolations(helper);
        should.CallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().CallAny(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies)).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().CallAny([helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies])).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().CallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().CallAny(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().CallAny([helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies]).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().CallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input with multiple dependencies");
        should = MethodMembers().That().Are(helper.MethodWithMultipleDependencies).Should();
        
        helper.AddSnapshotSubHeader("Conditions");
        should.CallAny(helper.CalledMethod1, helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.CallAny(helper.MethodWithoutDependencies).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().CallAny(helper.CalledMethod1, helper.MethodWithoutDependencies)).AssertNoViolations(helper);
        should.Be(MethodMembers().That().CallAny(helper.MethodWithoutDependencies)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().CallAny(helper.CalledMethod1, helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.BeMethodMembersThat().CallAny(helper.MethodWithoutDependencies).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task DependOnAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny(helper.BaseClass).AssertNoViolations(helper);
        should.DependOnAny(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.DependOnAny(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.DependOnAny([helper.BaseClass]).AssertNoViolations(helper);
        should.DependOnAny([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny(helper.BaseClass)).AssertNoViolations(helper);
        should.Be(Types().That().DependOnAny(helper.BaseClassSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DependOnAny(Classes().That().Are(helper.BaseClass))).AssertNoViolations(helper);
        should.Be(Types().That().DependOnAny([helper.BaseClass])).AssertNoViolations(helper);
        should.Be(Types().That().DependOnAny([helper.BaseClassSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny(helper.BaseClass).AssertNoViolations(helper);
        should.BeTypesThat().DependOnAny(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DependOnAny(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.BeTypesThat().DependOnAny([helper.BaseClass]).AssertNoViolations(helper);
        should.BeTypesThat().DependOnAny([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().HaveFullName(helper.ClassWithMultipleDependencies.FullName).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should.DependOnAny(Classes().That().Are(helper.ClassWithoutDependencies)).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependenciesSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny(helper.ClassWithoutDependencies)).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny(helper.ClassWithoutDependenciesSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny(Classes().That().Are(helper.ClassWithoutDependencies))).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny([helper.ClassWithoutDependencies])).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny([helper.ClassWithoutDependenciesSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny(Classes().That().Are(helper.ClassWithoutDependencies)).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny([helper.ClassWithoutDependenciesSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertException<TypeDoesNotExistInArchitecture>(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny(new List<IType>()).AssertOnlyViolations(helper);
        should.DependOnAny(new List<System.Type>()).AssertOnlyViolations(helper);
        should.DependOnAny(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny(new List<IType>())).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny(new List<System.Type>())).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny(Classes().That().HaveFullName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny(new List<IType>()).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny(new List<System.Type>()).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny(helper.ClassWithoutDependencies, helper.BaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny([helper.ClassWithoutDependencies, helper.BaseClass])).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input without dependencies");
        should = Types().That().Are(helper.ClassWithoutDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.DependOnAny([helper.BaseClass, helper.ChildClass]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DependOnAny([helper.BaseClass, helper.ChildClass])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DependOnAny([helper.BaseClass, helper.ChildClass]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");

        helper.AddSnapshotSubHeader("Conditions");
        Types().That().Are(helper.ChildClass1, helper.ChildClass2).Should().DependOnAny(helper.BaseClassWithMultipleDependenciesSystemType).AssertNoViolations(helper);
        Types().That().Are(helper.ChildClass, helper.BaseClass).Should().DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        Types().That().Are(helper.ChildClass1, helper.ChildClass2).Should().Be(Types().That().DependOnAny(helper.BaseClassWithMultipleDependenciesSystemType)).AssertNoViolations(helper);
        Types().That().Are(helper.ChildClass, helper.BaseClass).Should().Be(Types().That().DependOnAny(helper.ClassWithoutDependencies)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        Types().That().Are(helper.ChildClass1, helper.ChildClass2).Should().BeTypesThat().DependOnAny(helper.BaseClassWithMultipleDependenciesSystemType).AssertNoViolations(helper);
        Types().That().Are(helper.ChildClass, helper.BaseClass).Should().BeTypesThat().DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task DependOnAnyTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.ChildClass).Should().DependOnAnyTypesThat().Are(helper.BaseClass).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.BaseClass).Should().DependOnAnyTypesThat().Are(helper.ChildClass).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task ExistTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.BaseClass).Should().Exist().AssertNoViolations(helper);
        Types().That().Are(helper.BaseClassSystemType).Should().Exist().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().HaveFullName(helper.NonExistentObjectName).Should().Exist().AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    class CustomCondition : ICondition<IType>
    {
        public string Description => "follow custom condition";

        public IEnumerable<ConditionResult> Check(
            IEnumerable<IType> objects,
            Architecture architecture
        )
        {
            return objects.Select(t => new ConditionResult(
                t,
                t.Name == "ChildClass",
                "does not follow custom condition"
            ));
        }

        public bool CheckEmpty()
        {
            return true;
        }
    }

    [Fact]
    public async Task FollowCustomConditionTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.FollowCustomCondition(new CustomCondition()).AssertNoViolations(helper);
        should.FollowCustomCondition(t => new ConditionResult(t, t.Name == "ChildClass", "does not follow custom condition"), "follow custom condition").AssertNoViolations(helper);
        should.FollowCustomCondition(t => t.Name == "ChildClass", "follow custom condition", "does not follow custom condition").AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();
        should.FollowCustomCondition(new CustomCondition()).AssertOnlyViolations(helper);
        should.FollowCustomCondition(t => new ConditionResult(t, t.Name == "ChildClass", "does not follow custom condition"), "follow custom condition").AssertOnlyViolations(helper);
        should.FollowCustomCondition(t => t.Name == "ChildClass", "follow custom condition", "does not follow custom condition").AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    class CustomPredicate : IPredicate<IType>
    {
        public string Description => "follow custom predicate";

        public IEnumerable<IType> GetMatchingObjects(IEnumerable<IType> objects, Architecture architecture)
        {
            return objects.Where(t => t.Name == "ChildClass");
        }
    }

    [Fact]
    public async Task FollowCustomPredicateTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        helper.AddSnapshotSubHeader("Conditions");
        should.Be(Types().That().FollowCustomPredicate(new CustomPredicate())).AssertNoViolations(helper);
        should.Be(Types().That().FollowCustomPredicate(t => t.Name == "ChildClass", "follow custom predicate")).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.BeTypesThat().FollowCustomPredicate(new CustomPredicate()).AssertNoViolations(helper);
        should.BeTypesThat().FollowCustomPredicate(t => t.Name == "ChildClass", "follow custom predicate").AssertNoViolations(helper);
        
        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.Be(Types().That().FollowCustomPredicate(new CustomPredicate())).AssertOnlyViolations(helper);
        should.Be(Types().That().FollowCustomPredicate(t => t.Name == "ChildClass", "follow custom predicate")).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.BeTypesThat().FollowCustomPredicate(new CustomPredicate()).AssertOnlyViolations(helper);
        should.BeTypesThat().FollowCustomPredicate(t => t.Name == "ChildClass", "follow custom predicate").AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.HaveAnyAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should.HaveAnyAttributes(Attributes().That().Are(helper.Attribute1)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributes(helper.Attribute1)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.Attribute1])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(helper.Attribute1SystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.Attribute1SystemType])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(Attributes().That().Are(helper.Attribute1))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(Attributes().That().Are(helper.Attribute1)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributes(helper.UnusedAttribute)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.UnusedAttribute])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(helper.UnusedAttributeSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.UnusedAttributeSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributes(new List<Attribute>())).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(new List<System.Type>())).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributes(helper.Attribute1, helper.UnusedAttribute).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1, helper.UnusedAttribute]).AssertNoViolations(helper);
        should.HaveAnyAttributes(helper.Attribute1SystemType, helper.UnusedAttributeSystemType).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1SystemType, helper.UnusedAttributeSystemType]).AssertNoViolations(helper);
        should.HaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.UnusedAttribute)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributes(helper.Attribute1, helper.UnusedAttribute)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.Attribute1, helper.UnusedAttribute])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(helper.Attribute1SystemType, helper.UnusedAttributeSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes([helper.Attribute1SystemType, helper.UnusedAttributeSystemType])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.UnusedAttribute))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributes(helper.Attribute1, helper.UnusedAttribute).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.Attribute1, helper.UnusedAttribute]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(helper.Attribute1SystemType, helper.UnusedAttributeSystemType).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes([helper.Attribute1SystemType, helper.UnusedAttributeSystemType]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.UnusedAttribute)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");

        helper.AddSnapshotSubHeader("Conditions");
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithThreeAttributes).Should().HaveAnyAttributes(helper.Attribute1).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithoutAttributes).Should().HaveAnyAttributes(helper.Attribute1).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithThreeAttributes).Should().Be(Types().That().HaveAnyAttributes(helper.Attribute1)).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithoutAttributes).Should().Be(Types().That().HaveAnyAttributes(helper.Attribute1)).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithThreeAttributes).Should().BeTypesThat().HaveAnyAttributes(helper.Attribute1).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributes, helper.ClassWithoutAttributes).Should().BeTypesThat().HaveAnyAttributes(helper.Attribute1).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesThatTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithTwoAttributes).Should();
        should.HaveAnyAttributesThat().Are(helper.Attribute1).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();
        should.HaveAnyAttributesThat().Are(helper.UnusedAttribute).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.HaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments(null).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments(null)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments(null).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");

        helper.AddSnapshotSubHeader("Conditions");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should().HaveAnyAttributesWithArguments([]).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributesWithArguments).Should().HaveAnyAttributesWithArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should().Be(Types().That().HaveAnyAttributesWithArguments([])).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributesWithArguments).Should().Be(Types().That().HaveAnyAttributesWithArguments([])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should().BeTypesThat().HaveAnyAttributesWithArguments([]).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithTwoAttributesWithArguments).Should().BeTypesThat().HaveAnyAttributesWithArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithArguments([helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue]).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithArguments(helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithArguments([helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithArguments(helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithArguments([helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithArguments(helper.UnusedAttributeIntValue, helper.UnusedAttributeStringValue).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");

        helper.AddSnapshotSubHeader("Conditions");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().HaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().HaveAnyAttributesWithArguments(helper.Attribute2IntegerArgument).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().Be(Types().That().HaveAnyAttributesWithArguments(helper.Attribute1StringArgument)).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().Be(Types().That().HaveAnyAttributesWithArguments(helper.Attribute2IntegerArgument)).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().BeTypesThat().HaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertNoViolations(helper);
        Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should().BeTypesThat().HaveAnyAttributesWithArguments(helper.Attribute2IntegerArgument).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesWithNamedArguments()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations for attribute without named arguments");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertNoViolations(helper);
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue),]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument),])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue),])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.UnusedAttributeStringValue),]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments, helper.ClassWithTwoAttributesWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.HaveAnyAttributesWithNamedArguments([("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAnyAttributesWithNamedArguments([("NamedParameter3", helper.Attribute2TypeArgumentSystemType)])).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAnyAttributesWithNamedArguments([("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAttributeWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument]).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations for wrong attribute");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1TypeArgumentSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1TypeArgumentSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, [helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, [helper.UnusedTypeArgumentSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.UnusedTypeArgumentSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, [helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.UnusedTypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.UnusedTypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument]).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass), helper.Attribute1StringArgument).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass), helper.Attribute1StringArgument)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass), helper.Attribute1StringArgument).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, null).AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, null).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, null)).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, null)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, null).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, null).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, new List<object>()).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, new List<object>()).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, new List<object>())).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, new List<object>())).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, new List<object>()).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, new List<object>()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument]).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument, helper.Attribute1IntegerArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument, helper.Attribute1IntegerArgument]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute2, helper.Attribute2IntegerArgument).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithArguments(helper.Attribute2, helper.Attribute2IntegerArgument)).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithArguments(helper.Attribute2, helper.Attribute2IntegerArgument).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAttributeWithNamedArguments()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);
        
        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations for attribute without named arguments");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.UnusedTypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.UnusedTypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.UnusedAttributeStringValue))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.UnusedAttributeStringValue)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("InvalidName", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("InvalidName", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.UnusedAttributeStringValue)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, []).AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, []).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [])).AssertNoViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, []).AssertNoViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, []).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),])).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments, helper.ClassWithTwoAttributesWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveAttributeWithNamedArguments(helper.Attribute2, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType)).AssertAnyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute2, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute2SystemType, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType)).AssertAnyViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute2SystemType, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute2, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType))).AssertAnyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute2, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)])).AssertAnyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute2SystemType, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType))).AssertAnyViolations(helper);
        should.Be(Types().That().HaveAttributeWithNamedArguments(helper.Attribute2SystemType, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)])).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute2, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType)).AssertAnyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute2, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute2SystemType, ("NamedParameter3", helper.Attribute2TypeArgumentSystemType)).AssertAnyViolations(helper);
        should.BeTypesThat().HaveAttributeWithNamedArguments(helper.Attribute2SystemType, [("NamedParameter3", helper.Attribute2TypeArgumentSystemType)]).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveNameTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.HaveNameMatching("^Base.*$").AssertNoViolations(helper);
        should.HaveFullName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.HaveFullNameMatching("^.*\\.Base.*$").AssertNoViolations(helper);
        should.HaveNameContaining("Base").AssertNoViolations(helper);
        should.HaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.HaveNameStartingWith("Base").AssertNoViolations(helper);
        should.HaveNameEndingWith("Class").AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveName(helper.BaseClass.Name)).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameMatching("^Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullName(helper.BaseClass.FullName)).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullNameMatching("^.*\\.Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameContaining("Base")).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullNameContaining(helper.BaseClass.Namespace.Name)).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameStartingWith("Base")).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameEndingWith("Class")).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.BeTypesThat().HaveNameMatching("^Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().HaveFullName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.BeTypesThat().HaveFullNameMatching("^.*\\.Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().HaveNameContaining("Base").AssertNoViolations(helper);
        should.BeTypesThat().HaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.BeTypesThat().HaveNameStartingWith("Base").AssertNoViolations(helper);
        should.BeTypesThat().HaveNameEndingWith("Class").AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.HaveName(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.HaveName("^.*\\.Base.*$").AssertOnlyViolations(helper);
        should.HaveFullName(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.HaveFullName("^Base.*$").AssertOnlyViolations(helper);
        should.HaveNameContaining(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.HaveFullNameContaining(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.HaveNameStartingWith(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.HaveNameEndingWith("Base").AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().HaveName(helper.BaseClass.Name)).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameMatching("^Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullName(helper.BaseClass.FullName)).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullNameMatching("^.*\\.Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameContaining("Base")).AssertNoViolations(helper);
        should.Be(Types().That().HaveFullNameContaining(helper.BaseClass.Namespace.Name)).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameStartingWith("Base")).AssertNoViolations(helper);
        should.Be(Types().That().HaveNameEndingWith("Class")).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().HaveName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.BeTypesThat().HaveNameMatching("^Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().HaveFullName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.BeTypesThat().HaveFullNameMatching("^.*\\.Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().HaveNameContaining("Base").AssertNoViolations(helper);
        should.BeTypesThat().HaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.BeTypesThat().HaveNameStartingWith("Base").AssertNoViolations(helper);
        should.BeTypesThat().HaveNameEndingWith("Class").AssertNoViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().DependOnAny(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBe(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.NotBe(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should.NotBe(Classes().That().Are(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependenciesSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().AreNot(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.Be(Types().That().AreNot(helper.ClassWithoutDependenciesSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().AreNot(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.Be(Types().That().AreNot([helper.ClassWithoutDependencies])).AssertNoViolations(helper);
        should.Be(Types().That().AreNot([helper.ClassWithoutDependenciesSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().AreNot(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.BeTypesThat().AreNot(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should.BeTypesThat().AreNot(Classes().That().Are(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.BeTypesThat().AreNot([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should.BeTypesThat().AreNot([helper.ClassWithoutDependenciesSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().DependOnAny(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBe(helper.ChildClass).AssertAnyViolations(helper);
        should.NotBe(helper.ChildClassSystemType).AssertAnyViolations(helper);
        should.NotBe(Classes().That().Are(helper.ChildClass)).AssertAnyViolations(helper);
        should.NotBe([helper.ChildClass]).AssertAnyViolations(helper);
        should.NotBe([helper.ChildClassSystemType]).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().AreNot(helper.ChildClass)).AssertAnyViolations(helper);
        should.Be(Types().That().AreNot(helper.ChildClassSystemType)).AssertAnyViolations(helper);
        should.Be(Types().That().AreNot(Classes().That().Are(helper.ChildClass))).AssertAnyViolations(helper);
        should.Be(Types().That().AreNot([helper.ChildClass])).AssertAnyViolations(helper);
        should.Be(Types().That().AreNot([helper.ChildClassSystemType])).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().AreNot(helper.ChildClass).AssertAnyViolations(helper);
        should.BeTypesThat().AreNot(helper.ChildClassSystemType).AssertAnyViolations(helper);
        should.BeTypesThat().AreNot(Classes().That().Are(helper.ChildClass)).AssertAnyViolations(helper);
        should.BeTypesThat().AreNot([helper.ChildClass]).AssertAnyViolations(helper);
        should.BeTypesThat().AreNot([helper.ChildClassSystemType]).AssertAnyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().DependOnAny(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBe(new List<IType>()).AssertNoViolations(helper);
        should.NotBe(new List<System.Type>()).AssertNoViolations(helper);
        should.NotBe(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().AreNot(new List<IType>())).AssertNoViolations(helper);
        should.Be(Types().That().AreNot(new List<System.Type>())).AssertNoViolations(helper);
        should.Be(Types().That().AreNot(Classes().That().HaveFullName(helper.NonExistentObjectName))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().AreNot(new List<IType>()).AssertNoViolations(helper);
        should.BeTypesThat().AreNot(new List<System.Type>()).AssertNoViolations(helper);
        should.BeTypesThat().AreNot(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().DependOnAny(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBe(helper.ClassWithoutDependencies, helper.BaseClass).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependencies, helper.BaseClass]).AssertNoViolations(helper);
        should.NotBe(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().AreNot(helper.ClassWithoutDependencies, helper.BaseClass)).AssertNoViolations(helper);
        should.Be(Types().That().AreNot([helper.ClassWithoutDependencies, helper.BaseClass])).AssertNoViolations(helper);
        should.Be(Types().That().AreNot(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().AreNot([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().AreNot(helper.ClassWithoutDependencies, helper.BaseClass).AssertNoViolations(helper);
        should.BeTypesThat().AreNot([helper.ClassWithoutDependencies, helper.BaseClass]).AssertNoViolations(helper);
        should.BeTypesThat().AreNot(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertNoViolations(helper);
        should.BeTypesThat().AreNot([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertNoViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotCallAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotCallAny(helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.NotCallAny([helper.MethodWithoutDependencies]).AssertNoViolations(helper);
        should.NotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().DoNotCallAny(helper.MethodWithoutDependencies)).AssertNoViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny([helper.MethodWithoutDependencies])).AssertNoViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().DoNotCallAny(helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny([helper.MethodWithoutDependencies]).AssertNoViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotCallAny(helper.CalledMethod).AssertOnlyViolations(helper);
        should.NotCallAny([helper.CalledMethod]).AssertOnlyViolations(helper);
        should.NotCallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().DoNotCallAny(helper.CalledMethod)).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny([helper.CalledMethod])).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny(MethodMembers().That().Are(helper.CalledMethod))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().DoNotCallAny(helper.CalledMethod).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny([helper.CalledMethod]).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotCallAny(new List<MethodMember>()).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().DoNotCallAny(new List<MethodMember>())).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().DoNotCallAny(new List<MethodMember>()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = MethodMembers().That().Are(helper.MethodWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotCallAny(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2).AssertOnlyViolations(helper);
        should.NotCallAny([helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2]).AssertOnlyViolations(helper);
        should.NotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().DoNotCallAny(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2)).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny([helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2])).AssertOnlyViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().DoNotCallAny(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny([helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2]).AssertOnlyViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency, helper.MethodWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotCallAny(helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.NotCallAny(helper.CalledMethod, helper.CalledMethod1, helper.CalledMethod2).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(MethodMembers().That().DoNotCallAny(helper.MethodWithoutDependencies)).AssertNoViolations(helper);
        should.Be(MethodMembers().That().DoNotCallAny(helper.CalledMethod, helper.CalledMethod1, helper.CalledMethod2)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeMethodMembersThat().DoNotCallAny(helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.BeMethodMembersThat().DoNotCallAny(helper.CalledMethod, helper.CalledMethod1, helper.CalledMethod2).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotDependOnAnyTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.BaseClass).Should().NotDependOnAnyTypesThat().Are(helper.ChildClass).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.ChildClass).Should().NotDependOnAnyTypesThat().Are(helper.BaseClass).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotDependOnAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.NotDependOnAny(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should.NotDependOnAny(Classes().That().Are(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.NotDependOnAny([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should.NotDependOnAny([helper.ClassWithoutDependenciesSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(helper.ClassWithoutDependenciesSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.ClassWithoutDependencies])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.ClassWithoutDependenciesSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(Classes().That().Are(helper.ClassWithoutDependencies)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.ClassWithoutDependenciesSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(helper.BaseClass).AssertOnlyViolations(helper);
        should.NotDependOnAny(helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.NotDependOnAny(Classes().That().Are(helper.BaseClass)).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClass]).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(helper.BaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(helper.BaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(Classes().That().Are(helper.BaseClass))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(helper.BaseClass).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(Classes().That().Are(helper.BaseClass)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.BaseClass]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertException<TypeDoesNotExistInArchitecture>(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(new List<IType>()).AssertNoViolations(helper);
        should.NotDependOnAny(new List<System.Type>()).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(new List<IType>())).AssertNoViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(new List<System.Type>())).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(new List<IType>()).AssertNoViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(new List<System.Type>()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should.NotDependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(helper.ClassWithoutDependencies, helper.BaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.ClassWithoutDependencies, helper.BaseClass])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input with multiple dependencies");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotDependOnAny(helper.BaseClassWithMember, helper.OtherBaseClass).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClassWithMember, helper.OtherBaseClass]).AssertOnlyViolations(helper);
        should.NotDependOnAny(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType]).AssertOnlyViolations(helper);
        should.NotDependOnAny(Classes().That().Are(helper.BaseClassWithMember, helper.OtherBaseClass)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotDependOnAny(helper.BaseClassWithMember, helper.OtherBaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.BaseClassWithMember, helper.OtherBaseClass])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotDependOnAny([helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType])).AssertOnlyViolations(helper);
        should.Be(Classes().That().DoNotDependOnAny(helper.BaseClassWithMember, helper.OtherBaseClass)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotDependOnAny(helper.BaseClassWithMember, helper.OtherBaseClass).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.BaseClassWithMember, helper.OtherBaseClass]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny([helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotDependOnAny(Classes().That().Are(helper.BaseClassWithMember, helper.OtherBaseClass)).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotExistTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().DependOnAny(helper.ChildClass).Should();
        should.NotExist().AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().DependOnAny(helper.BaseClass).Should();
        should.NotExist().AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAnyAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(helper.UnusedAttribute).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.UnusedAttribute]).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(helper.UnusedAttributeSystemType).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.UnusedAttribute)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.UnusedAttribute])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.UnusedAttributeSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.UnusedAttributeSystemType])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.UnusedAttribute).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.UnusedAttribute]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.UnusedAttributeSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(helper.Attribute1).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(helper.Attribute1SystemType).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1SystemType]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute1)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.Attribute1])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute1SystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.Attribute1SystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute1).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.Attribute1]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute1SystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.Attribute1SystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(typeof(TypeDependencyNamespace.BaseClass)).AssertException<TypeDoesNotExistInArchitecture>(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(typeof(TypeDependencyNamespace.BaseClass))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(typeof(TypeDependencyNamespace.BaseClass)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(new List<Attribute>()).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(new List<System.Type>()).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(new List<Attribute>())).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(new List<System.Type>())).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(new List<Attribute>()).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(new List<System.Type>()).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(helper.Attribute1, helper.Attribute2).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1, helper.Attribute2]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute1, helper.Attribute2)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.Attribute1, helper.Attribute2])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute1, helper.Attribute2).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.Attribute1, helper.Attribute2]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithoutAttributes, helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributes(helper.Attribute2).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(helper.Attribute1).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute2)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributes(helper.Attribute1)).AssertAnyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute2).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributes(helper.Attribute1).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAnyAttributesThatTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.NotHaveAnyAttributesThat().Are(helper.UnusedAttribute).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.NotHaveAnyAttributesThat().Are(helper.Attribute1).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAnyAttributesWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.UnusedTypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.UnusedTypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.UnusedAttributeStringValue).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithArguments([helper.UnusedAttributeStringValue]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.UnusedAttributeStringValue)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.UnusedAttributeStringValue])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.UnusedAttributeStringValue).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.UnusedAttributeStringValue]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type without attributes");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(null).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(null)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(null).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments([helper.UnusedTypeArgument, helper.Attribute1StringArgument]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithArguments(helper.UnusedTypeArgument, helper.Attribute1StringArgument).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.UnusedTypeArgument, helper.Attribute1StringArgument])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.UnusedTypeArgument, helper.Attribute1StringArgument)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.UnusedTypeArgument, helper.Attribute1StringArgument]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.UnusedTypeArgument, helper.Attribute1StringArgument).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithArguments([helper.Attribute1StringArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithArguments([helper.Attribute1StringArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments(helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithArguments([helper.Attribute1StringArgument]).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAnyAttributesWithNamedArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.UnusedTypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.UnusedTypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)]).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue)).AssertNoViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("InvalidName", helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("InvalidName", helper.Attribute1StringArgument)]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.UnusedAttributeStringValue)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.UnusedAttributeStringValue)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgument), ("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments, helper.ClassWithTwoAttributesWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments(("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAnyAttributesWithNamedArguments([("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAttributeWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2TypeArgumentSystemType).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2TypeArgumentSystemType]).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2TypeArgumentSystemType).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2TypeArgumentSystemType])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2TypeArgumentSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2TypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2TypeArgumentSystemType]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2TypeArgumentSystemType).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2TypeArgumentSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument]).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2StringArgument]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute2StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute2StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1StringArgument]).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1StringArgument])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1StringArgument)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1StringArgument])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, [helper.Attribute1StringArgument]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, helper.Attribute1StringArgument).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, [helper.Attribute1StringArgument]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass),1).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass),1)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass),1).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.UnusedAttribute, null).AssertNoViolations(helper);
        should.NotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, null).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, null)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, null)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttribute, null).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, null).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();
        should.NotHaveAttributeWithArguments(helper.Attribute1, []).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, []).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1TypeArgumentSystemType, helper.Attribute1IntegerArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithArguments, helper.ClassWithTwoAttributesWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1StringArgument]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, helper.Attribute1StringArgument).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithArguments(helper.Attribute1SystemType, [helper.Attribute1StringArgument]).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAttributeWithNamedArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();

        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute2TypeArgumentSystemType)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute2TypeArgumentSystemType)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute2StringArgument)]).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute2StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute2StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute2StringArgument)])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute2StringArgument))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute2StringArgument)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute2StringArgument)]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute2StringArgument)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute2StringArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)])).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument))).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttribute, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttribute, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.UnusedAttributeSystemType, [("NamedParameter1", helper.Attribute1TypeArgument)]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(typeof(TypeDependencyNamespace.BaseClass), ("NamedParameter1", helper.Attribute1TypeArgument)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, []).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, []).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, []).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, []).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument),]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType), ("NamedParameter2", helper.Attribute1StringArgument)]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttributeWithNamedArguments, helper.ClassWithTwoAttributesWithNamedArguments).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType))).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)])).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, ("NamedParameter1", helper.Attribute1TypeArgumentSystemType)).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [("NamedParameter1", helper.Attribute1TypeArgumentSystemType)]).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveNameTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.NotHaveNameMatching("^.*\\.Base.*$").AssertNoViolations(helper);
        should.NotHaveFullName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.NotHaveFullNameMatching("^Base.*$").AssertNoViolations(helper);
        should.NotHaveNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.NotHaveFullNameContaining(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotHaveNameStartingWith(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.NotHaveNameEndingWith("Test").AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveName(helper.BaseClass.FullName)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveNameMatching("^.*\\.Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveFullName(helper.BaseClass.Name)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveFullNameMatching("^Base.*$")).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveNameContaining(helper.BaseClass.Namespace.Name)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveFullNameContaining(helper.NonExistentObjectName)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveNameStartingWith(helper.BaseClass.Namespace.Name)).AssertNoViolations(helper);
        should.Be(Types().That().DoNotHaveNameEndingWith("Test")).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveNameMatching("^.*\\.Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveFullName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveFullNameMatching("^Base.*$").AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveFullNameContaining(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveNameStartingWith(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.BeTypesThat().DoNotHaveNameEndingWith("Test").AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotHaveName(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.NotHaveNameMatching("^Base.*$").AssertOnlyViolations(helper);
        should.NotHaveFullName(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.NotHaveFullNameMatching("^.*\\.Base.*$").AssertOnlyViolations(helper);
        should.NotHaveNameContaining(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.NotHaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.NotHaveNameStartingWith(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.NotHaveNameEndingWith(helper.BaseClass.Name).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().DoNotHaveName(helper.BaseClass.Name)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveNameMatching("^Base.*$")).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveFullName(helper.BaseClass.FullName)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveFullNameMatching("^.*\\.Base.*$")).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveNameContaining(helper.BaseClass.Name)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveFullNameContaining(helper.BaseClass.Namespace.Name)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveNameStartingWith(helper.BaseClass.Name)).AssertOnlyViolations(helper);
        should.Be(Types().That().DoNotHaveNameEndingWith(helper.BaseClass.Name)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().DoNotHaveName(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveNameMatching("^Base.*$").AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveFullName(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveFullNameMatching("^.*\\.Base.*$").AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveNameContaining(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveNameStartingWith(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.BeTypesThat().DoNotHaveNameEndingWith(helper.BaseClass.Name).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyDependOnTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyDependOn(helper.BaseClass).AssertNoViolations(helper);
        should.OnlyDependOn(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.OnlyDependOn(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.OnlyDependOn([helper.BaseClass]).AssertNoViolations(helper);
        should.OnlyDependOn([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyDependOn(helper.BaseClass)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyDependOn(helper.BaseClassSystemType)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyDependOn(Classes().That().Are(helper.BaseClass))).AssertNoViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClass])).AssertNoViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClassSystemType])).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyDependOn(helper.BaseClass).AssertNoViolations(helper);
        should.BeTypesThat().OnlyDependOn(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.BeTypesThat().OnlyDependOn(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.BeTypesThat().OnlyDependOn([helper.BaseClass]).AssertNoViolations(helper);
        should.BeTypesThat().OnlyDependOn([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyDependOn(helper.BaseClass).AssertOnlyViolations(helper);
        should.OnlyDependOn(helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.OnlyDependOn(Classes().That().Are(helper.BaseClass)).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClass]).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyDependOn(helper.BaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(helper.BaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(Classes().That().Are(helper.BaseClass))).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClass])).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClassSystemType])).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyDependOn(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertException<TypeDoesNotExistInArchitecture>(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyDependOn(typeof(AttributeNamespace.ClassWithoutAttributes))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyDependOn(typeof(AttributeNamespace.ClassWithoutAttributes)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyDependOn(new List<IType>()).AssertOnlyViolations(helper);
        should.OnlyDependOn(new List<System.Type>()).AssertOnlyViolations(helper);
        should.OnlyDependOn(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyDependOn(new List<IType>())).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(new List<System.Type>())).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(Classes().That().HaveFullName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyDependOn(new List<IType>()).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn(new List<System.Type>()).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn(Classes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyDependOn(helper.BaseClass, helper.OtherBaseClass).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClass, helper.OtherBaseClass]).AssertOnlyViolations(helper);
        should.OnlyDependOn(helper.BaseClassSystemType, helper.OtherBaseClassSystemType).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClassSystemType, helper.OtherBaseClassSystemType]).AssertOnlyViolations(helper);
        should.OnlyDependOn(Classes().That().Are(helper.BaseClass, helper.OtherBaseClass)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyDependOn(helper.BaseClass, helper.OtherBaseClass)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClass, helper.OtherBaseClass])).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(helper.BaseClassSystemType, helper.OtherBaseClassSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn([helper.BaseClassSystemType, helper.OtherBaseClassSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyDependOn(Classes().That().Are(helper.BaseClass, helper.OtherBaseClass))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyDependOn(helper.BaseClass, helper.OtherBaseClass).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn([helper.BaseClass, helper.OtherBaseClass]).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn(helper.BaseClassSystemType, helper.OtherBaseClassSystemType).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn([helper.BaseClassSystemType, helper.OtherBaseClassSystemType]).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyDependOn(Classes().That().Are(helper.BaseClass, helper.OtherBaseClass)).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyDependOnTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.OnlyDependOnTypesThat().Are(helper.BaseClass).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should.OnlyDependOnTypesThat().Are(helper.BaseClass).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyHaveAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        
        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute1)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.Attribute1])).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute1SystemType)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.Attribute1SystemType])).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(Attributes().That().Are(helper.UnusedAttribute)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(helper.UnusedAttribute)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.UnusedAttribute])).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(helper.UnusedAttributeSystemType)).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.UnusedAttributeSystemType])).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(Attributes().That().Are(helper.UnusedAttribute))).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Attribute outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(typeof(TypeDependencyNamespace.BaseClass)).AssertException<TypeDoesNotExistInArchitecture>(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(typeof(TypeDependencyNamespace.BaseClass))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyHaveAttributes(typeof(TypeDependencyNamespace.BaseClass)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(new List<Attribute>())).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(new List<System.Type>())).AssertOnlyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName))).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyHaveAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(Attributes().That().HaveFullName(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(helper.Attribute1, helper.Attribute2).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1, helper.Attribute2]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute1, helper.Attribute2)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.Attribute1, helper.Attribute2])).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType)).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType])).AssertNoViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2))).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute1, helper.Attribute2).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes([helper.Attribute1, helper.Attribute2]).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType]).AssertNoViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithSingleAttribute, helper.ClassWithTwoAttributes).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.OnlyHaveAttributes(helper.Attribute1).AssertAnyViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute2).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute1)).AssertAnyViolations(helper);
        should.Be(Types().That().OnlyHaveAttributes(helper.Attribute2)).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates as conditions");
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute1).AssertAnyViolations(helper);
        should.BeTypesThat().OnlyHaveAttributes(helper.Attribute2).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyHaveAttributesThatTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributesThat().Are(helper.Attribute1).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributesThat().Are(helper.UnusedAttribute).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public void VisibilityTest()
    {
        var visibilityRules = new List<IArchRule>
        {
            Types().That().ArePrivate().Should().BePrivate(),
            Types().That().ArePrivate().Should().BeTypesThat().ArePrivate(),
            Types().That().ArePublic().Should().BePublic(),
            Types().That().ArePublic().Should().BeTypesThat().ArePublic(), 
            Types().That().AreProtected().Should().BeProtected(), 
            Types().That().AreProtected().Should().BeTypesThat().AreProtected(),
            Types().That().AreInternal().Should().BeInternal(),
            Types().That().AreInternal().Should().BeTypesThat().AreInternal(),
            Types().That().AreProtectedInternal().Should().BeProtectedInternal(),
            Types().That().AreProtectedInternal().Should().BeTypesThat().AreProtectedInternal(),
            Types().That().ArePrivateProtected().Should().BePrivateProtected(),
            Types().That().ArePrivateProtected().Should().BeTypesThat().ArePrivateProtected(),
            Types().That().AreNotPrivate().Should().NotBePrivate(),
            Types().That().AreNotPrivate().Should().BeTypesThat().AreNotPrivate(),
            Types().That().AreNotPublic().Should().NotBePublic(),
            Types().That().AreNotPublic().Should().BeTypesThat().AreNotPublic(),
            Types().That().AreNotProtected().Should().NotBeProtected(),
            Types().That().AreNotProtected().Should().BeTypesThat().AreNotProtected(),
            Types().That().AreNotInternal().Should().NotBeInternal(),
            Types().That().AreNotInternal().Should().BeTypesThat().AreNotInternal(),
            Types().That().AreNotProtectedInternal().Should().NotBeProtectedInternal(),
            Types().That().AreNotProtectedInternal().Should().BeTypesThat().AreNotProtectedInternal(),
            Types().That().AreNotPrivateProtected().Should().NotBePrivateProtected(),
            Types().That().AreNotPrivateProtected().Should().BeTypesThat().AreNotPrivateProtected(),
            Types().That().ArePrivate().Should().NotBePublic().AndShould().NotBeProtected().AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould().NotBePrivateProtected(),
            Types().That().ArePublic().Should().NotBePrivate().AndShould().NotBeProtected().AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould().NotBePrivateProtected(),
            Types().That().AreProtected().Should().NotBePublic().AndShould().NotBePrivate().AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould().NotBePrivateProtected(),
            Types().That().AreInternal().Should().NotBePublic().AndShould().NotBeProtected().AndShould().NotBePrivate().AndShould().NotBeProtectedInternal().AndShould().NotBePrivateProtected(),
            Types().That().AreProtectedInternal().Should().NotBePublic().AndShould().NotBeProtected().AndShould().NotBeInternal().AndShould().NotBePrivate().AndShould().NotBePrivateProtected(),
            Types().That().ArePrivateProtected().Should().NotBePublic().AndShould().NotBeProtected().AndShould().NotBeInternal().AndShould().NotBeProtectedInternal().AndShould().NotBePrivate(),
            Types().That().AreNotPrivate().Should().BePublic().OrShould().BeProtected().OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould().BePrivateProtected(),
            Types().That().AreNotPublic().Should().BePrivate().OrShould().BeProtected().OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould().BePrivateProtected(),
            Types().That().AreNotProtected().Should().BePublic().OrShould().BePrivate().OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould().BePrivateProtected(),
            Types().That().AreNotInternal().Should().BePublic().OrShould().BeProtected().OrShould().BePrivate().OrShould().BeProtectedInternal().OrShould().BePrivateProtected(),
            Types().That().AreNotProtectedInternal().Should().BePublic().OrShould().BeProtected().OrShould().BeInternal().OrShould().BePrivate().OrShould().BePrivateProtected(),
            Types().That().AreNotPrivateProtected().Should().BePublic().OrShould().BeProtected().OrShould().BeInternal().OrShould().BeProtectedInternal().OrShould().BePrivate(),
        };

        foreach (var visibilityRule in visibilityRules)
        {
            Assert.True(
                visibilityRule.HasNoViolations(StaticTestArchitectures.VisibilityArchitecture)
            );
        }
    }
}
