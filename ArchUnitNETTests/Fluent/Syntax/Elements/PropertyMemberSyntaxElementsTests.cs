using System.Threading.Tasks;
using ArchUnitNETTests.AssemblyTestHelper;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    // csharpier-ignore
    public class PropertyMemberSyntaxElementsTests
    {
        [Fact]
        public async Task BeVirtualTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.VirtualProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeVirtual().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().AreVirtual()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.NonVirtualProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.BeVirtual().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().AreVirtual()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.VirtualProperty, helper.OtherVirtualProperty).Should().BeVirtual().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.VirtualProperty, helper.NonVirtualProperty).Should().BeVirtual().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotBeVirtualTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.NonVirtualProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeVirtual().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().AreNotVirtual()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.VirtualProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotBeVirtual().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().AreNotVirtual()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.NonVirtualProperty, helper.WritableProperty).Should().NotBeVirtual().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.NonVirtualProperty, helper.VirtualProperty).Should().NotBeVirtual().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WriteOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().HaveGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.WriteOnlyProperty).Should().HaveGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WriteOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveNoGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveNoGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WriteOnlyProperty, helper.OtherWriteOnlyProperty).Should().NotHaveGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WriteOnlyProperty, helper.WritableProperty).Should().NotHaveGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.GetOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.InitOnlyProperty).Should().HaveSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().HaveSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.GetOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveNoSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveNoSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.GetOnlyProperty, helper.OtherGetOnlyProperty).Should().NotHaveSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.GetOnlyProperty, helper.WritableProperty).Should().NotHaveSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveInitOnlySetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.InitOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInitOnlySetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInitOnlySetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInitOnlySetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInitOnlySetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.InitOnlyProperty, helper.OtherInitOnlyProperty).Should().HaveInitOnlySetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.InitOnlyProperty, helper.WritableProperty).Should().HaveInitOnlySetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveInitOnlySetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInitOnlySetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInitOnlySetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.InitOnlyProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInitOnlySetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInitOnlySetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHaveInitOnlySetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.InitOnlyProperty).Should().NotHaveInitOnlySetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePublicGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePublicGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePublicGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePublicGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePublicGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().HavePublicGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateGetter).Should().HavePublicGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePublicGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePublicGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePublicGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePublicGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePublicGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateGetter, helper.PropertyWithProtectedGetter).Should().NotHavePublicGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateGetter, helper.WritableProperty).Should().NotHavePublicGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePrivateGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateGetter, helper.OtherPropertyWithPrivateGetter).Should().HavePrivateGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateGetter, helper.WritableProperty).Should().HavePrivateGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePrivateGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHavePrivateGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateGetter).Should().NotHavePrivateGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveProtectedGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithProtectedGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithProtectedGetter, helper.OtherPropertyWithProtectedGetter).Should().HaveProtectedGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithProtectedGetter, helper.WritableProperty).Should().HaveProtectedGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveProtectedGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithProtectedGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHaveProtectedGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithProtectedGetter).Should().NotHaveProtectedGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveInternalGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithInternalGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInternalGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInternalGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInternalGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInternalGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithInternalGetter, helper.OtherPropertyWithInternalGetter).Should().HaveInternalGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithInternalGetter, helper.WritableProperty).Should().HaveInternalGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveInternalGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInternalGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInternalGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithInternalGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInternalGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInternalGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHaveInternalGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithInternalGetter).Should().NotHaveInternalGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveProtectedInternalGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithProtectedInternalGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedInternalGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedInternalGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedInternalGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedInternalGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithProtectedInternalGetter, helper.OtherPropertyWithProtectedInternalGetter).Should().HaveProtectedInternalGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithProtectedInternalGetter, helper.WritableProperty).Should().HaveProtectedInternalGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveProtectedInternalGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedInternalGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedInternalGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithProtectedInternalGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedInternalGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedInternalGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHaveProtectedInternalGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithProtectedInternalGetter).Should().NotHaveProtectedInternalGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePrivateProtectedGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateProtectedGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateProtectedGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateProtectedGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateProtectedGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedGetter, helper.OtherPropertyWithPrivateProtectedGetter).Should().HavePrivateProtectedGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedGetter, helper.WritableProperty).Should().HavePrivateProtectedGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePrivateProtectedGetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateProtectedGetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateProtectedGetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedGetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateProtectedGetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateProtectedGetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.GetOnlyProperty).Should().NotHavePrivateProtectedGetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateProtectedGetter).Should().NotHavePrivateProtectedGetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePublicSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePublicSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePublicSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePublicSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePublicSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().HavePublicSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateSetter).Should().HavePublicSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePublicSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePublicSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePublicSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePublicSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePublicSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateSetter, helper.GetOnlyProperty).Should().NotHavePublicSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateSetter, helper.WritableProperty).Should().NotHavePublicSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePrivateSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateSetter, helper.OtherPropertyWithPrivateSetter).Should().HavePrivateSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateSetter, helper.WritableProperty).Should().HavePrivateSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePrivateSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotHavePrivateSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateSetter).Should().NotHavePrivateSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveProtectedSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithProtectedSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithProtectedSetter, helper.OtherPropertyWithProtectedSetter).Should().HaveProtectedSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithProtectedSetter, helper.WritableProperty).Should().HaveProtectedSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveProtectedSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithProtectedSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotHaveProtectedSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithProtectedSetter).Should().NotHaveProtectedSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveInternalSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithInternalSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInternalSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInternalSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveInternalSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveInternalSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithInternalSetter, helper.OtherPropertyWithInternalSetter).Should().HaveInternalSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithInternalSetter, helper.WritableProperty).Should().HaveInternalSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveInternalSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInternalSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInternalSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithInternalSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveInternalSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveInternalSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotHaveInternalSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithInternalSetter).Should().NotHaveInternalSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HaveProtectedInternalSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithProtectedInternalSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedInternalSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedInternalSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HaveProtectedInternalSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HaveProtectedInternalSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithProtectedInternalSetter, helper.OtherPropertyWithProtectedInternalSetter).Should().HaveProtectedInternalSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithProtectedInternalSetter, helper.WritableProperty).Should().HaveProtectedInternalSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHaveProtectedInternalSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedInternalSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedInternalSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithProtectedInternalSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHaveProtectedInternalSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHaveProtectedInternalSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotHaveProtectedInternalSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithProtectedInternalSetter).Should().NotHaveProtectedInternalSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task HavePrivateProtectedSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateProtectedSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateProtectedSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.HavePrivateProtectedSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().HavePrivateProtectedSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedSetter, helper.OtherPropertyWithPrivateProtectedSetter).Should().HavePrivateProtectedSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedSetter, helper.WritableProperty).Should().HavePrivateProtectedSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }

        [Fact]
        public async Task NotHavePrivateProtectedSetterTest()
        {
            var helper = new PropertyMemberAssemblyTestHelper();

            helper.AddSnapshotHeader("No Violations");
            var should = PropertyMembers().That().Are(helper.WritableProperty).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateProtectedSetter().AssertNoViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateProtectedSetter()).AssertNoViolations(helper);

            helper.AddSnapshotHeader("Violations");
            should = PropertyMembers().That().Are(helper.PropertyWithPrivateProtectedSetter).Should();

            helper.AddSnapshotSubHeader("Conditions");
            should.NotHavePrivateProtectedSetter().AssertOnlyViolations(helper);

            helper.AddSnapshotSubHeader("Predicates");
            should.Be(PropertyMembers().That().DoNotHavePrivateProtectedSetter()).AssertOnlyViolations(helper);

            helper.AddSnapshotHeader("Multiple inputs");
            PropertyMembers().That().Are(helper.WritableProperty, helper.OtherWritableProperty).Should().NotHavePrivateProtectedSetter().AssertNoViolations(helper);
            PropertyMembers().That().Are(helper.WritableProperty, helper.PropertyWithPrivateProtectedSetter).Should().NotHavePrivateProtectedSetter().AssertAnyViolations(helper);

            await helper.AssertSnapshotMatches();
        }
    }
}
