using System.Threading.Tasks;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class AttributeSyntaxElementsTests
    {
        [Fact]
        public async Task BeAbstractTest()
        {
            var helper = new AttributeAssemblyTestHelpers();

            helper.AddSnapshotHeader("No Violations");
            var should = Attributes().That().Are(helper.AbstractAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAbstract().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreAbstract()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Attributes().That().Are(helper.RegularAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeAbstract().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreAbstract()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Attributes().That().Are(helper.AbstractAttribute, helper.OtherAbstractAttribute).Should().BeAbstract().AssertNoViolations(helper);
            Attributes().That().Are(helper.AbstractAttribute, helper.RegularAttribute).Should().BeAbstract().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeAbstractTest()
        {
            var helper = new AttributeAssemblyTestHelpers();

            helper.AddSnapshotHeader("No Violations");
            var should = Attributes().That().Are(helper.RegularAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAbstract().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreNotAbstract()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Attributes().That().Are(helper.AbstractAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeAbstract().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreNotAbstract()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Attributes().That().Are(helper.RegularAttribute, helper.OtherRegularAttribute).Should().NotBeAbstract().AssertNoViolations(helper);
            Attributes().That().Are(helper.RegularAttribute, helper.AbstractAttribute).Should().NotBeAbstract().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task BeSealedTest()
        {
            var helper = new AttributeAssemblyTestHelpers();

            helper.AddSnapshotHeader("No Violations");
            var should = Attributes().That().Are(helper.SealedAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeSealed().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreSealed()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Attributes().That().Are(helper.RegularAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeSealed().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreSealed()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Attributes().That().Are(helper.SealedAttribute, helper.OtherSealedAttribute).Should().BeSealed().AssertNoViolations(helper);
            Attributes().That().Are(helper.SealedAttribute, helper.RegularAttribute).Should().BeSealed().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeSealedTest()
        {
            var helper = new AttributeAssemblyTestHelpers();

            helper.AddSnapshotHeader("No Violations");
            var should = Attributes().That().Are(helper.RegularAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeSealed().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreNotSealed()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = Attributes().That().Are(helper.SealedAttribute).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeSealed().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(Attributes().That().AreNotSealed()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            Attributes().That().Are(helper.RegularAttribute, helper.OtherRegularAttribute).Should().NotBeSealed().AssertNoViolations(helper);
            Attributes().That().Are(helper.RegularAttribute, helper.SealedAttribute).Should().NotBeSealed().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
