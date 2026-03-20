using System.Collections.Generic;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using AttributeNamespace;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

// csharpier-ignore
public class MemberSyntaxElementsTests
{
    [Fact]
    public async Task BeStaticTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeStatic().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreStatic()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.NonStaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeStatic().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreStatic()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.StaticField, helper.OtherStaticField).Should().BeStatic().AssertNoViolations(helper);
        Members().That().Are(helper.StaticField, helper.NonStaticField).Should().BeStatic().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeStaticTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.NonStaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeStatic().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotStatic()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeStatic().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotStatic()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.NonStaticField, helper.ReadOnlyField).Should().NotBeStatic().AssertNoViolations(helper);
        Members().That().Are(helper.NonStaticField, helper.StaticField).Should().NotBeStatic().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeReadOnlyTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.ReadOnlyField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeReadOnly().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreReadOnly()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.WritableProperty).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeReadOnly().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreReadOnly()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.ReadOnlyField, helper.GetOnlyProperty).Should().BeReadOnly().AssertNoViolations(helper);
        Members().That().Are(helper.ReadOnlyField, helper.WritableProperty).Should().BeReadOnly().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeReadOnlyTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.WritableProperty).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeReadOnly().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotReadOnly()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.ReadOnlyField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeReadOnly().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotReadOnly()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.WritableProperty, helper.InitOnlyProperty).Should().NotBeReadOnly().AssertNoViolations(helper);
        Members().That().Are(helper.WritableProperty, helper.ReadOnlyField).Should().NotBeReadOnly().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeImmutableTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.ReadOnlyField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeImmutable().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreImmutable()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.WritableProperty).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeImmutable().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreImmutable()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.ReadOnlyField, helper.GetOnlyProperty).Should().BeImmutable().AssertNoViolations(helper);
        Members().That().Are(helper.ReadOnlyField, helper.WritableProperty).Should().BeImmutable().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeImmutableTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.WritableProperty).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeImmutable().AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotImmutable()).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.ReadOnlyField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeImmutable().AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotImmutable()).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotBeImmutable().AssertNoViolations(helper);
        Members().That().Are(helper.WritableProperty, helper.ReadOnlyField).Should().NotBeImmutable().AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeDeclaredInTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeDeclaredIn(helper.ClassWithStaticField).AssertNoViolations(helper);
        should.BeDeclaredIn(helper.ClassWithStaticFieldSystemType).AssertNoViolations(helper);
        should.BeDeclaredIn(Types().That().Are(helper.ClassWithStaticField)).AssertNoViolations(helper);
        should.BeDeclaredIn(new List<IType> { helper.ClassWithStaticField }).AssertNoViolations(helper);
        should.BeDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType }).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithStaticField)).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithStaticFieldSystemType)).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(Types().That().Are(helper.ClassWithStaticField))).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<IType> { helper.ClassWithStaticField })).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType })).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Complex conditions");
        should.BeDeclaredInTypesThat().Are(helper.ClassWithStaticField).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeDeclaredIn(helper.ClassWithNonStaticField).AssertOnlyViolations(helper);
        should.BeDeclaredIn(helper.ClassWithNonStaticFieldSystemType).AssertOnlyViolations(helper);
        should.BeDeclaredIn(Types().That().Are(helper.ClassWithNonStaticField)).AssertOnlyViolations(helper);
        should.BeDeclaredIn(new List<IType> { helper.ClassWithNonStaticField }).AssertOnlyViolations(helper);
        should.BeDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType }).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithNonStaticField)).AssertOnlyViolations(helper);
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithNonStaticFieldSystemType)).AssertOnlyViolations(helper);
        should.Be(Members().That().AreDeclaredIn(Types().That().Are(helper.ClassWithNonStaticField))).AssertOnlyViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<IType> { helper.ClassWithNonStaticField })).AssertOnlyViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType })).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Complex conditions");
        should.BeDeclaredInTypesThat().Are(helper.ClassWithNonStaticField).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeDeclaredIn(helper.ClassWithStaticField, helper.ClassWithNonStaticField).AssertNoViolations(helper);
        should.BeDeclaredIn(new List<IType> { helper.ClassWithStaticField, helper.ClassWithNonStaticField }).AssertNoViolations(helper);
        should.BeDeclaredIn(helper.ClassWithStaticFieldSystemType, helper.ClassWithNonStaticFieldSystemType).AssertNoViolations(helper);
        should.BeDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType, helper.ClassWithNonStaticFieldSystemType }).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithStaticField, helper.ClassWithNonStaticField)).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<IType> { helper.ClassWithStaticField, helper.ClassWithNonStaticField })).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(helper.ClassWithStaticFieldSystemType, helper.ClassWithNonStaticFieldSystemType)).AssertNoViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType, helper.ClassWithNonStaticFieldSystemType })).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeDeclaredIn(new List<IType>()).AssertOnlyViolations(helper);
        should.BeDeclaredIn(new List<System.Type>()).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreDeclaredIn(new List<IType>())).AssertOnlyViolations(helper);
        should.Be(Members().That().AreDeclaredIn(new List<System.Type>())).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Type not in architecture");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.BeDeclaredIn(new List<System.Type> { typeof(ClassWithoutAttributes) }).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreDeclaredIn(new List<System.Type> { typeof(ClassWithoutAttributes) })).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.StaticField, helper.OtherStaticField).Should().BeDeclaredIn(helper.ClassWithStaticField).AssertAnyViolations(helper);
        Members().That().Are(helper.StaticField, helper.OtherStaticField).Should().BeDeclaredIn(helper.ClassWithStaticField, helper.OtherClassWithStaticField).AssertNoViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeDeclaredInTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeDeclaredIn(helper.ClassWithNonStaticField).AssertNoViolations(helper);
        should.NotBeDeclaredIn(helper.ClassWithNonStaticFieldSystemType).AssertNoViolations(helper);
        should.NotBeDeclaredIn(Types().That().Are(helper.ClassWithNonStaticField)).AssertNoViolations(helper);
        should.NotBeDeclaredIn(new List<IType> { helper.ClassWithNonStaticField }).AssertNoViolations(helper);
        should.NotBeDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType }).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithNonStaticField)).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithNonStaticFieldSystemType)).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(Types().That().Are(helper.ClassWithNonStaticField))).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<IType> { helper.ClassWithNonStaticField })).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType })).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Complex conditions");
        should.NotBeDeclaredInTypesThat().Are(helper.ClassWithNonStaticField).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeDeclaredIn(helper.ClassWithStaticField).AssertOnlyViolations(helper);
        should.NotBeDeclaredIn(helper.ClassWithStaticFieldSystemType).AssertOnlyViolations(helper);
        should.NotBeDeclaredIn(Types().That().Are(helper.ClassWithStaticField)).AssertOnlyViolations(helper);
        should.NotBeDeclaredIn(new List<IType> { helper.ClassWithStaticField }).AssertOnlyViolations(helper);
        should.NotBeDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType }).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithStaticField)).AssertOnlyViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithStaticFieldSystemType)).AssertOnlyViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(Types().That().Are(helper.ClassWithStaticField))).AssertOnlyViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<IType> { helper.ClassWithStaticField })).AssertOnlyViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<System.Type> { helper.ClassWithStaticFieldSystemType })).AssertOnlyViolations(helper);

        helper.AddSnapshotSubHeader("Complex conditions");
        should.NotBeDeclaredInTypesThat().Are(helper.ClassWithStaticField).AssertOnlyViolations(helper);

        helper.AddSnapshotHeader("Multiple arguments");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeDeclaredIn(helper.ClassWithNonStaticField, helper.ClassWithReadOnlyField).AssertNoViolations(helper);
        should.NotBeDeclaredIn(new List<IType> { helper.ClassWithNonStaticField, helper.ClassWithReadOnlyField }).AssertNoViolations(helper);
        should.NotBeDeclaredIn(helper.ClassWithNonStaticFieldSystemType, helper.ClassWithReadOnlyFieldSystemType).AssertNoViolations(helper);
        should.NotBeDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType, helper.ClassWithReadOnlyFieldSystemType }).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithNonStaticField, helper.ClassWithReadOnlyField)).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<IType> { helper.ClassWithNonStaticField, helper.ClassWithReadOnlyField })).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(helper.ClassWithNonStaticFieldSystemType, helper.ClassWithReadOnlyFieldSystemType)).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<System.Type> { helper.ClassWithNonStaticFieldSystemType, helper.ClassWithReadOnlyFieldSystemType })).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Empty arguments");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeDeclaredIn(new List<IType>()).AssertNoViolations(helper);
        should.NotBeDeclaredIn(new List<System.Type>()).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotDeclaredIn(new List<IType>())).AssertNoViolations(helper);
        should.Be(Members().That().AreNotDeclaredIn(new List<System.Type>())).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Type not in architecture");
        should = Members().That().Are(helper.StaticField).Should();

        helper.AddSnapshotSubHeader("Conditions");
        should.NotBeDeclaredIn(new List<System.Type> { typeof(ClassWithoutAttributes) }).AssertNoViolations(helper);

        helper.AddSnapshotSubHeader("Predicates");
        should.Be(Members().That().AreNotDeclaredIn(new List<System.Type> { typeof(ClassWithoutAttributes) })).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Multiple inputs");
        Members().That().Are(helper.StaticField, helper.NonStaticField).Should().NotBeDeclaredIn(helper.ClassWithReadOnlyField).AssertNoViolations(helper);
        Members().That().Are(helper.StaticField, helper.NonStaticField).Should().NotBeDeclaredIn(helper.ClassWithStaticField).AssertAnyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task BeDeclaredInTypesThatTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.StaticField).Should();
        should.BeDeclaredInTypesThat().Are(helper.ClassWithStaticField).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.StaticField).Should();
        should.BeDeclaredInTypesThat().Are(helper.ClassWithNonStaticField).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }

    [Fact]
    public async Task NotBeDeclaredInTypesThatTest()
    {
        var helper = new TypeAssemblyTestHelper();

        helper.AddSnapshotHeader("No Violations");
        var should = Members().That().Are(helper.StaticField).Should();
        should.NotBeDeclaredInTypesThat().Are(helper.ClassWithNonStaticField).AssertNoViolations(helper);

        helper.AddSnapshotHeader("Violations");
        should = Members().That().Are(helper.StaticField).Should();
        should.NotBeDeclaredInTypesThat().Are(helper.ClassWithStaticField).AssertOnlyViolations(helper);

        await helper.AssertSnapshotMatches();
    }
}
