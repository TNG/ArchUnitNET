//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNETTests.Domain.StaticTestTypes;
using Enum = ArchUnitNET.Domain.Enum;

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
        public void AssignableToTest()
        {
            var falseTypeList1 = new List<Type> {typeof(PublicTestClass), typeof(InternalTestClass)};
            var falseTypeList2 = new List<IType> {StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass};
            var falseTypeListPattern = new List<string>
                {StaticTestTypes.PublicTestClass.FullName, StaticTestTypes.InternalTestClass.FullName};
            foreach (var type in _types)
            {
                //One Argument

                var typeIsAssignableToItself = Types().That().Are(type).Should().BeAssignableTo(type);
                var typeIsAssignableToItselfPattern = Types().That().Are(type).Should().BeAssignableTo(type.FullName);
                var typeIsNotAssignableToItself = Types().That().Are(type).Should().NotBeAssignableTo(type);
                var typeIsNotAssignableToItselfPattern =
                    Types().That().Are(type).Should().NotBeAssignableTo(type.FullName);
                var typeIsNotAssignableToFalseType1 = Types().That().Are(type).Should()
                    .NotBeAssignableTo(typeof(PublicTestClass)).OrShould().Be(typeof(PublicTestClass));
                var typeIsNotAssignableToFalseType2 = Types().That().Are(type).Should()
                    .NotBeAssignableTo(StaticTestTypes.PublicTestClass).OrShould().Be(typeof(PublicTestClass));
                var typeIsNotAssignableToFalseTypePattern = Types().That().Are(type).Should()
                    .NotBeAssignableTo(StaticTestTypes.PublicTestClass.FullName).OrShould().Be(typeof(PublicTestClass));
                var typeIsAssignableToFalseType1 = Types().That().Are(type).Should()
                    .BeAssignableTo(typeof(PublicTestClass)).AndShould().NotBe(typeof(PublicTestClass));
                var typeIsAssignableToFalseType2 = Types().That().Are(type).Should()
                    .BeAssignableTo(StaticTestTypes.PublicTestClass).AndShould().NotBe(typeof(PublicTestClass));
                var typeIsAssignableToFalseTypePattern = Types().That().Are(type).Should()
                    .BeAssignableTo(StaticTestTypes.PublicTestClass.FullName).AndShould()
                    .NotBe(typeof(PublicTestClass));

                Assert.True(typeIsAssignableToItself.HasNoViolations(Architecture));
                Assert.True(typeIsAssignableToItselfPattern.HasNoViolations(Architecture));
                Assert.False(typeIsNotAssignableToItself.HasNoViolations(Architecture));
                Assert.False(typeIsNotAssignableToItselfPattern.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseType1.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseType2.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseTypePattern.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseType1.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseType2.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseTypePattern.HasNoViolations(Architecture));

                //Multiple Arguments

                var typeIsAssignableToItselfFluent =
                    Types().That().Are(type).Should().BeAssignableTo(Types().That().Are(type));
                var typeIsNotAssignableToItselfFluent =
                    Types().That().Are(type).Should().NotBeAssignableTo(Types().That().Are(type));
                var typeIsNotAssignableToFalseTypeMultiple1 = Types().That().Are(type).Should()
                    .NotBeAssignableTo(falseTypeList1).OrShould().Be(falseTypeList1);
                var typeIsNotAssignableToFalseTypeMultiple2 = Types().That().Are(type).Should()
                    .NotBeAssignableTo(falseTypeList2).OrShould().Be(falseTypeList1);
                var typeIsNotAssignableToFalseTypeMultiplePattern = Types().That().Are(type).Should()
                    .NotBeAssignableTo(falseTypeListPattern).OrShould().Be(falseTypeList1);
                var typeIsAssignableToFalseTypeMultiple1 = Types().That().Are(type).Should()
                    .BeAssignableTo(falseTypeList1).AndShould().NotBe(falseTypeList1);
                var typeIsAssignableToFalseTypeMultiple2 = Types().That().Are(type).Should()
                    .BeAssignableTo(falseTypeList2).AndShould().NotBe(falseTypeList1);
                var typeIsAssignableToFalseTypeMultiplePattern = Types().That().Are(type).Should()
                    .BeAssignableTo(falseTypeListPattern).AndShould().NotBe(falseTypeList1);

                Assert.True(typeIsAssignableToItselfFluent.HasNoViolations(Architecture));
                Assert.False(typeIsNotAssignableToItselfFluent.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseTypeMultiple1.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseTypeMultiple2.HasNoViolations(Architecture));
                Assert.True(typeIsNotAssignableToFalseTypeMultiplePattern.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseTypeMultiple1.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseTypeMultiple2.HasNoViolations(Architecture));
                Assert.False(typeIsAssignableToFalseTypeMultiplePattern.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void NestedInTest()
        {
            Classes().That().AreNestedIn(typeof(PublicTestClass))
                .Should().Be(typeof(PublicTestClass.ChildClass)).AndShould().Exist().Check(Architecture);
            
            Classes().That().AreNestedIn(typeof(PublicTestClass))
                .Should().NotBe(typeof(PublicTestClass)).AndShould().Exist().Check(Architecture);

            Classes().That().AreNestedIn(StaticTestTypes.PublicTestClass)
                .Should().Be(typeof(PublicTestClass.ChildClass)).AndShould().Exist().Check(Architecture);
            
            Classes().That().AreNestedIn(typeof(PublicTestClass.ChildClass))
                .Should().NotExist().Check(Architecture);
            
            Classes().That().AreNestedIn(typeof(PublicTestClass), typeof(InternalTestClass))
                .Should().Be(typeof(PublicTestClass.ChildClass)).AndShould().Exist().Check(Architecture);
            
            var typeList1 = new List<Type> {typeof(PublicTestClass), typeof(InternalTestClass)};
            var typeList2 = new List<IType> {StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass};

            Classes().That().AreNestedIn(typeList1).Should().Be(typeof(PublicTestClass.ChildClass)).AndShould().Exist().Check(Architecture);
            Classes().That().AreNestedIn(typeList2).Should().Be(typeof(PublicTestClass.ChildClass)).AndShould().Exist().Check(Architecture);
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
                var implementInterfaceIsEqualToAssignableTo = Types().That().ImplementInterface(intf.FullName).Should()
                    .BeAssignableTo(intf.FullName).And().Types().That().AreAssignableTo(intf.FullName).Should()
                    .ImplementInterface(intf.FullName).OrShould().Be(intf.FullName);

                Assert.True(typesThatImplementInterfaceImplementInterface.HasNoViolations(Architecture));
                Assert.False(typesThatImplementInterfaceDoNotImplementInterface.HasNoViolations(Architecture));
                Assert.False(typesThatDoNotImplementInterfaceImplementInterface.HasNoViolations(Architecture));
                Assert.True(typesThatDoNotImplementInterfaceDoNotImplementInterface.HasNoViolations(Architecture));
                Assert.True(implementInterfaceIsEqualToAssignableTo.HasNoViolations(Architecture));
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
        public void ResideInAssemblyTest()
        {
            foreach (var type in _types)
            {
                {
                    var typeResidesInOwnAssembly =
                        Types().That().Are(type).Should().ResideInAssembly(type.Assembly.FullName);
                    var typeDoesNotResideInOwnAssembly =
                        Types().That().Are(type).Should().NotResideInAssembly(type.Assembly.FullName);
                    var thereAreTypesInOwnAssembly =
                        Types().That().ResideInAssembly(type.Assembly.FullName).Should().Exist();
                    var typesInOtherAssemblyAreOtherTypes =
                        Types().That().DoNotResideInAssembly(type.Assembly.FullName).Should().NotBe(type);

                    Assert.True(typeResidesInOwnAssembly.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotResideInOwnAssembly.HasNoViolations(Architecture));
                    Assert.True(thereAreTypesInOwnAssembly.HasNoViolations(Architecture));
                    Assert.True(typesInOtherAssemblyAreOtherTypes.HasNoViolations(Architecture));
                }

                {
                    var typeResidesInOwnAssembly =
                        Types().That().Are(type).Should().ResideInAssembly(type.Assembly);
                    var typeDoesNotResideInOwnAssembly =
                        Types().That().Are(type).Should().NotResideInAssembly(type.Assembly);
                    var thereAreTypesInOwnAssembly =
                        Types().That().ResideInAssembly(type.Assembly).Should().Exist();
                    var typesInOtherAssemblyAreOtherTypes =
                        Types().That().DoNotResideInAssembly(type.Assembly).Should().NotBe(type);

                    Assert.True(typeResidesInOwnAssembly.HasNoViolations(Architecture));
                    Assert.False(typeDoesNotResideInOwnAssembly.HasNoViolations(Architecture));
                    Assert.True(thereAreTypesInOwnAssembly.HasNoViolations(Architecture));
                    Assert.True(typesInOtherAssemblyAreOtherTypes.HasNoViolations(Architecture));
                }
            }

            foreach (var assembly in Architecture.Assemblies.Select(assembly => assembly.FullName))
            {
                var typesInAssemblyAreInAssembly =
                    Types().That().ResideInAssembly(assembly).Should().ResideInAssembly(assembly);
                var typesInOtherAssemblyAreInOtherAssembly = Types().That().DoNotResideInAssembly(assembly).Should()
                    .NotResideInAssembly(assembly);

                Assert.True(typesInAssemblyAreInAssembly.HasNoViolations(Architecture));
                Assert.True(typesInOtherAssemblyAreInOtherAssembly.HasNoViolations(Architecture));
            }
            
            foreach (var assembly in Architecture.Assemblies)
            {
                var typesInAssemblyAreInAssembly =
                    Types().That().ResideInAssembly(assembly).Should().ResideInAssembly(assembly);
                var typesInOtherAssemblyAreInOtherAssembly = Types().That().DoNotResideInAssembly(assembly).Should()
                    .NotResideInAssembly(assembly);

                Assert.True(typesInAssemblyAreInAssembly.HasNoViolations(Architecture));
                Assert.True(typesInOtherAssemblyAreInOtherAssembly.HasNoViolations(Architecture));
            }

            var testClassIsInRightAssembly = Types().That().Are(typeof(PublicTestClass)).Should()
                .ResideInAssembly(typeof(PublicTestClass).Assembly);
            var testClassIsInFalseAssembly = Types().That().Are(typeof(PublicTestClass)).Should()
                .NotResideInAssembly(typeof(PublicTestClass).Assembly);
            var typesInRightAssemblyDoNotContainTestClass = Types().That()
                .ResideInAssembly(typeof(PublicTestClass).Assembly)
                .Should().NotBe(typeof(PublicTestClass));

            Assert.True(testClassIsInRightAssembly.HasNoViolations(Architecture));
            Assert.False(testClassIsInFalseAssembly.HasNoViolations(Architecture));
            Assert.False(typesInRightAssemblyDoNotContainTestClass.HasNoViolations(Architecture));
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
        public void AreEnumsTest()
        {
            foreach (var type in _types)
            {
                var isEnum = type is Enum;
                var typeIsEnum = Types().That().Are(type).Should().BeEnums();
                var typeIsNotEnum = Types().That().Are(type).Should().NotBeEnums();
                var enumsDoNotIncludeType = Types().That().AreEnums().Should().NotBe(type);
                var notEnumsDoNotIncludeType = Types().That().AreNotEnums().Should().NotBe(type);

                Assert.Equal(isEnum, typeIsEnum.HasNoViolations(Architecture));
                Assert.Equal(!isEnum, typeIsNotEnum.HasNoViolations(Architecture));
                Assert.Equal(!isEnum, enumsDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(isEnum, notEnumsDoNotIncludeType.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void AreStructsTest()
        {
            foreach (var type in _types)
            {
                var isStruct = type is Struct;
                var typeIsStruct = Types().That().Are(type).Should().BeStructs();
                var typeIsNotStruct = Types().That().Are(type).Should().NotBeStructs();
                var structsDoNotIncludeType = Types().That().AreStructs().Should().NotBe(type);
                var notStructsDoNotIncludeType = Types().That().AreNotStructs().Should().NotBe(type);

                Assert.Equal(isStruct, typeIsStruct.HasNoViolations(Architecture));
                Assert.Equal(!isStruct, typeIsNotStruct.HasNoViolations(Architecture));
                Assert.Equal(!isStruct, structsDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(isStruct, notStructsDoNotIncludeType.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void AreValueTypesTest()
        {
            foreach (var type in _types)
            {
                var isValueType = type is Struct || type is Enum;
                var typeIsValueType = Types().That().Are(type).Should().BeValueTypes();
                var typeIsNotValueType = Types().That().Are(type).Should().NotBeValueTypes();
                var valueTypesDoNotIncludeType = Types().That().AreValueTypes().Should().NotBe(type);
                var notValueTypesDoNotIncludeType = Types().That().AreNotValueTypes().Should().NotBe(type);

                Assert.Equal(isValueType, typeIsValueType.HasNoViolations(Architecture));
                Assert.Equal(!isValueType, typeIsNotValueType.HasNoViolations(Architecture));
                Assert.Equal(!isValueType, valueTypesDoNotIncludeType.HasNoViolations(Architecture));
                Assert.Equal(isValueType, notValueTypesDoNotIncludeType.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            typesThatAreNotNestedMustBeVisible.Check(Architecture);
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = Types().That().ArePrivate().Or()
                .AreProtected().Or().ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            typesWithRestrictedVisibilityMustBeNested.Check(Architecture);
        }
    }
}