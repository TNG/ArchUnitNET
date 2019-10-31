//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

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

                Assert.Equal(propertyMember.GetterVisibility != NotAccessible,
                    propertyMemberHasGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == NotAccessible,
                    propertyMemberHasNoGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == NotAccessible,
                    propertyMembersWithGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != NotAccessible,
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
        public void HaveInternalGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasInternalGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveInternalGetter();
                var propertyMemberDoesNotHaveInternalGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveInternalGetter();
                var propertyMembersWithInternalGetterDoNotIncludeMember =
                    PropertyMembers().That().HaveInternalGetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutInternalGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveInternalGetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == Internal,
                    propertyMemberHasInternalGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Internal,
                    propertyMemberDoesNotHaveInternalGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Internal,
                    propertyMembersWithInternalGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == Internal,
                    propertyMembersWithoutInternalGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithInternalGetterHaveInternalGetter =
                PropertyMembers().That().HaveInternalGetter().Should().HaveInternalGetter();
            var propertyMembersWithInternalGetterDoNotHaveInternalGetter =
                PropertyMembers().That().HaveInternalGetter().Should().NotHaveInternalGetter().AndShould().Exist();
            var propertyMembersWithoutInternalGetterHaveInternalGetter =
                PropertyMembers().That().DoNotHaveInternalGetter().Should().HaveInternalGetter().AndShould().Exist();
            var propertyMembersWithoutInternalGetterDoNotHaveInternalGetter =
                PropertyMembers().That().DoNotHaveInternalGetter().Should().NotHaveInternalGetter();

            Assert.True(propertyMembersWithInternalGetterHaveInternalGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithInternalGetterDoNotHaveInternalGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutInternalGetterHaveInternalGetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutInternalGetterDoNotHaveInternalGetter.HasNoViolations(Architecture));
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
        public void HavePrivateGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPrivateGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePrivateGetter();
                var propertyMemberDoesNotHavePrivateGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePrivateGetter();
                var propertyMembersWithPrivateGetterDoNotIncludeMember =
                    PropertyMembers().That().HavePrivateGetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutPrivateGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePrivateGetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == Private,
                    propertyMemberHasPrivateGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Private,
                    propertyMemberDoesNotHavePrivateGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Private,
                    propertyMembersWithPrivateGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == Private,
                    propertyMembersWithoutPrivateGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPrivateGetterHavePrivateGetter =
                PropertyMembers().That().HavePrivateGetter().Should().HavePrivateGetter();
            var propertyMembersWithPrivateGetterDoNotHavePrivateGetter =
                PropertyMembers().That().HavePrivateGetter().Should().NotHavePrivateGetter().AndShould().Exist();
            var propertyMembersWithoutPrivateGetterHavePrivateGetter =
                PropertyMembers().That().DoNotHavePrivateGetter().Should().HavePrivateGetter().AndShould().Exist();
            var propertyMembersWithoutPrivateGetterDoNotHavePrivateGetter =
                PropertyMembers().That().DoNotHavePrivateGetter().Should().NotHavePrivateGetter();

            Assert.True(propertyMembersWithPrivateGetterHavePrivateGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithPrivateGetterDoNotHavePrivateGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutPrivateGetterHavePrivateGetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutPrivateGetterDoNotHavePrivateGetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HavePrivateProtectedGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPrivateProtectedGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePrivateProtectedGetter();
                var propertyMemberDoesNotHavePrivateProtectedGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePrivateProtectedGetter();
                var propertyMembersWithPrivateProtectedGetterDoNotIncludeMember =
                    PropertyMembers().That().HavePrivateProtectedGetter().Should().NotBe(propertyMember).OrShould()
                        .NotExist();
                var propertyMembersWithoutPrivateProtectedGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePrivateProtectedGetter().Should().NotBe(propertyMember)
                        .AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == PrivateProtected,
                    propertyMemberHasPrivateProtectedGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != PrivateProtected,
                    propertyMemberDoesNotHavePrivateProtectedGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != PrivateProtected,
                    propertyMembersWithPrivateProtectedGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == PrivateProtected,
                    propertyMembersWithoutPrivateProtectedGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPrivateProtectedGetterHavePrivateProtectedGetter =
                PropertyMembers().That().HavePrivateProtectedGetter().Should().HavePrivateProtectedGetter();
            var propertyMembersWithPrivateProtectedGetterDoNotHavePrivateProtectedGetter =
                PropertyMembers().That().HavePrivateProtectedGetter().Should().NotHavePrivateProtectedGetter()
                    .AndShould().Exist();
            var propertyMembersWithoutPrivateProtectedGetterHavePrivateProtectedGetter =
                PropertyMembers().That().DoNotHavePrivateProtectedGetter().Should().HavePrivateProtectedGetter()
                    .AndShould().Exist();
            var propertyMembersWithoutPrivateProtectedGetterDoNotHavePrivateProtectedGetter =
                PropertyMembers().That().DoNotHavePrivateProtectedGetter().Should().NotHavePrivateProtectedGetter();

            Assert.True(
                propertyMembersWithPrivateProtectedGetterHavePrivateProtectedGetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithPrivateProtectedGetterDoNotHavePrivateProtectedGetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithoutPrivateProtectedGetterHavePrivateProtectedGetter.HasNoViolations(Architecture));
            Assert.True(
                propertyMembersWithoutPrivateProtectedGetterDoNotHavePrivateProtectedGetter
                    .HasNoViolations(Architecture));
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
        public void HaveProtectedGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasProtectedGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveProtectedGetter();
                var propertyMemberDoesNotHaveProtectedGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveProtectedGetter();
                var propertyMembersWithProtectedGetterDoNotIncludeMember =
                    PropertyMembers().That().HaveProtectedGetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutProtectedGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveProtectedGetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == Protected,
                    propertyMemberHasProtectedGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Protected,
                    propertyMemberDoesNotHaveProtectedGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Protected,
                    propertyMembersWithProtectedGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == Protected,
                    propertyMembersWithoutProtectedGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithProtectedGetterHaveProtectedGetter =
                PropertyMembers().That().HaveProtectedGetter().Should().HaveProtectedGetter();
            var propertyMembersWithProtectedGetterDoNotHaveProtectedGetter =
                PropertyMembers().That().HaveProtectedGetter().Should().NotHaveProtectedGetter().AndShould().Exist();
            var propertyMembersWithoutProtectedGetterHaveProtectedGetter =
                PropertyMembers().That().DoNotHaveProtectedGetter().Should().HaveProtectedGetter().AndShould().Exist();
            var propertyMembersWithoutProtectedGetterDoNotHaveProtectedGetter =
                PropertyMembers().That().DoNotHaveProtectedGetter().Should().NotHaveProtectedGetter();

            Assert.True(propertyMembersWithProtectedGetterHaveProtectedGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithProtectedGetterDoNotHaveProtectedGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutProtectedGetterHaveProtectedGetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutProtectedGetterDoNotHaveProtectedGetter.HasNoViolations(Architecture));
        }

        [Fact]
        public void HaveProtectedInternalGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasProtectedInternalGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HaveProtectedInternalGetter();
                var propertyMemberDoesNotHaveProtectedInternalGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHaveProtectedInternalGetter();
                var propertyMembersWithProtectedInternalGetterDoNotIncludeMember =
                    PropertyMembers().That().HaveProtectedInternalGetter().Should().NotBe(propertyMember).OrShould()
                        .NotExist();
                var propertyMembersWithoutProtectedInternalGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHaveProtectedInternalGetter().Should().NotBe(propertyMember)
                        .AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == ProtectedInternal,
                    propertyMemberHasProtectedInternalGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != ProtectedInternal,
                    propertyMemberDoesNotHaveProtectedInternalGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != ProtectedInternal,
                    propertyMembersWithProtectedInternalGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == ProtectedInternal,
                    propertyMembersWithoutProtectedInternalGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithProtectedInternalGetterHaveProtectedInternalGetter =
                PropertyMembers().That().HaveProtectedInternalGetter().Should().HaveProtectedInternalGetter();
            var propertyMembersWithProtectedInternalGetterDoNotHaveProtectedInternalGetter =
                PropertyMembers().That().HaveProtectedInternalGetter().Should().NotHaveProtectedInternalGetter()
                    .AndShould().Exist();
            var propertyMembersWithoutProtectedInternalGetterHaveProtectedInternalGetter =
                PropertyMembers().That().DoNotHaveProtectedInternalGetter().Should().HaveProtectedInternalGetter()
                    .AndShould().Exist();
            var propertyMembersWithoutProtectedInternalGetterDoNotHaveProtectedInternalGetter =
                PropertyMembers().That().DoNotHaveProtectedInternalGetter().Should().NotHaveProtectedInternalGetter();

            Assert.True(
                propertyMembersWithProtectedInternalGetterHaveProtectedInternalGetter.HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithProtectedInternalGetterDoNotHaveProtectedInternalGetter
                    .HasNoViolations(Architecture));
            Assert.False(
                propertyMembersWithoutProtectedInternalGetterHaveProtectedInternalGetter.HasNoViolations(Architecture));
            Assert.True(
                propertyMembersWithoutProtectedInternalGetterDoNotHaveProtectedInternalGetter.HasNoViolations(
                    Architecture));
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
        public void HavePublicGetterTest()
        {
            foreach (var propertyMember in _propertyMembers)
            {
                var propertyMemberHasPublicGetter =
                    PropertyMembers().That().Are(propertyMember).Should().HavePublicGetter();
                var propertyMemberDoesNotHavePublicGetter =
                    PropertyMembers().That().Are(propertyMember).Should().NotHavePublicGetter();
                var propertyMembersWithPublicGetterDoNotIncludeMember =
                    PropertyMembers().That().HavePublicGetter().Should().NotBe(propertyMember).OrShould().NotExist();
                var propertyMembersWithoutPublicGetterDoNotIncludeMember =
                    PropertyMembers().That().DoNotHavePublicGetter().Should().NotBe(propertyMember).AndShould()
                        .Exist();

                Assert.Equal(propertyMember.GetterVisibility == Public,
                    propertyMemberHasPublicGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Public,
                    propertyMemberDoesNotHavePublicGetter.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility != Public,
                    propertyMembersWithPublicGetterDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(propertyMember.GetterVisibility == Public,
                    propertyMembersWithoutPublicGetterDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var propertyMembersWithPublicGetterHavePublicGetter =
                PropertyMembers().That().HavePublicGetter().Should().HavePublicGetter();
            var propertyMembersWithPublicGetterDoNotHavePublicGetter =
                PropertyMembers().That().HavePublicGetter().Should().NotHavePublicGetter().AndShould().Exist();
            var propertyMembersWithoutPublicGetterHavePublicGetter =
                PropertyMembers().That().DoNotHavePublicGetter().Should().HavePublicGetter().AndShould().Exist();
            var propertyMembersWithoutPublicGetterDoNotHavePublicGetter =
                PropertyMembers().That().DoNotHavePublicGetter().Should().NotHavePublicGetter();

            Assert.True(propertyMembersWithPublicGetterHavePublicGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithPublicGetterDoNotHavePublicGetter.HasNoViolations(Architecture));
            Assert.False(propertyMembersWithoutPublicGetterHavePublicGetter.HasNoViolations(Architecture));
            Assert.True(propertyMembersWithoutPublicGetterDoNotHavePublicGetter.HasNoViolations(Architecture));
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