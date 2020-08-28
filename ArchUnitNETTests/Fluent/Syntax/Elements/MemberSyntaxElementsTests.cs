//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class MemberSyntaxElementsTests
    {
        public MemberSyntaxElementsTests()
        {
            _members = Architecture.Members;
            _types = Architecture.Types;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IMember> _members;
        private readonly IEnumerable<IType> _types;

        private readonly List<IType> _falseTypes1 = new List<IType>
            {StaticTestTypes.PublicTestClass, StaticTestTypes.InternalTestClass};

        private readonly List<Type> _falseTypes2 = new List<Type> {typeof(PublicTestClass), typeof(InternalTestClass)};

        private readonly List<string> _falseTypesPattern = new List<string>
            {StaticTestTypes.PublicTestClass.FullName, StaticTestTypes.InternalTestClass.FullName};

        private readonly List<string> _falseTypeConstructors =
            new List<string> {PublicTestClassConstructor, InternalTestClassConstructor};

        private const string PublicTestClassConstructor = "ArchUnitNETTests.Domain.PublicTestClass::.ctor()";
        private const string InternalTestClassConstructor = "ArchUnitNETTests.Domain.InternalTestClass::.ctor()";

        [Fact]
        public void DeclaredInTest()
        {
            foreach (var member in _members)
            {
                //One Argument

                var declaredInRightType = Members().That().Are(member).Should().BeDeclaredIn(member.DeclaringType);
                var notDeclaredInRightType =
                    Members().That().Are(member).Should().NotBeDeclaredIn(member.DeclaringType);
                var declaredInOtherType1 = Members().That().Are(member).Should().BeDeclaredIn(typeof(PublicTestClass))
                    .AndShould().NotBe(PublicTestClassConstructor);
                var notDeclaredInOtherType1 =
                    Members().That().Are(member).Should().NotBeDeclaredIn(typeof(PublicTestClass)).OrShould()
                        .Be(PublicTestClassConstructor);
                var declaredInOtherType2 =
                    Members().That().Are(member).Should().BeDeclaredIn(StaticTestTypes.PublicTestClass).AndShould()
                        .NotBe(PublicTestClassConstructor);
                var notDeclaredInOtherType2 = Members().That().Are(member).Should()
                    .NotBeDeclaredIn(StaticTestTypes.PublicTestClass).OrShould().Be(PublicTestClassConstructor);

                Assert.True(declaredInRightType.HasNoViolations(Architecture));
                Assert.False(notDeclaredInRightType.HasNoViolations(Architecture));
                Assert.False(declaredInOtherType1.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherType1.HasNoViolations(Architecture));
                Assert.False(declaredInOtherType2.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherType2.HasNoViolations(Architecture));

                //Multiple Arguments

                var declaredInRightTypeFluent = Members().That().Are(member).Should()
                    .BeDeclaredIn(Types().That().HaveMemberWithName(member.Name));
                var notDeclaredInRightTypeFluent = Members().That().Are(member).Should()
                    .NotBeDeclaredIn(Types().That().HaveMemberWithName(member.Name));
                var declaredInOtherTypeMultiple1 = Members().That().Are(member).Should().BeDeclaredIn(_falseTypes1)
                    .AndShould().NotBe(_falseTypeConstructors);
                var notDeclaredInOtherTypeMultiple1 =
                    Members().That().Are(member).Should().NotBeDeclaredIn(_falseTypes1).OrShould()
                        .Be(_falseTypeConstructors);
                var declaredInOtherTypeMultiple2 =
                    Members().That().Are(member).Should().BeDeclaredIn(_falseTypes2).AndShould()
                        .NotBe(_falseTypeConstructors);
                var notDeclaredInOtherTypeMultiple2 = Members().That().Are(member).Should()
                    .NotBeDeclaredIn(_falseTypes2).OrShould().Be(_falseTypeConstructors);
                var declaredInOtherTypeMultiplePattern =
                    Members().That().Are(member).Should().BeDeclaredIn(_falseTypesPattern).AndShould()
                        .NotBe(_falseTypeConstructors);
                var notDeclaredInOtherTypeMultiplePattern = Members().That().Are(member).Should()
                    .NotBeDeclaredIn(_falseTypesPattern).OrShould().Be(_falseTypeConstructors);

                Assert.True(declaredInRightTypeFluent.HasNoViolations(Architecture));
                Assert.False(notDeclaredInRightTypeFluent.HasNoViolations(Architecture));
                Assert.False(declaredInOtherTypeMultiple1.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherTypeMultiple1.HasNoViolations(Architecture));
                Assert.False(declaredInOtherTypeMultiple2.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherTypeMultiple2.HasNoViolations(Architecture));
                Assert.False(declaredInOtherTypeMultiplePattern.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherTypeMultiplePattern.HasNoViolations(Architecture));
            }

            foreach (var type in _types)
            {
                var membersShouldBeDeclaredInOwnType = Members().That().AreDeclaredIn(type).Should().BeDeclaredIn(type);
                var membersShouldNotBeDeclaredInOwnType =
                    Members().That().AreNotDeclaredIn(type).Should().BeDeclaredIn(type);

                Assert.True(membersShouldBeDeclaredInOwnType.HasNoViolations(Architecture));
                Assert.False(membersShouldNotBeDeclaredInOwnType.HasNoViolations(Architecture));
            }

            //One Argument

            var emptyTypeHasOnlyConstructor1 = Members().That().AreDeclaredIn(typeof(PublicTestClass)).Should()
                .Be(PublicTestClassConstructor);
            var emptyTypeHasOnlyConstructor2 =
                Members().That().AreDeclaredIn(StaticTestTypes.PublicTestClass).Should().Be(PublicTestClassConstructor);
            var emptyTypeHasOnlyConstructorPattern =
                Members().That().AreDeclaredIn(typeof(PublicTestClass).FullName).Should()
                    .Be(PublicTestClassConstructor);
            var allMembersAreNotDeclaredInEmptyType1 =
                Members().That().AreNotDeclaredIn(typeof(PublicTestClass)).Should()
                    .Be(Members().That().AreNot(PublicTestClassConstructor));
            var allMembersAreNotDeclaredInEmptyType2 =
                Members().That().AreNotDeclaredIn(StaticTestTypes.PublicTestClass).Should()
                    .Be(Members().That().AreNot(PublicTestClassConstructor));
            var allMembersAreNotDeclaredInEmptyTypePattern = Members().That()
                .AreNotDeclaredIn(typeof(PublicTestClass).FullName).Should()
                .Be(Members().That().AreNot(PublicTestClassConstructor));

            Assert.True(emptyTypeHasOnlyConstructor1.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructor2.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructorPattern.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyType1.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyType2.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypePattern.HasNoViolations(Architecture));

            //Multiple Arguments

            var emptyTypeHasOnlyConstructorMultiple1 =
                Members().That().AreDeclaredIn(_falseTypes1).Should().Be(_falseTypeConstructors);
            var emptyTypeHasOnlyConstructorMultiple2 =
                Members().That().AreDeclaredIn(_falseTypes2).Should().Be(_falseTypeConstructors);
            var emptyTypeHasOnlyConstructorMultiplePattern =
                Members().That().AreDeclaredIn(_falseTypesPattern).Should().Be(_falseTypeConstructors);
            var allMembersAreNotDeclaredInEmptyTypeMultiple1 =
                Members().That().AreNotDeclaredIn(_falseTypes1).Should()
                    .Be(Members().That().AreNot(_falseTypeConstructors));
            var allMembersAreNotDeclaredInEmptyTypeMultiple2 =
                Members().That().AreNotDeclaredIn(_falseTypes2).Should()
                    .Be(Members().That().AreNot(_falseTypeConstructors));
            var allMembersAreNotDeclaredInEmptyTypeMultiplePattern =
                Members().That().AreNotDeclaredIn(_falseTypesPattern).Should()
                    .Be(Members().That().AreNot(_falseTypeConstructors));

            Assert.True(emptyTypeHasOnlyConstructorMultiple1.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructorMultiple2.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructorMultiplePattern.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypeMultiple1.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypeMultiple2.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypeMultiplePattern.HasNoViolations(Architecture));
        }
    }
}