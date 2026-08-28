using System.Threading.Tasks;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class ClassSyntaxElementsTests
    {
        [Fact]
        public async Task BeAbstractTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.AbstractClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAbstract().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreAbstract()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAbstract().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreAbstract()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.AbstractClass, helper.OtherAbstractClass).Should().BeAbstract().AssertNoViolations(helper);
            Classes().That().Are(helper.AbstractClass, helper.RegularClass).Should().BeAbstract().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeAbstractTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAbstract().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotAbstract()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.AbstractClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAbstract().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotAbstract()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotBeAbstract().AssertNoViolations(helper);
            Classes().That().Are(helper.RegularClass, helper.AbstractClass).Should().NotBeAbstract().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeSealedTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.SealedClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeSealed().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreSealed()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeSealed().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreSealed()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.SealedClass, helper.OtherSealedClass).Should().BeSealed().AssertNoViolations(helper);
            Classes().That().Are(helper.SealedClass, helper.RegularClass).Should().BeSealed().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeSealedTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeSealed().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotSealed()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.SealedClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeSealed().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotSealed()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotBeSealed().AssertNoViolations(helper);
            Classes().That().Are(helper.RegularClass, helper.SealedClass).Should().NotBeSealed().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeRecordTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.RecordClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeRecord().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreRecord()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeRecord().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreRecord()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.RecordClass, helper.OtherRecordClass).Should().BeRecord().AssertNoViolations(helper);
            Classes().That().Are(helper.RecordClass, helper.RegularClass).Should().BeRecord().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeRecordTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.RegularClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeRecord().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotRecord()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.RecordClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeRecord().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotRecord()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.RegularClass, helper.OtherRegularClass).Should().NotBeRecord().AssertNoViolations(helper);
            Classes().That().Are(helper.RegularClass, helper.RecordClass).Should().NotBeRecord().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeImmutableTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.ImmutableClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeImmutable().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreImmutable()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.MutableClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeImmutable().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreImmutable()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.ImmutableClass, helper.OtherImmutableClass).Should().BeImmutable().AssertNoViolations(helper);
            Classes().That().Are(helper.ImmutableClass, helper.MutableClass).Should().BeImmutable().AssertAnyViolations(helper);

            helper.AddSnapshotHeader("Class without members (vacuously immutable)");
            should = Classes().That().Are(helper.ClassWithoutMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeImmutable().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreImmutable()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Class with only static members (vacuously immutable)");
            should = Classes().That().Are(helper.ClassWithOnlyStaticMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeImmutable().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreImmutable()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Mutable static member does not make a class mutable");
            should = Classes().That().Are(helper.ClassWithImmutableInstanceAndMutableStaticMember).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeImmutable().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreImmutable()).AssertNoViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeImmutableTest()
        {
            var helper = new ClassAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = Classes().That().Are(helper.MutableClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeImmutable().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotImmutable()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Classes().That().Are(helper.ImmutableClass).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeImmutable().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotImmutable()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Class with only static members (vacuously immutable)");
            should = Classes().That().Are(helper.ClassWithOnlyStaticMembers).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeImmutable().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotImmutable()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Mutable static member does not make a class mutable");
            should = Classes().That().Are(helper.ClassWithImmutableInstanceAndMutableStaticMember).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeImmutable().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Classes().That().AreNotImmutable()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Classes().That().Are(helper.MutableClass, helper.OtherMutableClass).Should().NotBeImmutable().AssertNoViolations(helper);
            Classes().That().Are(helper.MutableClass, helper.ImmutableClass).Should().NotBeImmutable().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
