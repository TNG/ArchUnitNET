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

                Assert.Equal(propertyMember.IsVirtual, propertyMemberIsVirtual.Check(Architecture));
                Assert.Equal(!propertyMember.IsVirtual, propertyMemberIsNotVirtual.Check(Architecture));
                Assert.Equal(!propertyMember.IsVirtual, virtualPropertyMembersDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.IsVirtual, notVirtualPropertyMembersDoNotIncludeMember.Check(Architecture));
            }

            var virtualPropertyMembersShouldBeVirtual = PropertyMembers().That().AreVirtual().Should().BeVirtual();
            var virtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreVirtual().Should().NotBeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersShouldBeVirtual =
                PropertyMembers().That().AreNotVirtual().Should().BeVirtual().AndShould().Exist();
            var notVirtualPropertyMembersAreNotVirtual =
                PropertyMembers().That().AreNotVirtual().Should().NotBeVirtual();

            Assert.True(virtualPropertyMembersShouldBeVirtual.Check(Architecture));
            Assert.False(virtualPropertyMembersAreNotVirtual.Check(Architecture));
            Assert.False(notVirtualPropertyMembersShouldBeVirtual.Check(Architecture));
            Assert.True(notVirtualPropertyMembersAreNotVirtual.Check(Architecture));
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
                    propertyMemberHasGetter.Check(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMemberHasNoGetter.Check(Architecture));
                Assert.Equal(propertyMember.Visibility == NotAccessible,
                    propertyMembersWithGetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.Visibility != NotAccessible,
                    propertyMembersWithoutGetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithGetterHaveGetter = PropertyMembers().That().HaveGetter().Should().HaveGetter();
            var propertyMembersWithGetterHaveNoGetter =
                PropertyMembers().That().HaveGetter().Should().NotHaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveGetter =
                PropertyMembers().That().HaveNoGetter().Should().HaveGetter().AndShould().Exist();
            var propertyMembersWithoutGetterHaveNoGetter =
                PropertyMembers().That().HaveNoGetter().Should().NotHaveGetter();

            Assert.True(propertyMembersWithGetterHaveGetter.Check(Architecture));
            Assert.False(propertyMembersWithGetterHaveNoGetter.Check(Architecture));
            Assert.False(propertyMembersWithoutGetterHaveGetter.Check(Architecture));
            Assert.True(propertyMembersWithoutGetterHaveNoGetter.Check(Architecture));
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
                    propertyMemberHasInternalSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMemberDoesNotHaveInternalSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Internal,
                    propertyMembersWithInternalSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Internal,
                    propertyMembersWithoutInternalSetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithInternalSetterHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().HaveInternalSetter();
            var propertyMembersWithInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().HaveInternalSetter().Should().NotHaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().HaveInternalSetter().AndShould().Exist();
            var propertyMembersWithoutInternalSetterDoNotHaveInternalSetter =
                PropertyMembers().That().DoNotHaveInternalSetter().Should().NotHaveInternalSetter();

            Assert.True(propertyMembersWithInternalSetterHaveInternalSetter.Check(Architecture));
            Assert.False(propertyMembersWithInternalSetterDoNotHaveInternalSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutInternalSetterHaveInternalSetter.Check(Architecture));
            Assert.True(propertyMembersWithoutInternalSetterDoNotHaveInternalSetter.Check(Architecture));
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
                    propertyMemberHasPrivateProtectedSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMemberDoesNotHavePrivateProtectedSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != PrivateProtected,
                    propertyMembersWithPrivateProtectedSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == PrivateProtected,
                    propertyMembersWithoutPrivateProtectedSetterDoNotIncludeMember.Check(Architecture));
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

            Assert.True(propertyMembersWithPrivateProtectedSetterHavePrivateProtectedSetter.Check(Architecture));
            Assert.False(propertyMembersWithPrivateProtectedSetterDoNotHavePrivateProtectedSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutPrivateProtectedSetterHavePrivateProtectedSetter.Check(Architecture));
            Assert.True(
                propertyMembersWithoutPrivateProtectedSetterDoNotHavePrivateProtectedSetter.Check(Architecture));
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
                    propertyMemberHasPrivateSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMemberDoesNotHavePrivateSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Private,
                    propertyMembersWithPrivateSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Private,
                    propertyMembersWithoutPrivateSetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithPrivateSetterHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().HavePrivateSetter();
            var propertyMembersWithPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().HavePrivateSetter().Should().NotHavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().HavePrivateSetter().AndShould().Exist();
            var propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter =
                PropertyMembers().That().DoNotHavePrivateSetter().Should().NotHavePrivateSetter();

            Assert.True(propertyMembersWithPrivateSetterHavePrivateSetter.Check(Architecture));
            Assert.False(propertyMembersWithPrivateSetterDoNotHavePrivateSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutPrivateSetterHavePrivateSetter.Check(Architecture));
            Assert.True(propertyMembersWithoutPrivateSetterDoNotHavePrivateSetter.Check(Architecture));
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
                    propertyMemberHasProtectedInternalSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMemberDoesNotHaveProtectedInternalSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != ProtectedInternal,
                    propertyMembersWithProtectedInternalSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == ProtectedInternal,
                    propertyMembersWithoutProtectedInternalSetterDoNotIncludeMember.Check(Architecture));
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

            Assert.True(propertyMembersWithProtectedInternalSetterHaveProtectedInternalSetter.Check(Architecture));
            Assert.False(
                propertyMembersWithProtectedInternalSetterDoNotHaveProtectedInternalSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutProtectedInternalSetterHaveProtectedInternalSetter.Check(Architecture));
            Assert.True(
                propertyMembersWithoutProtectedInternalSetterDoNotHaveProtectedInternalSetter.Check(Architecture));
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
                    propertyMemberHasProtectedSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMemberDoesNotHaveProtectedSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Protected,
                    propertyMembersWithProtectedSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Protected,
                    propertyMembersWithoutProtectedSetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().HaveProtectedSetter();
            var propertyMembersWithProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().HaveProtectedSetter().Should().NotHaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().HaveProtectedSetter().AndShould().Exist();
            var propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter =
                PropertyMembers().That().DoNotHaveProtectedSetter().Should().NotHaveProtectedSetter();

            Assert.True(propertyMembersWithProtectedSetterHaveProtectedSetter.Check(Architecture));
            Assert.False(propertyMembersWithProtectedSetterDoNotHaveProtectedSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutProtectedSetterHaveProtectedSetter.Check(Architecture));
            Assert.True(propertyMembersWithoutProtectedSetterDoNotHaveProtectedSetter.Check(Architecture));
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
                    propertyMemberHasPublicSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMemberDoesNotHavePublicSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != Public,
                    propertyMembersWithPublicSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == Public,
                    propertyMembersWithoutPublicSetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithPublicSetterHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().HavePublicSetter();
            var propertyMembersWithPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().HavePublicSetter().Should().NotHavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().HavePublicSetter().AndShould().Exist();
            var propertyMembersWithoutPublicSetterDoNotHavePublicSetter =
                PropertyMembers().That().DoNotHavePublicSetter().Should().NotHavePublicSetter();

            Assert.True(propertyMembersWithPublicSetterHavePublicSetter.Check(Architecture));
            Assert.False(propertyMembersWithPublicSetterDoNotHavePublicSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutPublicSetterHavePublicSetter.Check(Architecture));
            Assert.True(propertyMembersWithoutPublicSetterDoNotHavePublicSetter.Check(Architecture));
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
                    propertyMemberHasSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMemberHasNoSetter.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility == NotAccessible,
                    propertyMembersWithSetterDoNotIncludeMember.Check(Architecture));
                Assert.Equal(propertyMember.SetterVisibility != NotAccessible,
                    propertyMembersWithoutSetterDoNotIncludeMember.Check(Architecture));
            }

            var propertyMembersWithSetterHaveSetter = PropertyMembers().That().HaveSetter().Should().HaveSetter();
            var propertyMembersWithSetterHaveNoSetter =
                PropertyMembers().That().HaveSetter().Should().NotHaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveSetter =
                PropertyMembers().That().HaveNoSetter().Should().HaveSetter().AndShould().Exist();
            var propertyMembersWithoutSetterHaveNoSetter =
                PropertyMembers().That().HaveNoSetter().Should().NotHaveSetter();

            Assert.True(propertyMembersWithSetterHaveSetter.Check(Architecture));
            Assert.False(propertyMembersWithSetterHaveNoSetter.Check(Architecture));
            Assert.False(propertyMembersWithoutSetterHaveSetter.Check(Architecture));
            Assert.True(propertyMembersWithoutSetterHaveNoSetter.Check(Architecture));
        }
    }
}