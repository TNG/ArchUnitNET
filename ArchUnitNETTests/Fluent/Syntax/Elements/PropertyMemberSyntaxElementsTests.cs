using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class PropertyMemberSyntaxElementsTests
    {
        public PropertyMemberSyntaxElementsTests()
        {
            _propertyMembers = Architecture.PropertyMembers;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<PropertyMember> _propertyMembers;


        [Fact]
        public void AreVirtualTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberIsVirtual = PropertyMembers().That().Are(propertyMember).Should().BeVirtual();
                var propertyMemberIsNotVirtual = PropertyMembers().That().Are(propertyMember).Should().NotBeVirtual();
                var virtualPropertyMembersDoNotIncludeMember =
                    PropertyMembers().That().AreVirtual().Should().NotBe(propertyMember).OrShould().NotExist();
                var notVirtualPropertyMembersDoNotIncludeMember =
                    PropertyMembers().That().AreNotVirtual().Should().NotBe(propertyMember).AndShould().Exist();

                Assert.Equal(propertyMember.IsVirtual, propertyMemberIsVirtual.HasNoViolations(Architecture));
                Assert.Equal(!propertyMember.IsVirtual, propertyMemberIsNotVirtual.HasNoViolations(Architecture));
                Assert.Equal(!propertyMember.IsVirtual,
                    virtualPropertyMembersDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.IsVirtual,
                    notVirtualPropertyMembersDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var virtualPropertyMembersShouldBeVirtual = PropertyMembers().That().AreVirtual().Should().BeVirtual();
            var virtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreVirtual().Should().NotBeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersShouldBeVirtual =
                PropertyMembers().That().AreNotVirtual().Should().BeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreNotVirtual().Should().NotBeVirtual();

            Assert.True(virtualPropertyMembersShouldBeVirtual.HasNoViolations(Architecture));
            Assert.False(virtualPropertyMembersAreNotVirtual.HasNoViolations(Architecture));
            Assert.False(notVirtualPropertyMembersShouldBeVirtual.HasNoViolations(Architecture));
            Assert.True(notVirtualPropertyMembersAreNotVirtual.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasGetter = PropertyMembers().That().Are(propertyMember).Should().HaveGetter();
                var propertyMemberHasNoGetter = PropertyMembers().That().Are(propertyMember).Should().NotHaveGetter();
                var propertyMembersWithGetterDoNotIncludeMember =
                    PropertyMembers().That().HaveGetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutGetterDoNotIncludeMember =
                    PropertyMembers().That().HaveNoGetter().Should().NotBe(propertyMember).AndShould().Exist();

                Assert.Equal(propertyMember.Visibility != NotAccessible,
                    propertyMemberHasGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMemberHasNoGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMembersWithGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.Visibility != NotAccessible,
                    propertyMembersWithoutGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithGetterHaveGetter = PropertyMembers().That().HaveGetter().Should().HaveGetter();
            var propertyMembersWithGetterHaveNoGetter =
                PropertyMembers().That().HaveGetter().Should().NotHaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveGetter =
                PropertyMembers().That().HaveNoGetter().Should().HaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveNoGetter =
                PropertyMembers().That().HaveNoGetter().Should().NotHaveGetter();

            Assert.True(propertyMembersWithGetterHaveGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithGetterHaveNoGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutGetterHaveGetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutGetterHaveNoGetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveInternalSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasInternalSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveInternalSetter();
                var propertyMemberDoesNotHaveInternalSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveInternalSetter();
                var propertyMembersWithInternalSetterDoNotIncludeMember =
                    PropertyMembers().That().HaveInternalSetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutInternalSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveInternalSetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == Internal,
                    propertyMemberHasInternalSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMemberDoesNotHaveInternalSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMembersWithInternalSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Internal,
                    propertyMembersWithoutInternalSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithInternalSetterHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().HaveInternalSetter();
            var propertyMembersWithInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().NotHaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().HaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().NotHaveInternalSetter();

            Assert.True(propertyMembersWithInternalSetterHaveInternalSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithInternalSetterDoNotHaveInternalSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutInternalSetterHaveInternalSetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutInternalSetterDoNotHaveInternalSetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HavePrivateProtectedSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPrivateProtectedSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePrivateProtectedSetter();
                var propertyMemberDoesNotHavePrivateProtectedSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePrivateProtectedSetter();
                var propertyMembersWithPrivateProtectedSetterDoNotIncludeMember =
                    PropertyMembers().That().HavePrivateProtectedSetter().Should().NotBe(propertyMember).OrShould()
                        .NotExist();
                var propertyMembersWithoutPrivateProtectedSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePrivateProtectedSetter().Should().NotBe(propertyMember)
                        .AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == PrivateProtected,
                    propertyMemberHasPrivateProtectedSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMemberDoesNotHavePrivateProtectedSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMembersWithPrivateProtectedSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == PrivateProtected,
                    propertyMembersWithoutPrivateProtectedSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPrivateProtectedSetterHavePrivateProtectedSetter =
                PropertyMembers().That().HavePrivateProtectedSetter().Should().HavePrivateProtectedSetter();
            var propertyMembersWithPrivateProtectedSetterDoNotHavePrivateProtectedSetter =
                PropertyMembers().That().HavePrivateProtectedSetter().Should().NotHavePrivateProtectedSetter()
                    .AndShould().Exist();
            var propertyMembersWithoutPrivateProtectedSetterHavePrivateProtectedSetter =
                PropertyMembers().That().DoNotHavePrivateProtectedSetter().Should().HavePrivateProtectedSetter()
                    .AndShould().Exist();
            var propertyMembersWithoutPrivateProtectedSetterDoNotHavePrivateProtectedSetter =
                PropertyMembers().That().DoNotHavePrivateProtectedSetter().Should().NotHavePrivateProtectedSetter();

            Assert.True(
                propertyMembersWithPrivateProtectedSetterHavePrivateProtectedSetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithPrivateProtectedSetterDoNotHavePrivateProtectedSetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithoutPrivateProtectedSetterHavePrivateProtectedSetter.HasNoViolations(Architecture));
            Assert.True(
                propertyMembersWithoutPrivateProtectedSetterDoNotHavePrivateProtectedSetter
                    .HasNoViolations(Architecture));
        }

        [Fact]
        public void HavePrivateSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPrivateSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePrivateSetter();
                var propertyMemberDoesNotHavePrivateSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePrivateSetter();
                var propertyMembersWithPrivateSetterDoNotIncludeMember =
                    PropertyMembers().That().HavePrivateSetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutPrivateSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePrivateSetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == Private,
                    propertyMemberHasPrivateSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMemberDoesNotHavePrivateSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMembersWithPrivateSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Private,
                    propertyMembersWithoutPrivateSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPrivateSetterHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().HavePrivateSetter();
            var propertyMembersWithPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().NotHavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().HavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().NotHavePrivateSetter();

            Assert.True(propertyMembersWithPrivateSetterHavePrivateSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithPrivateSetterDoNotHavePrivateSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutPrivateSetterHavePrivateSetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveProtectedInternalSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasProtectedInternalSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveProtectedInternalSetter();
                var propertyMemberDoesNotHaveProtectedInternalSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveProtectedInternalSetter();
                var propertyMembersWithProtectedInternalSetterDoNotIncludeMember =
                    PropertyMembers().That().HaveProtectedInternalSetter().Should().NotBe(propertyMember).OrShould()
                        .NotExist();
                var propertyMembersWithoutProtectedInternalSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveProtectedInternalSetter().Should().NotBe(propertyMember)
                        .AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == ProtectedInternal,
                    propertyMemberHasProtectedInternalSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMemberDoesNotHaveProtectedInternalSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMembersWithProtectedInternalSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == ProtectedInternal,
                    propertyMembersWithoutProtectedInternalSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithProtectedInternalSetterHaveProtectedInternalSetter =
                PropertyMembers().That().HaveProtectedInternalSetter().Should().HaveProtectedInternalSetter();
            var propertyMembersWithProtectedInternalSetterDoNotHaveProtectedInternalSetter =
                PropertyMembers().That().HaveProtectedInternalSetter().Should().NotHaveProtectedInternalSetter()
                    .AndShould().Exist();
            var propertyMembersWithoutProtectedInternalSetterHaveProtectedInternalSetter =
                PropertyMembers().That().DoNotHaveProtectedInternalSetter().Should().HaveProtectedInternalSetter()
                    .AndShould().Exist();
            var propertyMembersWithoutProtectedInternalSetterDoNotHaveProtectedInternalSetter =
                PropertyMembers().That().DoNotHaveProtectedInternalSetter().Should().NotHaveProtectedInternalSetter();

            Assert.True(
                propertyMembersWithProtectedInternalSetterHaveProtectedInternalSetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithProtectedInternalSetterDoNotHaveProtectedInternalSetter
                    .HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithoutProtectedInternalSetterHaveProtectedInternalSetter.HasNoViolations(Architecture));
            Assert.True(
                propertyMembersWithoutProtectedInternalSetterDoNotHaveProtectedInternalSetter.HasNoViolations(
                    Architecture));
        }

        [Fact]
        public void HaveProtectedSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasProtectedSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveProtectedSetter();
                var propertyMemberDoesNotHaveProtectedSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveProtectedSetter();
                var propertyMembersWithProtectedSetterDoNotIncludeMember =
                    PropertyMembers().That().HaveProtectedSetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutProtectedSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveProtectedSetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == Protected,
                    propertyMemberHasProtectedSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMemberDoesNotHaveProtectedSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMembersWithProtectedSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Protected,
                    propertyMembersWithoutProtectedSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().HaveProtectedSetter();
            var propertyMembersWithProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().NotHaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().HaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().NotHaveProtectedSetter();

            Assert.True(propertyMembersWithProtectedSetterHaveProtectedSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithProtectedSetterDoNotHaveProtectedSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutProtectedSetterHaveProtectedSetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HavePublicSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPublicSetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePublicSetter();
                var propertyMemberDoesNotHavePublicSetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePublicSetter();
                var propertyMembersWithPublicSetterDoNotIncludeMember =
                    PropertyMembers().That().HavePublicSetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutPublicSetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePublicSetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.SetterVisibility == Public,
                    propertyMemberHasPublicSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMemberDoesNotHavePublicSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMembersWithPublicSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Public,
                    propertyMembersWithoutPublicSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPublicSetterHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().HavePublicSetter();
            var propertyMembersWithPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().NotHavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().HavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().NotHavePublicSetter();

            Assert.True(propertyMembersWithPublicSetterHavePublicSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithPublicSetterDoNotHavePublicSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutPublicSetterHavePublicSetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutPublicSetterDoNotHavePublicSetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveSetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasSetter = PropertyMembers().That().Are(propertyMember).Should().HaveSetter();
                var propertyMemberHasNoSetter = PropertyMembers().That().Are(propertyMember).Should().NotHaveSetter();
                var propertyMembersWithSetterDoNotIncludeMember =
                    PropertyMembers().That().HaveSetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutSetterDoNotIncludeMember =
                    PropertyMembers().That().HaveNoSetter().Should().NotBe(propertyMember).AndShould().Exist();

                Assert.Equal(propertyMember.SetterVisibility != NotAccessible,
                    propertyMemberHasSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMemberHasNoSetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMembersWithSetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != NotAccessible,
                    propertyMembersWithoutSetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithSetterHaveSetter = PropertyMembers().That().HaveSetter().Should().HaveSetter();
            var propertyMembersWithSetterHaveNoSetter =
                PropertyMembers().That().HaveSetter().Should().NotHaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveSetter =
                PropertyMembers().That().HaveNoSetter().Should().HaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveNoSetter =
                PropertyMembers().That().HaveNoSetter().Should().NotHaveSetter();

            Assert.True(propertyMembersWithSetterHaveSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithSetterHaveNoSetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutSetterHaveSetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutSetterHaveNoSetter.HasNoViolations(Architecture));
        }
    }
}