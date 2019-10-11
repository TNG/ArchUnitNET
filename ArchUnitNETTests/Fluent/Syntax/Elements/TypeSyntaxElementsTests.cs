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

                Assert.Equal(type.IsNested, typeIsNested.HasNoViolations(Architecture));
                Assert.Equal(!type.IsNested, typeIsNotNested.HasNoViolations(Architecture));
                Assert.Equal(!type.IsNested, nestedTypesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(type.IsNested, notNestedTypesDoNotIncludeType.HasNoViolations(Architecture));
            }

            var nestedTypesAreNested = Types().That().AreNested().Should().BeNested();
            var nestedTypesAreNotNested = Types().That().AreNested().Should().NotBeNested();
            var notNestedTypesAreNested = Types().That().AreNotNested().Should().BeNested();
            var notNestedTypesAreNotNested = Types().That().AreNotNested().Should().NotBeNested();

            Assert.True(nestedTypesAreNested.HasNoViolations(Architecture));
            Assert.False(nestedTypesAreNotNested.HasNoViolations(Architecture));
            Assert.False(notNestedTypesAreNested.HasNoViolations(Architecture));
            Assert.True(notNestedTypesAreNotNested.HasNoViolations(Architecture));
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

            Assert.True(publicTestClassIsPublic.HasNoViolations(Architecture));
            Assert.False(publicTestClassIsNotPublic.HasNoViolations(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClass.HasNoViolations(Architecture));
            Assert.False(publicTypesAreNotPublicTestClass.HasNoViolations(Architecture));


            //Tests with multiple arguments

            var publicTestClassAndInternalTestClassIsPublicOrInternal = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic().OrShould().BeInternal();
            var publicTestClassAndInternalTestClassIsPublic = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types().That().AreNotPublic()
                .And().AreNotInternal().Should().NotBe(typeof(PublicTestClass), typeof(InternalTestClass));
            var internalTypesAreNotPublicTestClassOrInternalTestClass = Types().That().AreInternal().Should()
                .NotBe(typeof(PublicTestClass), typeof(InternalTestClass));

            Assert.True(publicTestClassAndInternalTestClassIsPublicOrInternal.HasNoViolations(Architecture));
            Assert.False(publicTestClassAndInternalTestClassIsPublic.HasNoViolations(Architecture));
            Assert.True(
                notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(Architecture));
            Assert.False(internalTypesAreNotPublicTestClassOrInternalTestClass.HasNoViolations(Architecture));
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

                    Assert.True(typeHasFieldMember.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotHaveFieldMember.HasNoViolations(Architecture));
                    Assert.True(typesWithFieldMemberShouldExist.HasNoViolations(Architecture));
                    Assert.False(typesWithFieldMemberShouldBeOtherTypes.HasNoViolations(Architecture));
                }
            }

            foreach (var fieldMember in Architecture.FieldMembers)
            {
                var typesWithFieldMemberShouldHaveFieldMember = Types().That()
                    .HaveFieldMemberWithName(fieldMember.Name).Should()
                    .HaveFieldMemberWithName(fieldMember.Name);
                var typesWithFieldMemberExist =
                    Types().That().HaveFieldMemberWithName(fieldMember.Name).Should().Exist();

                Assert.True(typesWithFieldMemberShouldHaveFieldMember.HasNoViolations(Architecture));
                Assert.True(typesWithFieldMemberExist.HasNoViolations(Architecture));
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

                    Assert.True(typeHasMember.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotHaveMember.HasNoViolations(Architecture));
                    Assert.True(typesWithMemberShouldExist.HasNoViolations(Architecture));
                    Assert.False(typesWithMemberShouldBeOtherTypes.HasNoViolations(Architecture));
                }
            }

            foreach (var member in Architecture.Members)
            {
                var typesWithMemberShouldHaveMember = Types().That()
                    .HaveMemberWithName(member.Name).Should()
                    .HaveMemberWithName(member.Name);
                var typesWithMemberExist =
                    Types().That().HaveMemberWithName(member.Name).Should().Exist();

                Assert.True(typesWithMemberShouldHaveMember.HasNoViolations(Architecture));
                Assert.True(typesWithMemberExist.HasNoViolations(Architecture));
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

                    Assert.True(typeHasMethodMember.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotHaveMethodMember.HasNoViolations(Architecture));
                    Assert.True(typesWithMethodMemberShouldExist.HasNoViolations(Architecture));
                    Assert.False(typesWithMethodMemberShouldBeOtherTypes.HasNoViolations(Architecture));
                }
            }

            foreach (var methodMember in Architecture.MethodMembers)
            {
                var typesWithMethodMemberShouldHaveMethodMember = Types().That()
                    .HaveMethodMemberWithName(methodMember.Name).Should()
                    .HaveMethodMemberWithName(methodMember.Name);
                var typesWithMethodMemberExist =
                    Types().That().HaveMethodMemberWithName(methodMember.Name).Should().Exist();

                Assert.True(typesWithMethodMemberShouldHaveMethodMember.HasNoViolations(Architecture));
                Assert.True(typesWithMethodMemberExist.HasNoViolations(Architecture));
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

                    Assert.True(typeHasPropertyMember.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotHavePropertyMember.HasNoViolations(Architecture));
                    Assert.True(typesWithPropertyMemberShouldExist.HasNoViolations(Architecture));
                    Assert.False(typesWithPropertyMemberShouldBeOtherTypes.HasNoViolations(Architecture));
                }
            }

            foreach (var propertyMember in Architecture.PropertyMembers)
            {
                var typesWithPropertyMemberShouldHavePropertyMember = Types().That()
                    .HavePropertyMemberWithName(propertyMember.Name).Should()
                    .HavePropertyMemberWithName(propertyMember.Name);
                var typesWithPropertyMemberExist =
                    Types().That().HavePropertyMemberWithName(propertyMember.Name).Should().Exist();

                Assert.True(typesWithPropertyMemberShouldHavePropertyMember.HasNoViolations(Architecture));
                Assert.True(typesWithPropertyMemberExist.HasNoViolations(Architecture));
            }
        }


        [Fact]
        public void ImplementInterfaceTest()
        {
            foreach (var intf in Architecture.Interfaces)
            {
                var typesThatImplementInterfaceImplementInterface = Types().That()
                    .ImplementInterface(intf.FullName)
                    .Should().ImplementInterface(intf.FullName);
                var typesThatImplementInterfaceDoNotImplementInterface = Types().That()
                    .ImplementInterface(intf.FullName).Should()
                    .NotImplementInterface(intf.FullName).AndShould().Exist();
                var typesThatDoNotImplementInterfaceImplementInterface = Types().That()
                    .DoNotImplementInterface(intf.FullName)
                    .Should().ImplementInterface(intf.FullName).AndShould().Exist();
                var typesThatDoNotImplementInterfaceDoNotImplementInterface = Types().That()
                    .DoNotImplementInterface(intf.FullName).Should()
                    .NotImplementInterface(intf.FullName);

                Assert.True(typesThatImplementInterfaceImplementInterface.HasNoViolations(Architecture));
                Assert.False(typesThatImplementInterfaceDoNotImplementInterface.HasNoViolations(Architecture));
                Assert.False(typesThatDoNotImplementInterfaceImplementInterface.HasNoViolations(Architecture));
                Assert.True(typesThatDoNotImplementInterfaceDoNotImplementInterface.HasNoViolations(Architecture));
            }

            var testClassThatImplementsInterfaceImplementsInterface = Classes().That()
                .Are(StaticTestTypes.InheritedType).Should()
                .ImplementInterface(InheritedTestInterface.FullName);
            var testClassThatImplementsOtherInterfaceImplementsInterfaces = Types().That()
                .Are(StaticTestTypes.InheritedType).Should()
                .ImplementInterface(InheritedTestInterface.FullName).AndShould()
                .ImplementInterface(InheritingInterface.FullName);
            var testInterfaceThatImplementsInterfaceImplementsInterface = Interfaces().That()
                .Are(InheritingInterface).Should()
                .ImplementInterface(InheritedTestInterface.FullName);
            var testClassThatImplementsNoInterfaceDoesNotImplementInterface = Interfaces().That()
                .Are(StaticTestTypes.PublicTestClass).Should()
                .NotImplementInterface(InheritedTestInterface.FullName);
            var testClassThatImplementsNoInterfaceImplementsInterface = Interfaces().That()
                .Are(StaticTestTypes.PublicTestClass).Should()
                .ImplementInterface(InheritedTestInterface.FullName).AndShould()
                .Exist();

            Assert.True(testClassThatImplementsInterfaceImplementsInterface.HasNoViolations(Architecture));
            Assert.True(testClassThatImplementsOtherInterfaceImplementsInterfaces.HasNoViolations(Architecture));
            Assert.True(testInterfaceThatImplementsInterfaceImplementsInterface.HasNoViolations(Architecture));
            Assert.True(testClassThatImplementsNoInterfaceDoesNotImplementInterface.HasNoViolations(Architecture));
            Assert.False(testClassThatImplementsNoInterfaceImplementsInterface.HasNoViolations(Architecture));
        }

        [Fact]
        public void ResideInNamespaceTest()
        {
            foreach (var type in _types)
            {
                var typeResidesInOwnNamespace =
                    Types().That().Are(type).Should().ResideInNamespace(type.Namespace.FullName);
                var typeDoesNotResideInOwnNamespace =
                    Types().That().Are(type).Should()
                        .NotResideInNamespace(type.Namespace.FullName);
                var thereAreTypesInOwnNamespace =
                    Types().That().ResideInNamespace(type.Namespace.FullName).Should().Exist();
                var typesInOtherNamespaceAreOtherTypes = Types().That()
                    .DoNotResideInNamespace(type.Namespace.FullName)
                    .Should().NotBe(type);

                Assert.True(typeResidesInOwnNamespace.HasNoViolations(Architecture));
                Assert.False(typeDoesNotResideInOwnNamespace.HasNoViolations(Architecture));
                Assert.True(thereAreTypesInOwnNamespace.HasNoViolations(Architecture));
                Assert.True(typesInOtherNamespaceAreOtherTypes.HasNoViolations(Architecture));
            }

            foreach (var namespc in Architecture.Namespaces.Select(namespc => namespc.FullName))
            {
                var typesInNamespaceAreInNamespace =
                    Types().That().ResideInNamespace(namespc).Should()
                        .ResideInNamespace(namespc);
                var typesInOtherNamespaceAreInOtherNamespace = Types().That()
                    .DoNotResideInNamespace(namespc).Should()
                    .NotResideInNamespace(namespc);

                Assert.True(typesInNamespaceAreInNamespace.HasNoViolations(Architecture));
                Assert.True(typesInOtherNamespaceAreInOtherNamespace.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            Assert.True(typesThatAreNotNestedMustBeVisible.HasNoViolations(Architecture));
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = Types().That().ArePrivate().Or()
                .AreProtected().Or().ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            Assert.True(typesWithRestrictedVisibilityMustBeNested.HasNoViolations(Architecture));
        }
    }
}