using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class TypeSyntaxElementsTests
    {
        public TypeSyntaxElementsTests()
        {
            _types = Architecture.Types;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IType> _types;

        [Fact]
        public void AreNestedTest()
        {
            foreach (var type in _types)
            {
                var typeIsNested = Types().That().Are(type).Should().BeNested();
                var typeIsNotNested = Types().That().Are(type).Should().NotBeNested();
                var nestedTypesDoNotIncludeType = Types().That().AreNested().Should().NotBe(type);
                var notNestedTypesDoNotIncludeType = Types().That().AreNotNested().Should().NotBe(type);

                Assert.Equal(type.IsNested, typeIsNested.HasViolations(Architecture));
                Assert.Equal(!type.IsNested, typeIsNotNested.HasViolations(Architecture));
                Assert.Equal(!type.IsNested, nestedTypesDoNotIncludeType.HasViolations(Architecture));
                Assert.Equal(type.IsNested, notNestedTypesDoNotIncludeType.HasViolations(Architecture));
            }

            var nestedTypesAreNested = Types().That().AreNested().Should().BeNested();
            var nestedTypesAreNotNested = Types().That().AreNested().Should().NotBeNested();
            var notNestedTypesAreNested = Types().That().AreNotNested().Should().BeNested();
            var notNestedTypesAreNotNested = Types().That().AreNotNested().Should().NotBeNested();

            Assert.True(nestedTypesAreNested.HasViolations(Architecture));
            Assert.False(nestedTypesAreNotNested.HasViolations(Architecture));
            Assert.False(notNestedTypesAreNested.HasViolations(Architecture));
            Assert.True(notNestedTypesAreNotNested.HasViolations(Architecture));
        }

        [Fact]
        public void AreTest()
        {
            //Tests with one argument

            var publicTestClassIsPublic = Types().That().Are(typeof(PublicTestClass)).Should().BePublic();
            var publicTestClassIsNotPublic = Types().That().Are(typeof(PublicTestClass)).Should().NotBePublic();
            var notPublicTypesAreNotPublicTestClass =
                Types().That().AreNotPublic().Should().NotBe(typeof(PublicTestClass));
            var publicTypesAreNotPublicTestClass = Types().That().ArePublic().Should().NotBe(typeof(PublicTestClass));

            Assert.True(publicTestClassIsPublic.HasViolations(Architecture));
            Assert.False(publicTestClassIsNotPublic.HasViolations(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClass.HasViolations(Architecture));
            Assert.False(publicTypesAreNotPublicTestClass.HasViolations(Architecture));


            //Tests with multiple arguments

            var publicTestClassAndInternalTestClassIsPublicOrInternal = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic().OrShould().BeInternal();
            var publicTestClassAndInternalTestClassIsPublic = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types().That().AreNotPublic()
                .And().AreNotInternal().Should().NotBe(typeof(PublicTestClass), typeof(InternalTestClass));
            var internalTypesAreNotPublicTestClassOrInternalTestClass = Types().That().AreInternal().Should()
                .NotBe(typeof(PublicTestClass), typeof(InternalTestClass));

            Assert.True(publicTestClassAndInternalTestClassIsPublicOrInternal.HasViolations(Architecture));
            Assert.False(publicTestClassAndInternalTestClassIsPublic.HasViolations(Architecture));
            Assert.True(
                notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.HasViolations(Architecture));
            Assert.False(internalTypesAreNotPublicTestClassOrInternalTestClass.HasViolations(Architecture));
        }

        [Fact]
        public void HaveFieldMemberWithNameTest()
        {
            foreach (var type in _types)
            {
                foreach (var fieldMember in type.GetFieldMembers())
                {
                    var typeHasFieldMember =
                        Types().That().Are(type).Should().HaveFieldMemberWithName(fieldMember.Name);
                    var typeDoesNotHaveFieldMember = Types().That().Are(type).Should()
                        .NotHaveFieldMemberWithName(fieldMember.Name);
                    var typesWithFieldMemberShouldExist =
                        Types().That().HaveFieldMemberWithName(fieldMember.Name).Should().Exist();
                    var typesWithFieldMemberShouldBeOtherTypes = Types().That()
                        .HaveFieldMemberWithName(fieldMember.Name).Should().NotBe(type);

                    Assert.True(typeHasFieldMember.HasViolations(Architecture));
                    Assert.False(typeDoesNotHaveFieldMember.HasViolations(Architecture));
                    Assert.True(typesWithFieldMemberShouldExist.HasViolations(Architecture));
                    Assert.False(typesWithFieldMemberShouldBeOtherTypes.HasViolations(Architecture));
                }
            }

            foreach (var fieldMember in Architecture.FieldMembers)
            {
                var typesWithFieldMemberShouldHaveFieldMember = Types().That()
                    .HaveFieldMemberWithName(fieldMember.Name).Should()
                    .HaveFieldMemberWithName(fieldMember.Name);
                var typesWithFieldMemberExist =
                    Types().That().HaveFieldMemberWithName(fieldMember.Name).Should().Exist();

                Assert.True(typesWithFieldMemberShouldHaveFieldMember.HasViolations(Architecture));
                Assert.True(typesWithFieldMemberExist.HasViolations(Architecture));
            }
        }

        [Fact]
        public void HaveMemberWithNameTest()
        {
            foreach (var type in _types)
            {
                foreach (var member in type.Members)
                {
                    var typeHasMember =
                        Types().That().Are(type).Should().HaveMemberWithName(member.Name);
                    var typeDoesNotHaveMember = Types().That().Are(type).Should()
                        .NotHaveMemberWithName(member.Name);
                    var typesWithMemberShouldExist =
                        Types().That().HaveMemberWithName(member.Name).Should().Exist();
                    var typesWithMemberShouldBeOtherTypes = Types().That()
                        .HaveMemberWithName(member.Name).Should().NotBe(type);

                    Assert.True(typeHasMember.HasViolations(Architecture));
                    Assert.False(typeDoesNotHaveMember.HasViolations(Architecture));
                    Assert.True(typesWithMemberShouldExist.HasViolations(Architecture));
                    Assert.False(typesWithMemberShouldBeOtherTypes.HasViolations(Architecture));
                }
            }

            foreach (var member in Architecture.Members)
            {
                var typesWithMemberShouldHaveMember = Types().That()
                    .HaveMemberWithName(member.Name).Should()
                    .HaveMemberWithName(member.Name);
                var typesWithMemberExist =
                    Types().That().HaveMemberWithName(member.Name).Should().Exist();

                Assert.True(typesWithMemberShouldHaveMember.HasViolations(Architecture));
                Assert.True(typesWithMemberExist.HasViolations(Architecture));
            }
        }


        [Fact]
        public void HaveMethodMemberWithNameTest()
        {
            foreach (var type in _types)
            {
                foreach (var methodMember in type.GetMethodMembers())
                {
                    var typeHasMethodMember =
                        Types().That().Are(type).Should().HaveMethodMemberWithName(methodMember.Name);
                    var typeDoesNotHaveMethodMember = Types().That().Are(type).Should()
                        .NotHaveMethodMemberWithName(methodMember.Name);
                    var typesWithMethodMemberShouldExist =
                        Types().That().HaveMethodMemberWithName(methodMember.Name).Should().Exist();
                    var typesWithMethodMemberShouldBeOtherTypes = Types().That()
                        .HaveMethodMemberWithName(methodMember.Name).Should().NotBe(type);

                    Assert.True(typeHasMethodMember.HasViolations(Architecture));
                    Assert.False(typeDoesNotHaveMethodMember.HasViolations(Architecture));
                    Assert.True(typesWithMethodMemberShouldExist.HasViolations(Architecture));
                    Assert.False(typesWithMethodMemberShouldBeOtherTypes.HasViolations(Architecture));
                }
            }

            foreach (var methodMember in Architecture.MethodMembers)
            {
                var typesWithMethodMemberShouldHaveMethodMember = Types().That()
                    .HaveMethodMemberWithName(methodMember.Name).Should()
                    .HaveMethodMemberWithName(methodMember.Name);
                var typesWithMethodMemberExist =
                    Types().That().HaveMethodMemberWithName(methodMember.Name).Should().Exist();

                Assert.True(typesWithMethodMemberShouldHaveMethodMember.HasViolations(Architecture));
                Assert.True(typesWithMethodMemberExist.HasViolations(Architecture));
            }
        }

        [Fact]
        public void HavePropertyMemberWithNameTest()
        {
            foreach (var type in _types)
            {
                foreach (var propertyMember in type.GetPropertyMembers())
                {
                    var typeHasPropertyMember =
                        Types().That().Are(type).Should().HavePropertyMemberWithName(propertyMember.Name);
                    var typeDoesNotHavePropertyMember = Types().That().Are(type).Should()
                        .NotHavePropertyMemberWithName(propertyMember.Name);
                    var typesWithPropertyMemberShouldExist =
                        Types().That().HavePropertyMemberWithName(propertyMember.Name).Should().Exist();
                    var typesWithPropertyMemberShouldBeOtherTypes = Types().That()
                        .HavePropertyMemberWithName(propertyMember.Name).Should().NotBe(type);

                    Assert.True(typeHasPropertyMember.HasViolations(Architecture));
                    Assert.False(typeDoesNotHavePropertyMember.HasViolations(Architecture));
                    Assert.True(typesWithPropertyMemberShouldExist.HasViolations(Architecture));
                    Assert.False(typesWithPropertyMemberShouldBeOtherTypes.HasViolations(Architecture));
                }
            }

            foreach (var propertyMember in Architecture.PropertyMembers)
            {
                var typesWithPropertyMemberShouldHavePropertyMember = Types().That()
                    .HavePropertyMemberWithName(propertyMember.Name).Should()
                    .HavePropertyMemberWithName(propertyMember.Name);
                var typesWithPropertyMemberExist =
                    Types().That().HavePropertyMemberWithName(propertyMember.Name).Should().Exist();

                Assert.True(typesWithPropertyMemberShouldHavePropertyMember.HasViolations(Architecture));
                Assert.True(typesWithPropertyMemberExist.HasViolations(Architecture));
            }
        }

        [Fact]
        public void ImplementInterfacesStartingWithSameNameTest()
        {
            var interfaceImplementsWrongInterface =
                Interfaces().That().Are(InheritedFromTestInterface12).Should().ImplementInterface(TestInterface1);

            Assert.False(interfaceImplementsWrongInterface.HasViolations(Architecture));
        }


        [Fact]
        public void ImplementInterfaceTest()
        {
            foreach (var intf in Architecture.Interfaces)
            {
                var typesThatImplementInterfaceImplementInterface = Types().That().ImplementInterface(intf)
                    .Should().ImplementInterface(intf.FullName);
                var typesThatImplementInterfaceDoNotImplementInterface = Types().That()
                    .ImplementInterface(intf).Should().NotImplementInterface(intf).AndShould().Exist();
                var typesThatDoNotImplementInterfaceImplementInterface = Types().That().DoNotImplementInterface(intf)
                    .Should().ImplementInterface(intf.FullName).AndShould().Exist();
                var typesThatDoNotImplementInterfaceDoNotImplementInterface = Types().That()
                    .DoNotImplementInterface(intf.FullName).Should().NotImplementInterface(intf);

                Assert.True(typesThatImplementInterfaceImplementInterface.HasViolations(Architecture));
                Assert.False(typesThatImplementInterfaceDoNotImplementInterface.HasViolations(Architecture));
                Assert.False(typesThatDoNotImplementInterfaceImplementInterface.HasViolations(Architecture));
                Assert.True(typesThatDoNotImplementInterfaceDoNotImplementInterface.HasViolations(Architecture));
            }

            var testClassThatImplementsInterfaceImplementsInterface = Classes().That()
                .Are(StaticTestTypes.InheritedType).Should().ImplementInterface(InheritedTestInterface);
            var testClassThatImplementsOtherInterfaceImplementsInterfaces = Types().That()
                .Are(StaticTestTypes.InheritedType).Should().ImplementInterface(InheritedTestInterface).AndShould()
                .ImplementInterface(InheritingInterface);
            var testInterfaceThatImplementsInterfaceImplementsInterface = Interfaces().That()
                .Are(InheritingInterface).Should().ImplementInterface(InheritedTestInterface);
            var testClassThatImplementsNoInterfaceDoesNotImplementInterface = Interfaces().That()
                .Are(StaticTestTypes.PublicTestClass).Should().NotImplementInterface(InheritedTestInterface);
            var testClassThatImplementsNoInterfaceImplementsInterface = Interfaces().That()
                .Are(StaticTestTypes.PublicTestClass).Should().ImplementInterface(InheritedTestInterface).AndShould()
                .Exist();

            Assert.True(testClassThatImplementsInterfaceImplementsInterface.HasViolations(Architecture));
            Assert.True(testClassThatImplementsOtherInterfaceImplementsInterfaces.HasViolations(Architecture));
            Assert.True(testInterfaceThatImplementsInterfaceImplementsInterface.HasViolations(Architecture));
            Assert.True(testClassThatImplementsNoInterfaceDoesNotImplementInterface.HasViolations(Architecture));
            Assert.False(testClassThatImplementsNoInterfaceImplementsInterface.HasViolations(Architecture));
        }

        [Fact]
        public void ResideInNamespaceTest()
        {
            foreach (var type in _types)
            {
                var typeResidesInOwnNamespace =
                    Types().That().Are(type).Should().ResideInNamespace(type.Namespace.FullName);
                var typeDoesNotResideInOwnNamespace =
                    Types().That().Are(type).Should().NotResideInNamespace(type.Namespace.FullName);
                var thereAreTypesInOwnNamespace =
                    Types().That().ResideInNamespace(type.Namespace.FullName).Should().Exist();
                var typesInOtherNamespaceAreOtherTypes = Types().That().DoNotResideInNamespace(type.Namespace.FullName)
                    .Should().NotBe(type);

                Assert.True(typeResidesInOwnNamespace.HasViolations(Architecture));
                Assert.False(typeDoesNotResideInOwnNamespace.HasViolations(Architecture));
                Assert.True(thereAreTypesInOwnNamespace.HasViolations(Architecture));
                Assert.True(typesInOtherNamespaceAreOtherTypes.HasViolations(Architecture));
            }

            foreach (var namespc in Architecture.Namespaces.Select(namespc => namespc.FullName))
            {
                var typesInNamespaceAreInNamespace =
                    Types().That().ResideInNamespace(namespc).Should().ResideInNamespace(namespc);
                var typesInOtherNamespaceAreInOtherNamespace = Types().That().DoNotResideInNamespace(namespc).Should()
                    .NotResideInNamespace(namespc);

                Assert.True(typesInNamespaceAreInNamespace.HasViolations(Architecture));
                Assert.True(typesInOtherNamespaceAreInOtherNamespace.HasViolations(Architecture));
            }
        }

        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            Assert.True(typesThatAreNotNestedMustBeVisible.HasViolations(Architecture));
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = Types().That().ArePrivate().Or()
                .AreProtected().Or().ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            Assert.True(typesWithRestrictedVisibilityMustBeNested.HasViolations(Architecture));
        }
    }
}