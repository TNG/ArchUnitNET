﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Domain;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class MethodMemberSyntaxElementsTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly IEnumerable<MethodMember> _methodMembers;
        private readonly IEnumerable<IType> _types;

        public MethodMemberSyntaxElementsTests()
        {
            _methodMembers = Architecture.MethodMembers;
            _types = Architecture.Types;
        }

        [Fact]
        public void AreConstructorsTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                var methodMemberIsConstructor = MethodMembers().That().Are(methodMember).Should().BeConstructor();
                var methodMemberIsNoConstructor = MethodMembers().That().Are(methodMember).Should().BeNoConstructor();
                var constructorMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreConstructors().Should().NotBe(methodMember).OrShould().NotExist();
                var noConstructorMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreNoConstructors().Should().NotBe(methodMember).AndShould().Exist();

                Assert.Equal(methodMember.IsConstructor(), methodMemberIsConstructor.HasNoViolations(Architecture));
                Assert.Equal(!methodMember.IsConstructor(), methodMemberIsNoConstructor.HasNoViolations(Architecture));
                Assert.Equal(!methodMember.IsConstructor(),
                    constructorMethodMembersDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(methodMember.IsConstructor(),
                    noConstructorMethodMembersDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var constructorMethodMembersShouldBeConstructor =
                MethodMembers().That().AreConstructors().Should().BeConstructor();
            var constructorMethodMembersAreNoConstructors =
                MethodMembers().That().AreConstructors().Should().BeNoConstructor().AndShould().Exist();
            var noConstructorMethodMembersShouldBeConstructor =
                MethodMembers().That().AreNoConstructors().Should().BeConstructor().AndShould().Exist();
            var noConstructorMethodMembersAreNoConstructors =
                MethodMembers().That().AreNoConstructors().Should().BeNoConstructor();

            Assert.True(constructorMethodMembersShouldBeConstructor.HasNoViolations(Architecture));
            Assert.False(constructorMethodMembersAreNoConstructors.HasNoViolations(Architecture));
            Assert.False(noConstructorMethodMembersShouldBeConstructor.HasNoViolations(Architecture));
            Assert.True(noConstructorMethodMembersAreNoConstructors.HasNoViolations(Architecture));
        }

        [Fact]
        public void AreVirtualTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                var methodMemberIsVirtual = MethodMembers().That().Are(methodMember).Should().BeVirtual();
                var methodMemberIsNotVirtual = MethodMembers().That().Are(methodMember).Should().NotBeVirtual();
                var virtualMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreVirtual().Should().NotBe(methodMember).OrShould().NotExist();
                var notVirtualMethodMembersDoNotIncludeMember =
                    MethodMembers().That().AreNotVirtual().Should().NotBe(methodMember).AndShould().Exist();

                Assert.Equal(methodMember.IsVirtual, methodMemberIsVirtual.HasNoViolations(Architecture));
                Assert.Equal(!methodMember.IsVirtual, methodMemberIsNotVirtual.HasNoViolations(Architecture));
                Assert.Equal(!methodMember.IsVirtual,
                    virtualMethodMembersDoNotIncludeMember.HasNoViolations(Architecture));
                Assert.Equal(methodMember.IsVirtual,
                    notVirtualMethodMembersDoNotIncludeMember.HasNoViolations(Architecture));
            }

            var virtualMethodMembersShouldBeVirtual = MethodMembers().That().AreVirtual().Should().BeVirtual();
            var virtualMethodMembersAreNotVirtual =
                MethodMembers().That().AreVirtual().Should().NotBeVirtual().AndShould().Exist();
            var notVirtualMethodMembersShouldBeVirtual =
                MethodMembers().That().AreNotVirtual().Should().BeVirtual().AndShould().Exist();
            var notVirtualMethodMembersAreNotVirtual =
                MethodMembers().That().AreNotVirtual().Should().NotBeVirtual();

            Assert.True(virtualMethodMembersShouldBeVirtual.HasNoViolations(Architecture));
            Assert.False(virtualMethodMembersAreNotVirtual.HasNoViolations(Architecture));
            Assert.False(notVirtualMethodMembersShouldBeVirtual.HasNoViolations(Architecture));
            Assert.True(notVirtualMethodMembersAreNotVirtual.HasNoViolations(Architecture));
        }

        [Fact]
        public void CalledByTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                foreach (var callingType in methodMember.GetMethodCallDependencies(true)
                    .Select(dependency => dependency.Origin.FullName))
                {
                    var methodIsCalledByRightType =
                        MethodMembers().That().Are(methodMember).Should().BeCalledBy(callingType);
                    var methodIsNotCalledByRightType =
                        MethodMembers().That().Are(methodMember).Should().NotBeCalledBy(callingType);

                    Assert.True(methodIsCalledByRightType.HasNoViolations(Architecture));
                    Assert.False(methodIsNotCalledByRightType.HasNoViolations(Architecture));
                }

                var methodIsCalledByFalseType = MethodMembers().That().Are(methodMember).Should()
                    .BeCalledBy(typeof(PublicTestClass).FullName);
                var methodIsNotCalledByFalseType = MethodMembers().That().Are(methodMember).Should()
                    .NotBeCalledBy(typeof(PublicTestClass).FullName);

                Assert.False(methodIsCalledByFalseType.HasNoViolations(Architecture));
                Assert.True(methodIsNotCalledByFalseType.HasNoViolations(Architecture));
            }

            foreach (var type in _types)
            {
                var calledMethodsShouldBeCalled = MethodMembers().That().AreCalledBy(type.FullName).Should()
                    .BeCalledBy(type.FullName);
                var notCalledMethodsShouldNotBeCalled = MethodMembers().That().AreNotCalledBy(type.FullName).Should()
                    .NotBeCalledBy(type.FullName);

                Assert.True(calledMethodsShouldBeCalled.HasNoViolations(Architecture));
                Assert.True(notCalledMethodsShouldNotBeCalled.HasNoViolations(Architecture));
            }

            var emptyTypeCallsNoMethods =
                MethodMembers().That().AreCalledBy(typeof(PublicTestClass).FullName).Should().NotExist();
            var methodsNotCalledByEmptyTypeShouldExist = MethodMembers().That()
                .AreNotCalledBy(typeof(PublicTestClass).FullName).Should().Exist();

            Assert.True(emptyTypeCallsNoMethods.HasNoViolations(Architecture));
            Assert.True(methodsNotCalledByEmptyTypeShouldExist.HasNoViolations(Architecture));
        }


        [Fact]
        public void HaveDependencyInMethodBodyTest()
        {
            foreach (var methodMember in _methodMembers)
            {
                foreach (var dependency in methodMember.GetBodyTypeMemberDependencies()
                    .Select(dependency => dependency.Target.FullName))
                {
                    var hasRightDependency = MethodMembers().That().Are(methodMember).Should()
                        .HaveDependencyInMethodBodyTo(dependency);
                    var doesNotHaveRightDependency = MethodMembers().That().Are(methodMember).Should()
                        .NotHaveDependencyInMethodBodyTo(dependency);

                    Assert.True(hasRightDependency.HasNoViolations(Architecture));
                    Assert.False(doesNotHaveRightDependency.HasNoViolations(Architecture));
                }
            }

            foreach (var type in _types)
            {
                var dependentMethodsShouldBeDependent = MethodMembers().That()
                    .HaveDependencyInMethodBodyTo(type.FullName).Should()
                    .HaveDependencyInMethodBodyTo(type.FullName);
                var notDependentMethodsShouldNotBeDependent = MethodMembers().That()
                    .DoNotHaveDependencyInMethodBodyTo(type.FullName).Should()
                    .NotHaveDependencyInMethodBodyTo(type.FullName);

                Assert.True(dependentMethodsShouldBeDependent.HasNoViolations(Architecture));
                Assert.True(notDependentMethodsShouldNotBeDependent.HasNoViolations(Architecture));
            }
        }

        [Fact]
        public void HaveReturnTypeTest()
        {
            var stringReturnTypes = new List<string> {"void", "string", "ReturnTypeClass"};
            var retTypeWithString = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethod").Should()
                .HaveReturnType(stringReturnTypes);
            var retTypeWithStringFail = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethod").Should()
                .HaveReturnType("bool");

            Assert.True(retTypeWithString.HasNoViolations(Architecture));
            Assert.False(retTypeWithStringFail.HasNoViolations(Architecture));

            var retTypeWithType = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethod").Should()
                .HaveReturnType(typeof(ReturnTypeClass), typeof(void), typeof(string));
            var retTypeWithTypeFail = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethod").Should()
                .HaveReturnType(typeof(bool));

            Assert.True(retTypeWithType.HasNoViolations(Architecture));
            Assert.False(retTypeWithTypeFail.HasNoViolations(Architecture));

            var objectProviderClass = Classes().That().HaveFullNameContaining("ReturnTypeClass");
            var retTypeWithObjectProvider = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethodClass")
                .Should()
                .HaveReturnType(objectProviderClass);
            var retTypeWithObjectProviderFail = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethodVoid")
                .Should()
                .HaveReturnType(objectProviderClass);

            Assert.True(retTypeWithObjectProvider.HasNoViolations(Architecture));
            Assert.False(retTypeWithObjectProviderFail.HasNoViolations(Architecture));

            var retTypeWithIType = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethodClass").Should()
                .HaveReturnType(objectProviderClass.GetObjects(Architecture).ToList().First().Type);
            var retTypeWithITypeFail = MethodMembers().That().HaveFullNameContaining("ReturnTypeMethodString").Should()
                .HaveReturnType(objectProviderClass.GetObjects(Architecture).ToList().First().Type);

            Assert.True(retTypeWithIType.HasNoViolations(Architecture));
            Assert.False(retTypeWithITypeFail.HasNoViolations(Architecture));
        }
    }

    internal class ReturnTypeClass
    {
        public void ReturnTypeMethodVoid()
        {
        }

        public string ReturnTypeMethodString()
        {
            return "";
        }

        public ReturnTypeClass ReturnTypeMethodClass()
        {
            return new ReturnTypeClass();
        }
    }
}