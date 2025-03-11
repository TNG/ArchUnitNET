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

        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<IMember> _members;
        private readonly IEnumerable<IType> _types;

        private readonly List<IType> _falseTypes1 = new List<IType>
        {
            StaticTestTypes.PublicTestClass,
            StaticTestTypes.InternalTestClass,
        };

        private readonly List<Type> _falseTypes2 = new List<Type>
        {
            typeof(PublicTestClass),
            typeof(InternalTestClass),
        };

        private static readonly IObjectProvider<MethodMember> FalseTypeConstructors =
            MethodMembers()
                .That()
                .HaveFullName("System.Void ArchUnitNETTests.Domain.PublicTestClass::.ctor()")
                .Or()
                .HaveFullName("System.Void ArchUnitNETTests.Domain.InternalTestClass::.ctor()");

        private static readonly IObjectProvider<MethodMember> PublicTestClassConstructor =
            MethodMembers()
                .That()
                .HaveFullName("System.Void ArchUnitNETTests.Domain.PublicTestClass::.ctor()");

        [Fact]
        public void DeclaredInTest()
        {
            foreach (var member in _members)
            {
                //One Argument

                var declaredInRightType = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(member.DeclaringType);
                var notDeclaredInRightType = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(member.DeclaringType);
                var declaredInOtherType1 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(typeof(PublicTestClass))
                    .AndShould()
                    .NotBe(PublicTestClassConstructor);
                var notDeclaredInOtherType1 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(typeof(PublicTestClass))
                    .OrShould()
                    .Be(PublicTestClassConstructor);
                var declaredInOtherType2 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(StaticTestTypes.PublicTestClass)
                    .AndShould()
                    .NotBe(PublicTestClassConstructor);
                var notDeclaredInOtherType2 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(StaticTestTypes.PublicTestClass)
                    .OrShould()
                    .Be(PublicTestClassConstructor);

                Assert.True(declaredInRightType.HasNoViolations(Architecture));
                Assert.False(notDeclaredInRightType.HasNoViolations(Architecture));
                Assert.False(declaredInOtherType1.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherType1.HasNoViolations(Architecture));
                Assert.False(declaredInOtherType2.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherType2.HasNoViolations(Architecture));

                //Multiple Arguments

                var declaredInRightTypeFluent = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(Types().That().HaveMemberWithName(member.Name));
                var notDeclaredInRightTypeFluent = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(Types().That().HaveMemberWithName(member.Name));
                var declaredInOtherTypeMultiple1 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(_falseTypes1)
                    .AndShould()
                    .NotBe(FalseTypeConstructors);
                var notDeclaredInOtherTypeMultiple1 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(_falseTypes1)
                    .OrShould()
                    .Be(FalseTypeConstructors);
                var declaredInOtherTypeMultiple2 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .BeDeclaredIn(_falseTypes2)
                    .AndShould()
                    .NotBe(FalseTypeConstructors);
                var notDeclaredInOtherTypeMultiple2 = Members()
                    .That()
                    .Are(member)
                    .Should()
                    .NotBeDeclaredIn(_falseTypes2)
                    .OrShould()
                    .Be(FalseTypeConstructors);

                Assert.True(declaredInRightTypeFluent.HasNoViolations(Architecture));
                Assert.False(notDeclaredInRightTypeFluent.HasNoViolations(Architecture));
                Assert.False(declaredInOtherTypeMultiple1.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherTypeMultiple1.HasNoViolations(Architecture));
                Assert.False(declaredInOtherTypeMultiple2.HasNoViolations(Architecture));
                Assert.True(notDeclaredInOtherTypeMultiple2.HasNoViolations(Architecture));
            }

            foreach (var type in _types)
            {
                var membersShouldBeDeclaredInOwnType = Members()
                    .That()
                    .AreDeclaredIn(type)
                    .Should()
                    .BeDeclaredIn(type)
                    .WithoutRequiringPositiveResults();
                var membersShouldNotBeDeclaredInOwnType = Members()
                    .That()
                    .AreNotDeclaredIn(type)
                    .Should()
                    .BeDeclaredIn(type)
                    .WithoutRequiringPositiveResults();

                Assert.True(membersShouldBeDeclaredInOwnType.HasNoViolations(Architecture));
                Assert.False(membersShouldNotBeDeclaredInOwnType.HasNoViolations(Architecture));
            }

            //One Argument

            var emptyTypeHasOnlyConstructor1 = Members()
                .That()
                .AreDeclaredIn(typeof(PublicTestClass))
                .Should()
                .Be(PublicTestClassConstructor);
            var emptyTypeHasOnlyConstructor2 = Members()
                .That()
                .AreDeclaredIn(StaticTestTypes.PublicTestClass)
                .Should()
                .Be(PublicTestClassConstructor);
            var allMembersAreNotDeclaredInEmptyType1 = Members()
                .That()
                .AreNotDeclaredIn(typeof(PublicTestClass))
                .Should()
                .Be(Members().That().AreNot(PublicTestClassConstructor));
            var allMembersAreNotDeclaredInEmptyType2 = Members()
                .That()
                .AreNotDeclaredIn(StaticTestTypes.PublicTestClass)
                .Should()
                .Be(Members().That().AreNot(PublicTestClassConstructor));

            Assert.True(emptyTypeHasOnlyConstructor1.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructor2.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyType1.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyType2.HasNoViolations(Architecture));

            //Multiple Arguments

            var emptyTypeHasOnlyConstructorMultiple1 = Members()
                .That()
                .AreDeclaredIn(_falseTypes1)
                .Should()
                .Be(FalseTypeConstructors);
            var emptyTypeHasOnlyConstructorMultiple2 = Members()
                .That()
                .AreDeclaredIn(_falseTypes2)
                .Should()
                .Be(FalseTypeConstructors);
            var allMembersAreNotDeclaredInEmptyTypeMultiple1 = Members()
                .That()
                .AreNotDeclaredIn(_falseTypes1)
                .Should()
                .Be(Members().That().AreNot(FalseTypeConstructors));
            var allMembersAreNotDeclaredInEmptyTypeMultiple2 = Members()
                .That()
                .AreNotDeclaredIn(_falseTypes2)
                .Should()
                .Be(Members().That().AreNot(FalseTypeConstructors));

            Assert.True(emptyTypeHasOnlyConstructorMultiple1.HasNoViolations(Architecture));
            Assert.True(emptyTypeHasOnlyConstructorMultiple2.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypeMultiple1.HasNoViolations(Architecture));
            Assert.True(allMembersAreNotDeclaredInEmptyTypeMultiple2.HasNoViolations(Architecture));
        }

        [Fact]
        public void IsStaticTest()
        {
            var correctIsStatic = Members().That().AreStatic().Should().BeStatic();
            var correctIsStatic2 = Members().That().AreNotStatic().Should().NotBeStatic();
            var wrongStatic = Members().That().AreStatic().Should().NotBeStatic();
            var wrongStatic2 = Members().That().AreNotStatic().Should().BeStatic();

            Assert.True(correctIsStatic.HasNoViolations(Architecture));
            Assert.True(correctIsStatic2.HasNoViolations(Architecture));
            Assert.False(wrongStatic.HasNoViolations(Architecture));
            Assert.False(wrongStatic2.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreImmutableTest()
        {
            foreach (var member in _members)
            {
                var memberIsImmutable = Members().That().Are(member).Should().BeImmutable();
                var memberIsNotImmutable = Members().That().Are(member).Should().NotBeImmutable();
                var membersThatAreImmutableDoNotIncludeMember = Members()
                    .That()
                    .AreImmutable()
                    .Should()
                    .NotBe(member)
                    .OrShould()
                    .NotExist();
                var membersThatAreNotImmutableDoNotIncludeMember = Members()
                    .That()
                    .AreNotImmutable()
                    .Should()
                    .NotBe(member)
                    .AndShould()
                    .Exist();

                bool isImmutable = member.Writability.IsImmutable();
                Assert.Equal(isImmutable, memberIsImmutable.HasNoViolations(Architecture));
                Assert.Equal(!isImmutable, memberIsNotImmutable.HasNoViolations(Architecture));
                Assert.Equal(
                    !isImmutable,
                    membersThatAreImmutableDoNotIncludeMember.HasNoViolations(Architecture)
                );
                Assert.Equal(
                    isImmutable,
                    membersThatAreNotImmutableDoNotIncludeMember.HasNoViolations(Architecture)
                );
            }

            var membersThatAreImmutableAreImmutable = Members()
                .That()
                .AreImmutable()
                .Should()
                .BeImmutable();
            var membersThatAreImmutableAreNotImmutable = Members()
                .That()
                .AreImmutable()
                .Should()
                .NotBeImmutable()
                .AndShould()
                .Exist();
            var membersThatAreNotImmutableAreImmutable = Members()
                .That()
                .AreNotImmutable()
                .Should()
                .BeImmutable()
                .AndShould()
                .Exist();
            var membersThatAreNotImmutableAreNotImmutable = Members()
                .That()
                .AreNotImmutable()
                .Should()
                .NotBeImmutable();

            Assert.True(membersThatAreImmutableAreImmutable.HasNoViolations(Architecture));
            Assert.False(membersThatAreImmutableAreNotImmutable.HasNoViolations(Architecture));
            Assert.False(membersThatAreNotImmutableAreImmutable.HasNoViolations(Architecture));
            Assert.True(membersThatAreNotImmutableAreNotImmutable.HasNoViolations(Architecture));
        }
    }
}
