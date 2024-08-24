using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ObjectsShouldTests
{
    [Fact]
    public async Task BeTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.Be(helper.ChildClass.FullName).AssertNoViolations(helper);
        should.Be("^.*\\.ChildClass$", true).AssertNoViolations(helper);
        should.Be([helper.ChildClass.FullName]).AssertNoViolations(helper);
        should.Be(["^.*\\.ChildClass$"], true).AssertNoViolations(helper);
        should.Be(helper.ChildClass).AssertNoViolations(helper);
        should.Be(helper.ChildClassSystemType).AssertNoViolations(helper);
        should.Be(Classes().That().Are(helper.ChildClass)).AssertNoViolations(helper);
        should.Be([helper.ChildClass]).AssertNoViolations(helper);
        should.Be([helper.ChildClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ChildClass).Should();
        should.Be(helper.ClassWithoutDependencies.FullName).AssertOnlyViolations(helper);
        should.Be("^.*\\.ClassWithoutDependencies$", true).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependencies.FullName]).AssertOnlyViolations(helper);
        should.Be(["^.*\\.ClassWithoutDependencies$"], true).AssertOnlyViolations(helper);
        should.Be(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.Be(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should
            .Be(Classes().That().Are(helper.ClassWithoutDependencies))
            .AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependenciesSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent type");
        should = Types().That().Are(helper.BaseClass).Should();
        should.Be(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.Be([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.BaseClass).Should();
        should.Be(new List<string>()).AssertOnlyViolations(helper);
        should.Be(new List<IType>()).AssertOnlyViolations(helper);
        should.Be(new List<System.Type>()).AssertOnlyViolations(helper);
        should.Be(Classes().That().Are(helper.NonExistentObjectName)).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ChildClass).Should();
        should
            .Be([helper.ClassWithoutDependencies.FullName, helper.BaseClass.FullName])
            .AssertOnlyViolations(helper);
        should.Be(helper.ClassWithoutDependencies, helper.BaseClass).AssertOnlyViolations(helper);
        should.Be([helper.ClassWithoutDependencies, helper.BaseClass]).AssertOnlyViolations(helper);
        should
            .Be(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)
            .AssertOnlyViolations(helper);
        should
            .Be([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ChildClass, helper.BaseClass)
            .Should()
            .Be(helper.ChildClass, helper.BaseClass)
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ChildClass, helper.BaseClass)
            .Should()
            .Be(helper.ChildClass, helper.ClassWithoutDependencies)
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeInternalTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types().That().Are(helper.InternalClass).Should().BeInternal().AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BeInternal()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BeInternal().AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PublicInnerClass)
            .Should()
            .BeInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BeInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BeInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BeInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BeInternal()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.InternalClass, helper.OtherInternalClass)
            .Should()
            .BeInternal()
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.InternalClass, helper.ProtectedInternalInnerClass)
            .Should()
            .BeInternal()
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BePrivateTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BePrivate()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PublicInnerClass)
            .Should()
            .BePrivate()
            .AssertOnlyViolations(helper);
        Types().That().Are(helper.InternalClass).Should().BePrivate().AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BePrivate()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BePrivate()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BePrivate()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BePrivate()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.PrivateInnerClass, helper.OtherPrivateInnerClass)
            .Should()
            .BePrivate()
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass, helper.PrivateProtectedInnerClass)
            .Should()
            .BePrivate()
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BePrivateProtectedTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types()
            .That()
            .Are(helper.PublicClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PublicInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass, helper.OtherPrivateProtectedInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass, helper.PrivateInnerClass)
            .Should()
            .BePrivateProtected()
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeProtectedTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BeProtected()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types().That().Are(helper.PublicClass).Should().BeProtected().AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PublicInnerClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BeProtected()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ProtectedInnerClass, helper.OtherProtectedInnerClass)
            .Should()
            .BeProtected()
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass, helper.ProtectedInternalInnerClass)
            .Should()
            .BeProtected()
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeProtectedInternalTest()
    {
        var helper = new VisibilityAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types()
            .That()
            .Are(helper.PublicClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PublicInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass, helper.OtherProtectedInternalInnerClass)
            .Should()
            .BeProtectedInternal()
            .AssertNoViolations(helper);

        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass, helper.InternalClass)
            .Should()
            .BeProtectedInternal()
            .AssertAnyViolations(helper);
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
        Types()
            .That()
            .Are(helper.InternalInnerClass)
            .Should()
            .BePublic()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInnerClass)
            .Should()
            .BePublic()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.ProtectedInternalInnerClass)
            .Should()
            .BePublic()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateInnerClass)
            .Should()
            .BePublic()
            .AssertOnlyViolations(helper);
        Types()
            .That()
            .Are(helper.PrivateProtectedInnerClass)
            .Should()
            .BePublic()
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.PublicClass, helper.OtherPublicClass)
            .Should()
            .BePublic()
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.PublicClass, helper.InternalClass)
            .Should()
            .BePublic()
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task CallAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.CallAny(helper.CalledMethod.FullName).AssertNoViolations(helper);
        should.CallAny("^.*::CalledMethod\\(\\)$", true).AssertNoViolations(helper);
        should.CallAny([helper.CalledMethod.FullName]).AssertNoViolations(helper);
        should.CallAny(["^.*::CalledMethod\\(\\)$"], true).AssertNoViolations(helper);
        should.CallAny(helper.CalledMethod).AssertNoViolations(helper);
        should.CallAny([helper.CalledMethod]).AssertNoViolations(helper);
        should.CallAny(MethodMembers().That().Are(helper.CalledMethod)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.CallAny(helper.MethodWithoutDependencies.FullName).AssertOnlyViolations(helper);
        should.CallAny([helper.MethodWithoutDependencies.FullName]).AssertOnlyViolations(helper);
        should.CallAny(helper.MethodWithoutDependencies).AssertOnlyViolations(helper);
        should.CallAny([helper.MethodWithoutDependencies]).AssertOnlyViolations(helper);
        should
            .CallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent method member");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.CallAny(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.CallAny([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.CallAny(new List<string>()).AssertOnlyViolations(helper);
        should.CallAny(new List<MethodMember>()).AssertOnlyViolations(helper);
        should
            .CallAny(MethodMembers().That().Are(helper.NonExistentObjectName))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = MethodMembers().That().Are(helper.MethodWithMultipleDependencies).Should();
        should
            .CallAny(
                [
                    helper.MethodWithoutDependencies.FullName,
                    helper.MethodWithMultipleDependencies.FullName
                ]
            )
            .AssertOnlyViolations(helper);
        should
            .CallAny(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies)
            .AssertOnlyViolations(helper);
        should
            .CallAny([helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies])
            .AssertOnlyViolations(helper);
        should
            .CallAny(
                MethodMembers()
                    .That()
                    .Are(helper.MethodWithoutDependencies, helper.MethodWithMultipleDependencies)
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input with multiple dependencies");
        MethodMembers()
            .That()
            .Are(helper.MethodWithMultipleDependencies)
            .Should()
            .CallAny(helper.CalledMethod1, helper.MethodWithoutDependencies)
            .AssertNoViolations(helper);
        MethodMembers()
            .That()
            .Are(helper.MethodWithMultipleDependencies)
            .Should()
            .CallAny(helper.MethodWithoutDependencies)
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task DependOnAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.DependOnAny(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.DependOnAny("^.*\\.BaseClass$", true).AssertNoViolations(helper);
        should.DependOnAny([helper.BaseClass.FullName]).AssertNoViolations(helper);
        should.DependOnAny(["^.*\\.BaseClass$"], true).AssertNoViolations(helper);
        should.DependOnAny(helper.BaseClass).AssertNoViolations(helper);
        should.DependOnAny(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.DependOnAny(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.DependOnAny([helper.BaseClass]).AssertNoViolations(helper);
        should.DependOnAny([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithMultipleDependencies.FullName).Should();
        should.DependOnAny(helper.ClassWithoutDependencies.FullName).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies.FullName]).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should
            .DependOnAny(Classes().That().Are(helper.ClassWithoutDependencies))
            .AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should
            .DependOnAny([helper.ClassWithoutDependenciesSystemType])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent type");
        should = Types().That().Are(helper.ClassWithMultipleDependencies.FullName).Should();
        should.DependOnAny(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.DependOnAny([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithMultipleDependencies.FullName).Should();
        should
            .DependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies.FullName).Should();
        should.DependOnAny(new List<string>()).AssertOnlyViolations(helper);
        should.DependOnAny(new List<IType>()).AssertOnlyViolations(helper);
        should.DependOnAny(new List<System.Type>()).AssertOnlyViolations(helper);
        should
            .DependOnAny(Classes().That().Are(helper.NonExistentObjectName))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies.FullName).Should();
        should
            .DependOnAny([helper.ClassWithoutDependencies.FullName, helper.BaseClass.FullName])
            .AssertOnlyViolations(helper);
        should
            .DependOnAny(helper.ClassWithoutDependencies, helper.BaseClass)
            .AssertOnlyViolations(helper);
        should
            .DependOnAny([helper.ClassWithoutDependencies, helper.BaseClass])
            .AssertOnlyViolations(helper);
        should
            .DependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)
            .AssertOnlyViolations(helper);
        should
            .DependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input without dependencies");
        should = Types().That().Are(helper.ClassWithoutDependencies).Should();
        should
            .DependOnAny([helper.BaseClass.FullName, helper.ChildClass.FullName])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ChildClass1, helper.ChildClass2)
            .Should()
            .DependOnAny(helper.BaseClassWithMultipleDependenciesSystemType)
            .AssertNoViolations(helper);
        should = Types().That().Are(helper.ChildClass, helper.BaseClass).Should();
        should.DependOnAny(helper.ClassWithoutDependencies.FullName).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies.FullName]).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependencies).AssertOnlyViolations(helper);
        should.DependOnAny([helper.ClassWithoutDependencies]).AssertOnlyViolations(helper);
        should.DependOnAny(helper.ClassWithoutDependenciesSystemType).AssertOnlyViolations(helper);
        should
            .DependOnAny([helper.ClassWithoutDependenciesSystemType])
            .AssertOnlyViolations(helper);
        should
            .DependOnAny(Classes().That().Are(helper.ClassWithoutDependencies))
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task DependOnAnyTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.ChildClass)
            .Should()
            .DependOnAnyTypesThat()
            .Are(helper.BaseClass)
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types()
            .That()
            .Are(helper.BaseClass)
            .Should()
            .DependOnAnyTypesThat()
            .Are(helper.ChildClass)
            .AssertOnlyViolations(helper);
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
        Types()
            .That()
            .Are(helper.NonExistentObjectName)
            .Should()
            .Exist()
            .AssertOnlyViolations(helper);
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
        should
            .FollowCustomCondition(
                t => new ConditionResult(
                    t,
                    t.Name == "ChildClass",
                    "does not follow custom condition"
                ),
                "follow custom condition"
            )
            .AssertNoViolations(helper);
        should
            .FollowCustomCondition(
                t => t.Name == "ChildClass",
                "follow custom condition",
                "does not follow custom condition"
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();
        should.FollowCustomCondition(new CustomCondition()).AssertOnlyViolations(helper);
        should
            .FollowCustomCondition(
                t => new ConditionResult(
                    t,
                    t.Name == "ChildClass",
                    "does not follow custom condition"
                ),
                "follow custom condition"
            )
            .AssertOnlyViolations(helper);
        should
            .FollowCustomCondition(
                t => t.Name == "ChildClass",
                "follow custom condition",
                "does not follow custom condition"
            )
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributes(helper.Attribute1.FullName).AssertNoViolations(helper);
        should.HaveAnyAttributes("^.*\\.Attribute1$", true).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1.FullName]).AssertNoViolations(helper);
        should.HaveAnyAttributes(["^.*\\.Attribute1$"], true).AssertNoViolations(helper);
        should.HaveAnyAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.HaveAnyAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.HaveAnyAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should
            .HaveAnyAttributes(Attributes().That().Are(helper.Attribute1))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributes(helper.UnusedAttribute.FullName).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.UnusedAttribute.FullName]).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should
            .HaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent attribute");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributes(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.HaveAnyAttributes([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributes(new List<string>()).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.HaveAnyAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should
            .HaveAnyAttributes(Attributes().That().Are(helper.NonExistentObjectName))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should
            .HaveAnyAttributes([helper.Attribute1.FullName, helper.UnusedAttribute.FullName])
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributes(helper.Attribute1, helper.UnusedAttribute)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributes([helper.Attribute1, helper.UnusedAttribute])
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributes(helper.Attribute1SystemType, helper.UnusedAttributeSystemType)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributes([helper.Attribute1SystemType, helper.UnusedAttributeSystemType])
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.UnusedAttribute))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ClassWithAttributes, helper.OtherClassWithAttributes)
            .Should()
            .HaveAnyAttributes(helper.Attribute1)
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ClassWithAttributes, helper.ClassWithoutAttributes)
            .Should()
            .HaveAnyAttributes(helper.Attribute1)
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesThatTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributesThat().Are(helper.Attribute1).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should.HaveAnyAttributesThat().Are(helper.UnusedAttribute).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithArguments(helper.Attribute1Parameter3Value)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithArguments([helper.Attribute1Parameter3Value])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithArguments(helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithArguments([helper.Attribute1Parameter1Value])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithArguments(helper.UnusedTypeParameterValue)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithArguments([helper.UnusedTypeParameterValue])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();
        should
            .HaveAnyAttributesWithArguments(helper.Attribute1Parameter2Value)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithArguments([helper.Attribute1Parameter2Value])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should.HaveAnyAttributesWithArguments(null).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        Types()
            .That()
            .Are(helper.ClassWithArguments)
            .Should()
            .HaveAnyAttributesWithArguments([])
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ClassWithAttributes)
            .Should()
            .HaveAnyAttributesWithArguments([])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithArguments(
                [helper.Attribute1Parameter3InvalidValue, helper.UnusedParameterValue]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithArguments(
                helper.Attribute1Parameter3InvalidValue,
                helper.UnusedParameterValue
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ClassWithArguments, helper.OtherClassWithArguments)
            .Should()
            .HaveAnyAttributesWithArguments(helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ClassWithArguments, helper.ClassWithAttributes)
            .Should()
            .HaveAnyAttributesWithArguments(helper.Attribute1Parameter2Value)
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAnyAttributesWithNamedArguments()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1Pair)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1Pair])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter2Pair)
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter2Pair])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1InvalidNamePair)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1InvalidNamePair])
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1InvalidValuePair)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1InvalidValuePair])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter2InvalidNamePair)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter2InvalidNamePair])
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter2InvalidValuePair)
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter2InvalidValuePair])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should.HaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAnyAttributesWithNamedArguments(
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter1Pair, helper.Attribute1NamedParameter2Pair]
            )
            .AssertNoViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments(
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments(
                [
                    helper.Attribute1NamedParameter1Pair,
                    helper.Attribute1NamedParameter2InvalidValuePair
                ]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .HaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1Pair)
            .AssertAnyViolations(helper);
        should
            .HaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1Pair])
            .AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAttributeWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter3Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter3Value]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter3Value)
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1Parameter3Value])
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter3Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter3Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter1Value]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1Parameter1Value])
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter1Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter3InvalidValue
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter3InvalidValue]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter3InvalidValue)
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1,
                [helper.Attribute1Parameter3InvalidValue]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter3InvalidValue
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter3InvalidValue]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute2Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute2Parameter2Value]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute2Parameter2Value)
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2Parameter2Value])
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute2Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute2Parameter2Value]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent attribute");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.NonExistentObjectName,
                helper.Attribute1Parameter1Value
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                typeof(TypeDependencyNamespace.BaseClass),
                helper.Attribute1Parameter1Value
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(helper.Attribute1.FullName, null)
            .AssertOnlyViolations(helper);
        should.HaveAttributeWithArguments(helper.Attribute1, null).AssertOnlyViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1SystemType, null)
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(helper.Attribute1.FullName, new List<object>())
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1, new List<object>())
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(helper.Attribute1SystemType, new List<object>())
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter1Value, helper.Attribute1Parameter2Value]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1,
                helper.Attribute1Parameter1Value,
                helper.Attribute1Parameter2Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1,
                [helper.Attribute1Parameter1Value, helper.Attribute1Parameter2Value]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter1Value,
                helper.Attribute1Parameter2Value
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter1Value, helper.Attribute1Parameter2Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ClassWithArguments, helper.OtherClassWithArguments)
            .Should()
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ClassWithArguments, helper.ClassWithAttributes)
            .Should()
            .HaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter2Value)
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveAttributeWithNamedArguments()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1InvalidValuePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1InvalidValuePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1InvalidValuePair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter2InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter2InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter2InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter2InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter2InvalidNamePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter2InvalidNamePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter2InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter2InvalidValuePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter2InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter2InvalidValuePair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter2InvalidValuePair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter2InvalidValuePair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttribute.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttribute.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttribute,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttribute,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttributeSystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.UnusedAttributeSystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                typeof(TypeDependencyNamespace.BaseClass),
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Emtpy arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(helper.Attribute1.FullName, [])
            .AssertNoViolations(helper);
        should.HaveAttributeWithNamedArguments(helper.Attribute1, []).AssertNoViolations(helper);
        should
            .HaveAttributeWithNamedArguments(helper.Attribute1SystemType, [])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute2NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute2NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute2NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute2NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute2NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute2NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .HaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task HaveNameTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.BaseClass).Should();
        should.HaveName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.HaveName("^Base.*$", true).AssertNoViolations(helper);
        should.HaveFullName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.HaveFullName("^.*\\.Base.*$", true).AssertNoViolations(helper);
        should.HaveNameContaining("Base").AssertNoViolations(helper);
        should.HaveFullNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.HaveNameStartingWith("Base").AssertNoViolations(helper);
        should.HaveNameEndingWith("Class").AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();
        should.HaveName(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.HaveName("^.*\\.Base.*$", false).AssertOnlyViolations(helper);
        should.HaveFullName(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.HaveFullName("^Base.*$", false).AssertOnlyViolations(helper);
        should.HaveNameContaining(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.HaveFullNameContaining(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.HaveNameStartingWith(helper.BaseClass.Namespace.Name).AssertOnlyViolations(helper);
        should.HaveNameEndingWith("Base").AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().DependOnAny(helper.BaseClass.FullName).Should();
        should.NotBe(helper.ClassWithoutDependencies.FullName).AssertNoViolations(helper);
        should.NotBe("^.*\\.ClassWithoutDependencies$", true).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependencies.FullName]).AssertNoViolations(helper);
        should.NotBe("^.*\\.ClassWithoutDependencies$", true).AssertNoViolations(helper);
        should.NotBe(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.NotBe(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should
            .NotBe(Classes().That().Are(helper.ClassWithoutDependencies))
            .AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should.NotBe([helper.ClassWithoutDependenciesSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().DependOnAny(helper.BaseClass.FullName).Should();
        should.NotBe(helper.ChildClass.FullName).AssertAnyViolations(helper);
        should.NotBe([helper.ChildClass.FullName]).AssertAnyViolations(helper);
        should.NotBe(helper.ChildClass).AssertAnyViolations(helper);
        should.NotBe(helper.ChildClassSystemType).AssertAnyViolations(helper);
        should.NotBe(Classes().That().Are(helper.ChildClass)).AssertAnyViolations(helper);
        should.NotBe([helper.ChildClass]).AssertAnyViolations(helper);
        should.NotBe([helper.ChildClassSystemType]).AssertAnyViolations(helper);

        helper.AddSnapshotHeader("Non-existent type");
        should = Types().That().DependOnAny(helper.BaseClass.FullName).Should();
        should.NotBe(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotBe([helper.NonExistentObjectName]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().DependOnAny(helper.BaseClass.FullName).Should();
        should.NotBe(new List<string>()).AssertNoViolations(helper);
        should.NotBe(new List<IType>()).AssertNoViolations(helper);
        should.NotBe(new List<System.Type>()).AssertNoViolations(helper);
        should.NotBe(Classes().That().Are(helper.NonExistentObjectName)).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().DependOnAny(helper.BaseClass.FullName).Should();
        should
            .NotBe([helper.ClassWithoutDependencies.FullName, helper.BaseClass.FullName])
            .AssertNoViolations(helper);
        should.NotBe(helper.ClassWithoutDependencies, helper.BaseClass).AssertNoViolations(helper);
        should
            .NotBe([helper.ClassWithoutDependencies, helper.BaseClass])
            .AssertNoViolations(helper);
        should
            .NotBe(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)
            .AssertNoViolations(helper);
        should
            .NotBe([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])
            .AssertNoViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotCallAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.NotCallAny(helper.MethodWithoutDependencies.FullName).AssertNoViolations(helper);
        should.NotCallAny("^.*\\.MethodWithoutDependencies$", true).AssertNoViolations(helper);
        should.NotCallAny([helper.MethodWithoutDependencies.FullName]).AssertNoViolations(helper);
        should.NotCallAny("^.*\\.MethodWithoutDependencies$", true).AssertNoViolations(helper);
        should.NotCallAny(helper.MethodWithoutDependencies).AssertNoViolations(helper);
        should.NotCallAny([helper.MethodWithoutDependencies]).AssertNoViolations(helper);
        should
            .NotCallAny(MethodMembers().That().Are(helper.MethodWithoutDependencies))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.NotCallAny(helper.CalledMethod.FullName).AssertOnlyViolations(helper);
        should.NotCallAny([helper.CalledMethod.FullName]).AssertOnlyViolations(helper);
        should.NotCallAny(helper.CalledMethod).AssertOnlyViolations(helper);
        should.NotCallAny([helper.CalledMethod]).AssertOnlyViolations(helper);
        should
            .NotCallAny(MethodMembers().That().Are(helper.CalledMethod))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent method member");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.NotCallAny(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotCallAny([helper.NonExistentObjectName]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = MethodMembers().That().Are(helper.MethodWithSingleDependency).Should();
        should.NotCallAny(new List<string>()).AssertNoViolations(helper);
        should.NotCallAny(new List<MethodMember>()).AssertNoViolations(helper);
        should
            .NotCallAny(MethodMembers().That().Are(helper.NonExistentObjectName))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = MethodMembers().That().Are(helper.MethodWithMultipleDependencies).Should();
        should
            .NotCallAny("^.*::(MethodWithoutDependencies|CalledMethod[0-9])\\(\\)$", true)
            .AssertOnlyViolations(helper);
        should
            .NotCallAny(
                [
                    helper.MethodWithoutDependencies.FullName,
                    helper.CalledMethod1.FullName,
                    helper.CalledMethod2.FullName
                ]
            )
            .AssertOnlyViolations(helper);
        should
            .NotCallAny(
                helper.MethodWithoutDependencies,
                helper.CalledMethod1,
                helper.CalledMethod2
            )
            .AssertOnlyViolations(helper);
        should
            .NotCallAny(
                [helper.MethodWithoutDependencies, helper.CalledMethod1, helper.CalledMethod2]
            )
            .AssertOnlyViolations(helper);
        should
            .NotCallAny(
                MethodMembers()
                    .That()
                    .Are(
                        helper.MethodWithoutDependencies,
                        helper.CalledMethod1,
                        helper.CalledMethod2
                    )
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        MethodMembers()
            .That()
            .Are(helper.MethodWithSingleDependency, helper.MethodWithMultipleDependencies)
            .Should()
            .NotCallAny(helper.MethodWithoutDependencies)
            .AssertNoViolations(helper);
        MethodMembers()
            .That()
            .Are(helper.MethodWithSingleDependency, helper.MethodWithMultipleDependencies)
            .Should()
            .NotCallAny(helper.CalledMethod, helper.CalledMethod1, helper.CalledMethod2)
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotDependOnAnyTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        Types()
            .That()
            .Are(helper.BaseClass)
            .Should()
            .NotDependOnAnyTypesThat()
            .Are(helper.ChildClass)
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        Types()
            .That()
            .Are(helper.ChildClass)
            .Should()
            .NotDependOnAnyTypesThat()
            .Are(helper.BaseClass)
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotDependOnAnyTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.NotDependOnAny(helper.ClassWithoutDependencies.FullName).AssertNoViolations(helper);
        should.NotDependOnAny("^.*\\.ClassWithoutDependencies$", true).AssertNoViolations(helper);
        should
            .NotDependOnAny([helper.ClassWithoutDependencies.FullName])
            .AssertNoViolations(helper);
        should.NotDependOnAny("^.*\\.ClassWithoutDependencies$", true).AssertNoViolations(helper);
        should.NotDependOnAny(helper.ClassWithoutDependencies).AssertNoViolations(helper);
        should.NotDependOnAny(helper.ClassWithoutDependenciesSystemType).AssertNoViolations(helper);
        should
            .NotDependOnAny(Classes().That().Are(helper.ClassWithoutDependencies))
            .AssertNoViolations(helper);
        should.NotDependOnAny([helper.ClassWithoutDependencies]).AssertNoViolations(helper);
        should
            .NotDependOnAny([helper.ClassWithoutDependenciesSystemType])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ChildClass).Should();
        should.NotDependOnAny(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClass.FullName]).AssertOnlyViolations(helper);
        should.NotDependOnAny(helper.BaseClass).AssertOnlyViolations(helper);
        should.NotDependOnAny(helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.NotDependOnAny(Classes().That().Are(helper.BaseClass)).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClass]).AssertOnlyViolations(helper);
        should.NotDependOnAny([helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent type");
        should = Types().That().Are(helper.ChildClass).Should();
        should.NotDependOnAny(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotDependOnAny([helper.NonExistentObjectName]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ChildClass).Should();
        should
            .NotDependOnAny(typeof(AttributeNamespace.ClassWithoutAttributes))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ChildClass).Should();
        should.NotDependOnAny(new List<string>()).AssertNoViolations(helper);
        should.NotDependOnAny(new List<IType>()).AssertNoViolations(helper);
        should.NotDependOnAny(new List<System.Type>()).AssertNoViolations(helper);
        should
            .NotDependOnAny(Classes().That().Are(helper.NonExistentObjectName))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ChildClass).Should();
        should
            .NotDependOnAny([helper.ClassWithoutDependencies.FullName, helper.BaseClass.FullName])
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny(helper.ClassWithoutDependencies, helper.BaseClass)
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny([helper.ClassWithoutDependencies, helper.BaseClass])
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny(helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType)
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny([helper.ClassWithoutDependenciesSystemType, helper.BaseClassSystemType])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Input with multiple dependencies");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should
            .NotDependOnAny("^.*\\.(BaseClassWithMember|OtherBaseClass)$", true)
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny([helper.BaseClassWithMember.FullName, helper.OtherBaseClass.FullName])
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny(helper.BaseClassWithMember, helper.OtherBaseClass)
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny([helper.BaseClassWithMember, helper.OtherBaseClass])
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny(helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType)
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny([helper.BaseClassWithMemberSystemType, helper.OtherBaseClassSystemType])
            .AssertOnlyViolations(helper);
        should
            .NotDependOnAny(Classes().That().Are(helper.BaseClassWithMember, helper.OtherBaseClass))
            .AssertOnlyViolations(helper);

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
        should.NotHaveAnyAttributes(helper.UnusedAttribute.FullName).AssertNoViolations(helper);
        should.NotHaveAnyAttributes("^.*\\.UnusedAttribute$", true).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.UnusedAttribute.FullName]).AssertNoViolations(helper);
        should.NotHaveAnyAttributes("^.*\\.UnusedAttribute$", true).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(helper.UnusedAttribute).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.UnusedAttribute]).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(helper.UnusedAttributeSystemType).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.UnusedAttributeSystemType]).AssertNoViolations(helper);
        should
            .NotHaveAnyAttributes(Attributes().That().Are(helper.UnusedAttribute))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.NotHaveAnyAttributes(helper.Attribute1.FullName).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1.FullName]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(helper.Attribute1).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1]).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes(helper.Attribute1SystemType).AssertOnlyViolations(helper);
        should.NotHaveAnyAttributes([helper.Attribute1SystemType]).AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent attribute");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();
        should.NotHaveAnyAttributes(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotHaveAnyAttributes([helper.NonExistentObjectName]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should
            .NotHaveAnyAttributes(typeof(TypeDependencyNamespace.BaseClass))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();
        should.NotHaveAnyAttributes(new List<string>()).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(new List<Attribute>()).AssertNoViolations(helper);
        should.NotHaveAnyAttributes(new List<System.Type>()).AssertNoViolations(helper);
        should
            .NotHaveAnyAttributes(Attributes().That().Are(helper.NonExistentObjectName))
            .AssertNoViolations(helper);
        should = Types().That().Are(helper.NonExistentObjectName).Should();
        should
            .NotHaveAnyAttributes(new List<string>())
            .WithoutRequiringPositiveResults()
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributes(new List<Attribute>())
            .WithoutRequiringPositiveResults()
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributes(new List<System.Type>())
            .WithoutRequiringPositiveResults()
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributes(Attributes().That().Are(helper.NonExistentObjectName))
            .WithoutRequiringPositiveResults()
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should
            .NotHaveAnyAttributes([helper.Attribute1.FullName, helper.Attribute2.FullName])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes(helper.Attribute1, helper.Attribute2)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes([helper.Attribute1, helper.Attribute2])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Types()
            .That()
            .Are(helper.ClassWithoutAttributes, helper.OtherClassWithoutAttributes)
            .Should()
            .NotHaveAnyAttributes(helper.Attribute2)
            .AssertNoViolations(helper);
        Types()
            .That()
            .Are(helper.ClassWithoutAttributes, helper.OtherClassWithAttributes)
            .Should()
            .NotHaveAnyAttributes(helper.Attribute1)
            .AssertAnyViolations(helper);
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
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.UnusedTypeParameterValue)
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments([helper.UnusedTypeParameterValue])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.UnusedParameterValue)
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments([helper.UnusedParameterValue])
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.Attribute1Parameter3Value)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments([helper.Attribute1Parameter3Value])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.Attribute1Parameter2Value)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments([helper.Attribute1Parameter2Value])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type without attributes");
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should.NotHaveAnyAttributesWithArguments(null).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should.NotHaveAnyAttributesWithArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithArguments(
                [helper.UnusedTypeParameterValue, helper.Attribute1Parameter1Value]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments(
                helper.UnusedTypeParameterValue,
                helper.Attribute1Parameter1Value
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .NotHaveAnyAttributesWithArguments(helper.Attribute1Parameter1Value)
            .AssertAnyViolations(helper);
        should
            .NotHaveAnyAttributesWithArguments([helper.Attribute1Parameter1Value])
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAnyAttributesWithNamedArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1InvalidNamePair)
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter1InvalidNamePair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                helper.Attribute1NamedParameter1InvalidValuePair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter1InvalidValuePair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter2InvalidNamePair)
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter2InvalidNamePair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                helper.Attribute1NamedParameter2InvalidValuePair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter2InvalidValuePair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1Pair)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1Pair])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter2Pair)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter2Pair])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should.NotHaveAnyAttributesWithNamedArguments([]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(
                [helper.Attribute1NamedParameter1Pair, helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments(
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .NotHaveAnyAttributesWithNamedArguments(helper.Attribute1NamedParameter1Pair)
            .AssertAnyViolations(helper);
        should
            .NotHaveAnyAttributesWithNamedArguments([helper.Attribute1NamedParameter1Pair])
            .AssertAnyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAttributeWithArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute2Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute2Parameter1Value]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2Parameter1Value)
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2Parameter1Value])
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute2Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute2Parameter1Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute2Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute2Parameter1Value]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute2Parameter1Value)
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute2Parameter1Value])
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute2Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute2Parameter1Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter1Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter1Value]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter1Value)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1Parameter1Value])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter1Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter1Value]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter2Value]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter2Value)
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1Parameter2Value])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter2Value]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.UnusedAttribute.FullName,
                helper.Attribute1Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.UnusedAttribute.FullName,
                [helper.Attribute1Parameter1Value]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.UnusedAttribute, helper.Attribute1Parameter1Value)
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.UnusedAttribute,
                [helper.Attribute1Parameter1Value]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.UnusedAttributeSystemType,
                helper.Attribute1Parameter1Value
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.UnusedAttributeSystemType,
                [helper.Attribute1Parameter1Value]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(typeof(TypeDependencyNamespace.BaseClass), 1)
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Null argument");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(helper.UnusedAttribute.FullName, null)
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.UnusedAttribute, null)
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.UnusedAttributeSystemType, null)
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(helper.Attribute1.FullName, [])
            .AssertOnlyViolations(helper);
        should.NotHaveAttributeWithArguments(helper.Attribute1, []).AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1SystemType, [])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter3Value,
                helper.Attribute1Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter3Value, helper.Attribute1Parameter2Value]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1,
                helper.Attribute1Parameter3Value,
                helper.Attribute1Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1,
                [helper.Attribute1Parameter3Value, helper.Attribute1Parameter2Value]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter3Value,
                helper.Attribute1Parameter2Value
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter3Value, helper.Attribute1Parameter2Value]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                helper.Attribute1Parameter1Value
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1Parameter1Value]
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, helper.Attribute1Parameter1Value)
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithArguments(helper.Attribute1, [helper.Attribute1Parameter1Value])
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                helper.Attribute1Parameter1Value
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1Parameter1Value]
            )
            .AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveAttributeWithNamedArgumentsTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations with type arguments");
        var should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("No violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute2NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute2NamedParameter1Pair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations with type arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Violations with value arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Unused attribute");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttribute.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttribute.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttribute,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttribute,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttributeSystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.UnusedAttributeSystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                typeof(TypeDependencyNamespace.BaseClass),
                helper.Attribute1NamedParameter1Pair
            )
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(helper.Attribute1.FullName, [])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(helper.Attribute1, [])
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(helper.Attribute1SystemType, [])
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithArguments).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair,
                helper.Attribute1NamedParameter2Pair
            )
            .AssertOnlyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair, helper.Attribute1NamedParameter2Pair]
            )
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types().That().Are(helper.ClassWithArguments, helper.ClassWithAttributes).Should();
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1.FullName,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                helper.Attribute1NamedParameter1Pair
            )
            .AssertAnyViolations(helper);
        should
            .NotHaveAttributeWithNamedArguments(
                helper.Attribute1SystemType,
                [helper.Attribute1NamedParameter1Pair]
            )
            .AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotHaveNameTest()
    {
        var helper = new DependencyAssemblyTestHelper();

        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.BaseClass).Should();
        should.NotHaveName(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.NotHaveName("^.*\\.Base.*$", true).AssertNoViolations(helper);
        should.NotHaveFullName(helper.BaseClass.Name).AssertNoViolations(helper);
        should.NotHaveFullName("^Base.*$", true).AssertNoViolations(helper);
        should.NotHaveNameContaining(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.NotHaveFullNameContaining(helper.NonExistentObjectName).AssertNoViolations(helper);
        should.NotHaveNameStartingWith(helper.BaseClass.Namespace.Name).AssertNoViolations(helper);
        should.NotHaveNameEndingWith("Base").AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.BaseClass).Should();
        should.NotHaveName(helper.BaseClass.Name).AssertOnlyViolations(helper);
        should.NotHaveName("^Base.*$", true).AssertOnlyViolations(helper);
        should.NotHaveFullName(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.NotHaveFullName("^.*\\.Base.*$", true).AssertOnlyViolations(helper);
        should.NotHaveNameContaining("Base").AssertOnlyViolations(helper);
        should
            .NotHaveFullNameContaining(helper.BaseClass.Namespace.Name)
            .AssertOnlyViolations(helper);
        should.NotHaveNameStartingWith("Base").AssertOnlyViolations(helper);
        should.NotHaveNameEndingWith("Class").AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyDependOnTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.OnlyDependOn(helper.BaseClass.FullName).AssertNoViolations(helper);
        should.OnlyDependOn("^.*\\.BaseClass$", true).AssertNoViolations(helper);
        should.OnlyDependOn([helper.BaseClass.FullName]).AssertNoViolations(helper);
        should.OnlyDependOn(["^.*\\.BaseClass$"], true).AssertNoViolations(helper);
        should.OnlyDependOn(helper.BaseClass).AssertNoViolations(helper);
        should.OnlyDependOn(helper.BaseClassSystemType).AssertNoViolations(helper);
        should.OnlyDependOn(Classes().That().Are(helper.BaseClass)).AssertNoViolations(helper);
        should.OnlyDependOn([helper.BaseClass]).AssertNoViolations(helper);
        should.OnlyDependOn([helper.BaseClassSystemType]).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should.OnlyDependOn(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClass.FullName]).AssertOnlyViolations(helper);
        should.OnlyDependOn(helper.BaseClass).AssertOnlyViolations(helper);
        should.OnlyDependOn(helper.BaseClassSystemType).AssertOnlyViolations(helper);
        should.OnlyDependOn(Classes().That().Are(helper.BaseClass)).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClass]).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClassSystemType]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent type");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should.OnlyDependOn(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type outside of architecture");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should
            .OnlyDependOn(typeof(AttributeNamespace.ClassWithoutAttributes))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should.OnlyDependOn(new List<string>()).AssertOnlyViolations(helper);
        should.OnlyDependOn(new List<IType>()).AssertOnlyViolations(helper);
        should.OnlyDependOn(new List<System.Type>()).AssertOnlyViolations(helper);
        should
            .OnlyDependOn(Classes().That().Are(helper.NonExistentObjectName))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should
            .OnlyDependOn([helper.BaseClass.FullName, helper.OtherBaseClass.FullName])
            .AssertOnlyViolations(helper);
        should.OnlyDependOn(helper.BaseClass, helper.OtherBaseClass).AssertOnlyViolations(helper);
        should.OnlyDependOn([helper.BaseClass, helper.OtherBaseClass]).AssertOnlyViolations(helper);
        should
            .OnlyDependOn(helper.BaseClassSystemType, helper.OtherBaseClassSystemType)
            .AssertOnlyViolations(helper);
        should
            .OnlyDependOn([helper.BaseClassSystemType, helper.OtherBaseClassSystemType])
            .AssertOnlyViolations(helper);
        should
            .OnlyDependOn(Classes().That().Are(helper.BaseClass, helper.OtherBaseClass))
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyDependOnTypesThatTest()
    {
        var helper = new DependencyAssemblyTestHelper();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ChildClass).Should();
        should.OnlyDependOnTypesThat().Are(helper.BaseClass.FullName).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithMultipleDependencies).Should();
        should.OnlyDependOnTypesThat().Are(helper.BaseClass.FullName).AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyHaveAttributesTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributes(helper.Attribute1.FullName).AssertNoViolations(helper);
        should.OnlyHaveAttributes("^.*\\.Attribute1$", true).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1.FullName]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(["^.*\\.Attribute1$"], true).AssertNoViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute1).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1]).AssertNoViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute1SystemType).AssertNoViolations(helper);
        should.OnlyHaveAttributes([helper.Attribute1SystemType]).AssertNoViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributes(helper.UnusedAttribute.FullName).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttribute.FullName]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.UnusedAttribute))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Non-existent attribute");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributes(helper.NonExistentObjectName).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.NonExistentObjectName]).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Attribute outside of architecture");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should
            .OnlyHaveAttributes(typeof(TypeDependencyNamespace.BaseClass))
            .AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributes(new List<string>()).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(new List<Attribute>()).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(new List<System.Type>()).AssertOnlyViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.NonExistentObjectName))
            .AssertOnlyViolations(helper);
        should = Types().That().Are(helper.ClassWithoutAttributes).Should();
        should.OnlyHaveAttributes(new List<string>()).AssertNoViolations(helper);
        should.OnlyHaveAttributes(new List<Attribute>()).AssertNoViolations(helper);
        should.OnlyHaveAttributes(new List<System.Type>()).AssertNoViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.NonExistentObjectName))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Types().That().Are(helper.ClassWithAttributes).Should();
        should
            .OnlyHaveAttributes([helper.Attribute1.FullName, helper.Attribute2.FullName])
            .AssertNoViolations(helper);
        should.OnlyHaveAttributes(helper.Attribute1, helper.Attribute2).AssertNoViolations(helper);
        should
            .OnlyHaveAttributes([helper.Attribute1, helper.Attribute2])
            .AssertNoViolations(helper);
        should
            .OnlyHaveAttributes(helper.Attribute1SystemType, helper.Attribute2SystemType)
            .AssertNoViolations(helper);
        should
            .OnlyHaveAttributes([helper.Attribute1SystemType, helper.Attribute2SystemType])
            .AssertNoViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.Attribute1, helper.Attribute2))
            .AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        should = Types()
            .That()
            .Are(helper.ClassWithAttributes, helper.OtherClassWithAttributes)
            .Should();
        should.OnlyHaveAttributes(helper.UnusedAttribute.FullName).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttribute.FullName]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(helper.UnusedAttribute).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttribute]).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes(helper.UnusedAttributeSystemType).AssertOnlyViolations(helper);
        should.OnlyHaveAttributes([helper.UnusedAttributeSystemType]).AssertOnlyViolations(helper);
        should
            .OnlyHaveAttributes(Attributes().That().Are(helper.UnusedAttribute))
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task OnlyHaveAttributesThatTest()
    {
        var helper = new AttributeAssemblyTestHelpers();
        helper.AddSnapshotHeader("No violations");
        var should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should.OnlyHaveAttributesThat().Are(helper.Attribute1.FullName).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Types().That().Are(helper.ClassWithSingleAttribute).Should();
        should
            .OnlyHaveAttributesThat()
            .Are(helper.UnusedAttribute.FullName)
            .AssertOnlyViolations(helper);
        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public void VisibilityTest()
    {
        var visibilityRules = new List<IArchRule>
        {
            Types().That().ArePrivate().Should().BePrivate(),
            Types().That().ArePublic().Should().BePublic(),
            Types().That().AreProtected().Should().BeProtected(),
            Types().That().AreInternal().Should().BeInternal(),
            Types().That().AreProtectedInternal().Should().BeProtectedInternal(),
            Types().That().ArePrivateProtected().Should().BePrivateProtected(),
            Types().That().AreNotPrivate().Should().NotBePrivate(),
            Types().That().AreNotPublic().Should().NotBePublic(),
            Types().That().AreNotProtected().Should().NotBeProtected(),
            Types().That().AreNotInternal().Should().NotBeInternal(),
            Types().That().AreNotProtectedInternal().Should().NotBeProtectedInternal(),
            Types().That().AreNotPrivateProtected().Should().NotBePrivateProtected(),
            Types()
                .That()
                .ArePrivate()
                .Should()
                .NotBePublic()
                .AndShould()
                .NotBeProtected()
                .AndShould()
                .NotBeInternal()
                .AndShould()
                .NotBeProtectedInternal()
                .AndShould()
                .NotBePrivateProtected(),
            Types()
                .That()
                .ArePublic()
                .Should()
                .NotBePrivate()
                .AndShould()
                .NotBeProtected()
                .AndShould()
                .NotBeInternal()
                .AndShould()
                .NotBeProtectedInternal()
                .AndShould()
                .NotBePrivateProtected(),
            Types()
                .That()
                .AreProtected()
                .Should()
                .NotBePublic()
                .AndShould()
                .NotBePrivate()
                .AndShould()
                .NotBeInternal()
                .AndShould()
                .NotBeProtectedInternal()
                .AndShould()
                .NotBePrivateProtected(),
            Types()
                .That()
                .AreInternal()
                .Should()
                .NotBePublic()
                .AndShould()
                .NotBeProtected()
                .AndShould()
                .NotBePrivate()
                .AndShould()
                .NotBeProtectedInternal()
                .AndShould()
                .NotBePrivateProtected(),
            Types()
                .That()
                .AreProtectedInternal()
                .Should()
                .NotBePublic()
                .AndShould()
                .NotBeProtected()
                .AndShould()
                .NotBeInternal()
                .AndShould()
                .NotBePrivate()
                .AndShould()
                .NotBePrivateProtected(),
            Types()
                .That()
                .ArePrivateProtected()
                .Should()
                .NotBePublic()
                .AndShould()
                .NotBeProtected()
                .AndShould()
                .NotBeInternal()
                .AndShould()
                .NotBeProtectedInternal()
                .AndShould()
                .NotBePrivate(),
            Types()
                .That()
                .AreNotPrivate()
                .Should()
                .BePublic()
                .OrShould()
                .BeProtected()
                .OrShould()
                .BeInternal()
                .OrShould()
                .BeProtectedInternal()
                .OrShould()
                .BePrivateProtected(),
            Types()
                .That()
                .AreNotPublic()
                .Should()
                .BePrivate()
                .OrShould()
                .BeProtected()
                .OrShould()
                .BeInternal()
                .OrShould()
                .BeProtectedInternal()
                .OrShould()
                .BePrivateProtected(),
            Types()
                .That()
                .AreNotProtected()
                .Should()
                .BePublic()
                .OrShould()
                .BePrivate()
                .OrShould()
                .BeInternal()
                .OrShould()
                .BeProtectedInternal()
                .OrShould()
                .BePrivateProtected(),
            Types()
                .That()
                .AreNotInternal()
                .Should()
                .BePublic()
                .OrShould()
                .BeProtected()
                .OrShould()
                .BePrivate()
                .OrShould()
                .BeProtectedInternal()
                .OrShould()
                .BePrivateProtected(),
            Types()
                .That()
                .AreNotProtectedInternal()
                .Should()
                .BePublic()
                .OrShould()
                .BeProtected()
                .OrShould()
                .BeInternal()
                .OrShould()
                .BePrivate()
                .OrShould()
                .BePrivateProtected(),
            Types()
                .That()
                .AreNotPrivateProtected()
                .Should()
                .BePublic()
                .OrShould()
                .BeProtected()
                .OrShould()
                .BeInternal()
                .OrShould()
                .BeProtectedInternal()
                .OrShould()
                .BePrivate(),
        };

        foreach (var visibilityRule in visibilityRules)
        {
            Assert.True(
                visibilityRule.HasNoViolations(StaticTestArchitectures.VisibilityArchitecture)
            );
        }
    }
}
