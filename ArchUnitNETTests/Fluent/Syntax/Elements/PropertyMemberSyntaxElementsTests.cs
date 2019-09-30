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

                Assert.Equal(propertyMember.IsVirtual, propertyMemberIsVirtual.HasViolations(Architecture));
                Assert.Equal(!propertyMember.IsVirtual, propertyMemberIsNotVirtual.HasViolations(Architecture));
                Assert.Equal(!propertyMember.IsVirtual,
                    virtualPropertyMembersDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.IsVirtual,
                    notVirtualPropertyMembersDoNotIncludeMember.HasViolations(Architecture));
            }

            var virtualPropertyMembersShouldBeVirtual = PropertyMembers().That().AreVirtual().Should().BeVirtual();
            var virtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreVirtual().Should().NotBeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersShouldBeVirtual =
                PropertyMembers().That().AreNotVirtual().Should().BeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreNotVirtual().Should().NotBeVirtual();

            Assert.True(virtualPropertyMembersShouldBeVirtual.HasViolations(Architecture));
            Assert.False(virtualPropertyMembersAreNotVirtual.HasViolations(Architecture));
            Assert.False(notVirtualPropertyMembersShouldBeVirtual.HasViolations(Architecture));
            Assert.True(notVirtualPropertyMembersAreNotVirtual.HasViolations(Architecture));
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
                    propertyMemberHasGetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMemberHasNoGetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMembersWithGetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.Visibility != NotAccessible,
                    propertyMembersWithoutGetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithGetterHaveGetter = PropertyMembers().That().HaveGetter().Should().HaveGetter();
            var propertyMembersWithGetterHaveNoGetter =
                PropertyMembers().That().HaveGetter().Should().NotHaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveGetter =
                PropertyMembers().That().HaveNoGetter().Should().HaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveNoGetter =
                PropertyMembers().That().HaveNoGetter().Should().NotHaveGetter();

            Assert.True(propertyMembersWithGetterHaveGetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithGetterHaveNoGetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutGetterHaveGetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutGetterHaveNoGetter.HasViolations(Architecture));
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
                    propertyMemberHasInternalSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMemberDoesNotHaveInternalSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMembersWithInternalSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Internal,
                    propertyMembersWithoutInternalSetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithInternalSetterHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().HaveInternalSetter();
            var propertyMembersWithInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().NotHaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().HaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().NotHaveInternalSetter();

            Assert.True(propertyMembersWithInternalSetterHaveInternalSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithInternalSetterDoNotHaveInternalSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutInternalSetterHaveInternalSetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutInternalSetterDoNotHaveInternalSetter.HasViolations(Architecture));
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
                    propertyMemberHasPrivateProtectedSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMemberDoesNotHavePrivateProtectedSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMembersWithPrivateProtectedSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == PrivateProtected,
                    propertyMembersWithoutPrivateProtectedSetterDoNotIncludeMember.HasViolations(Architecture));
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
                propertyMembersWithPrivateProtectedSetterHavePrivateProtectedSetter.HasViolations(Architecture));
            Assert.False(
                propertyMembersWithPrivateProtectedSetterDoNotHavePrivateProtectedSetter.HasViolations(Architecture));
            Assert.False(
                propertyMembersWithoutPrivateProtectedSetterHavePrivateProtectedSetter.HasViolations(Architecture));
            Assert.True(
                propertyMembersWithoutPrivateProtectedSetterDoNotHavePrivateProtectedSetter
                    .HasViolations(Architecture));
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
                    propertyMemberHasPrivateSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMemberDoesNotHavePrivateSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMembersWithPrivateSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Private,
                    propertyMembersWithoutPrivateSetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithPrivateSetterHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().HavePrivateSetter();
            var propertyMembersWithPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().NotHavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().HavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().NotHavePrivateSetter();

            Assert.True(propertyMembersWithPrivateSetterHavePrivateSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithPrivateSetterDoNotHavePrivateSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutPrivateSetterHavePrivateSetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter.HasViolations(Architecture));
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
                    propertyMemberHasProtectedInternalSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMemberDoesNotHaveProtectedInternalSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMembersWithProtectedInternalSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == ProtectedInternal,
                    propertyMembersWithoutProtectedInternalSetterDoNotIncludeMember.HasViolations(Architecture));
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
                propertyMembersWithProtectedInternalSetterHaveProtectedInternalSetter.HasViolations(Architecture));
            Assert.False(
                propertyMembersWithProtectedInternalSetterDoNotHaveProtectedInternalSetter.HasViolations(Architecture));
            Assert.False(
                propertyMembersWithoutProtectedInternalSetterHaveProtectedInternalSetter.HasViolations(Architecture));
            Assert.True(
                propertyMembersWithoutProtectedInternalSetterDoNotHaveProtectedInternalSetter.HasViolations(
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
                    propertyMemberHasProtectedSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMemberDoesNotHaveProtectedSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMembersWithProtectedSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Protected,
                    propertyMembersWithoutProtectedSetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().HaveProtectedSetter();
            var propertyMembersWithProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().NotHaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().HaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().NotHaveProtectedSetter();

            Assert.True(propertyMembersWithProtectedSetterHaveProtectedSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithProtectedSetterDoNotHaveProtectedSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutProtectedSetterHaveProtectedSetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter.HasViolations(Architecture));
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
                    propertyMemberHasPublicSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMemberDoesNotHavePublicSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMembersWithPublicSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Public,
                    propertyMembersWithoutPublicSetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithPublicSetterHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().HavePublicSetter();
            var propertyMembersWithPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().NotHavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().HavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().NotHavePublicSetter();

            Assert.True(propertyMembersWithPublicSetterHavePublicSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithPublicSetterDoNotHavePublicSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutPublicSetterHavePublicSetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutPublicSetterDoNotHavePublicSetter.HasViolations(Architecture));
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
                    propertyMemberHasSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMemberHasNoSetter.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMembersWithSetterDoNotIncludeMember.HasViolations(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != NotAccessible,
                    propertyMembersWithoutSetterDoNotIncludeMember.HasViolations(Architecture));
            }

            var propertyMembersWithSetterHaveSetter = PropertyMembers().That().HaveSetter().Should().HaveSetter();
            var propertyMembersWithSetterHaveNoSetter =
                PropertyMembers().That().HaveSetter().Should().NotHaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveSetter =
                PropertyMembers().That().HaveNoSetter().Should().HaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveNoSetter =
                PropertyMembers().That().HaveNoSetter().Should().NotHaveSetter();

            Assert.True(propertyMembersWithSetterHaveSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithSetterHaveNoSetter.HasViolations(Architecture));
            Assert.False(propertyMembersWithoutSetterHaveSetter.HasViolations(Architecture));
            Assert.True(propertyMembersWithoutSetterHaveNoSetter.HasViolations(Architecture));
        }
    }
}