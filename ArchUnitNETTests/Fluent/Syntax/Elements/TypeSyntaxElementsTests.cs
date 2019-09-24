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

                Assert.Equal(type.IsNested, typeIsNested.Check(Architecture));
                Assert.Equal(!type.IsNested, typeIsNotNested.Check(Architecture));
                Assert.Equal(!type.IsNested, nestedTypesDoNotIncludeType.Check(Architecture));
                Assert.Equal(type.IsNested, notNestedTypesDoNotIncludeType.Check(Architecture));
            }

            var nestedTypesAreNested = Types().That().AreNested().Should().BeNested();
            var nestedTypesAreNotNested = Types().That().AreNested().Should().NotBeNested();
            var notNestedTypesAreNested = Types().That().AreNotNested().Should().BeNested();
            var notNestedTypesAreNotNested = Types().That().AreNotNested().Should().NotBeNested();

            Assert.True(nestedTypesAreNested.Check(Architecture));
            Assert.False(nestedTypesAreNotNested.Check(Architecture));
            Assert.False(notNestedTypesAreNested.Check(Architecture));
            Assert.True(notNestedTypesAreNotNested.Check(Architecture));
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

            Assert.True(publicTestClassIsPublic.Check(Architecture));
            Assert.False(publicTestClassIsNotPublic.Check(Architecture));
            Assert.True(notPublicTypesAreNotPublicTestClass.Check(Architecture));
            Assert.False(publicTypesAreNotPublicTestClass.Check(Architecture));


            //Tests with multiple arguments

            var publicTestClassAndInternalTestClassIsPublicOrInternal = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic().OrShould().BeInternal();
            var publicTestClassAndInternalTestClassIsPublic = Types().That()
                .Are(typeof(PublicTestClass), typeof(InternalTestClass)).Should().BePublic();
            var notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass = Types().That().AreNotPublic()
                .And().AreNotInternal().Should().NotBe(typeof(PublicTestClass), typeof(InternalTestClass));
            var internalTypesAreNotPublicTestClassOrInternalTestClass = Types().That().AreInternal().Should()
                .NotBe(typeof(PublicTestClass), typeof(InternalTestClass));

            Assert.True(publicTestClassAndInternalTestClassIsPublicOrInternal.Check(Architecture));
            Assert.False(publicTestClassAndInternalTestClassIsPublic.Check(Architecture));
            Assert.True(notPublicAndNotInternalClassesAreNotPublicTestClassOrInternalTestClass.Check(Architecture));
            Assert.False(internalTypesAreNotPublicTestClassOrInternalTestClass.Check(Architecture));
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

                    Assert.True(typeHasFieldMember.Check(Architecture));
                    Assert.False(typeDoesNotHaveFieldMember.Check(Architecture));
                    Assert.True(typesWithFieldMemberShouldExist.Check(Architecture));
                    Assert.False(typesWithFieldMemberShouldBeOtherTypes.Check(Architecture));
                }
            }

            foreach (var fieldMember in Architecture.FieldMembers)
            {
                var typesWithFieldMemberShouldHaveFieldMember = Types().That()
                    .HaveFieldMemberWithName(fieldMember.Name).Should()
                    .HaveFieldMemberWithName(fieldMember.Name);
                var typesWithFieldMemberExist =
                    Types().That().HaveFieldMemberWithName(fieldMember.Name).Should().Exist();

                Assert.True(typesWithFieldMemberShouldHaveFieldMember.Check(Architecture));
                Assert.True(typesWithFieldMemberExist.Check(Architecture));
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

                    Assert.True(typeHasMember.Check(Architecture));
                    Assert.False(typeDoesNotHaveMember.Check(Architecture));
                    Assert.True(typesWithMemberShouldExist.Check(Architecture));
                    Assert.False(typesWithMemberShouldBeOtherTypes.Check(Architecture));
                }
            }

            foreach (var member in Architecture.Members)
            {
                var typesWithMemberShouldHaveMember = Types().That()
                    .HaveMemberWithName(member.Name).Should()
                    .HaveMemberWithName(member.Name);
                var typesWithMemberExist =
                    Types().That().HaveMemberWithName(member.Name).Should().Exist();

                Assert.True(typesWithMemberShouldHaveMember.Check(Architecture));
                Assert.True(typesWithMemberExist.Check(Architecture));
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

                    Assert.True(typeHasMethodMember.Check(Architecture));
                    Assert.False(typeDoesNotHaveMethodMember.Check(Architecture));
                    Assert.True(typesWithMethodMemberShouldExist.Check(Architecture));
                    Assert.False(typesWithMethodMemberShouldBeOtherTypes.Check(Architecture));
                }
            }

            foreach (var methodMember in Architecture.MethodMembers)
            {
                var typesWithMethodMemberShouldHaveMethodMember = Types().That()
                    .HaveMethodMemberWithName(methodMember.Name).Should()
                    .HaveMethodMemberWithName(methodMember.Name);
                var typesWithMethodMemberExist =
                    Types().That().HaveMethodMemberWithName(methodMember.Name).Should().Exist();

                Assert.True(typesWithMethodMemberShouldHaveMethodMember.Check(Architecture));
                Assert.True(typesWithMethodMemberExist.Check(Architecture));
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

                    Assert.True(typeHasPropertyMember.Check(Architecture));
                    Assert.False(typeDoesNotHavePropertyMember.Check(Architecture));
                    Assert.True(typesWithPropertyMemberShouldExist.Check(Architecture));
                    Assert.False(typesWithPropertyMemberShouldBeOtherTypes.Check(Architecture));
                }
            }

            foreach (var propertyMember in Architecture.PropertyMembers)
            {
                var typesWithPropertyMemberShouldHavePropertyMember = Types().That()
                    .HavePropertyMemberWithName(propertyMember.Name).Should()
                    .HavePropertyMemberWithName(propertyMember.Name);
                var typesWithPropertyMemberExist =
                    Types().That().HavePropertyMemberWithName(propertyMember.Name).Should().Exist();

                Assert.True(typesWithPropertyMemberShouldHavePropertyMember.Check(Architecture));
                Assert.True(typesWithPropertyMemberExist.Check(Architecture));
            }
        }

        [Fact]
        public void ImplementInterfacesStartingWithSameNameTest()
        {
            var interfaceImplementsWrongInterface =
                Interfaces().That().Are(InheritedFromTestInterface12).Should().ImplementInterface(TestInterface1);

            Assert.False(interfaceImplementsWrongInterface.Check(Architecture));
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

                Assert.True(typesThatImplementInterfaceImplementInterface.Check(Architecture));
                Assert.False(typesThatImplementInterfaceDoNotImplementInterface.Check(Architecture));
                Assert.False(typesThatDoNotImplementInterfaceImplementInterface.Check(Architecture));
                Assert.True(typesThatDoNotImplementInterfaceDoNotImplementInterface.Check(Architecture));
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

            Assert.True(testClassThatImplementsInterfaceImplementsInterface.Check(Architecture));
            Assert.True(testClassThatImplementsOtherInterfaceImplementsInterfaces.Check(Architecture));
            Assert.True(testInterfaceThatImplementsInterfaceImplementsInterface.Check(Architecture));
            Assert.True(testClassThatImplementsNoInterfaceDoesNotImplementInterface.Check(Architecture));
            Assert.False(testClassThatImplementsNoInterfaceImplementsInterface.Check(Architecture));
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

                Assert.True(typeResidesInOwnNamespace.Check(Architecture));
                Assert.False(typeDoesNotResideInOwnNamespace.Check(Architecture));
                Assert.True(thereAreTypesInOwnNamespace.Check(Architecture));
                Assert.True(typesInOtherNamespaceAreOtherTypes.Check(Architecture));
            }

            foreach (var namespc in Architecture.Namespaces.Select(namespc => namespc.FullName))
            {
                var typesInNamespaceAreInNamespace =
                    Types().That().ResideInNamespace(namespc).Should().ResideInNamespace(namespc);
                var typesInOtherNamespaceAreInOtherNamespace = Types().That().DoNotResideInNamespace(namespc).Should()
                    .NotResideInNamespace(namespc);

                Assert.True(typesInNamespaceAreInNamespace.Check(Architecture));
                Assert.True(typesInOtherNamespaceAreInOtherNamespace.Check(Architecture));
            }
        }

        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            Assert.True(typesThatAreNotNestedMustBeVisible.Check(Architecture));
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = Types().That().ArePrivate().Or()
                .AreProtected().Or().ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            Assert.True(typesWithRestrictedVisibilityMustBeNested.Check(Architecture));
        }
    }
}