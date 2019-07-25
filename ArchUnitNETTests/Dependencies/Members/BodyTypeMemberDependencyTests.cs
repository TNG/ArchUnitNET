/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;

// ReSharper disable UnusedVariable

namespace ArchUnitNETTests.Dependencies.Members
{
    public class BodyTypeMemberDependencyTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly MethodMember _methodWithTypeA;
        private readonly Class _typeA;

        public BodyTypeMemberDependencyTests()
        {
            var classWithBodyTypeA = _architecture.GetClassOfType(typeof(ClassWithBodyTypeA));
            _methodWithTypeA = classWithBodyTypeA
                .Members[nameof(ClassWithBodyTypeA.MethodWithTypeA).BuildMethodMemberName()] as MethodMember;
            _typeA = _architecture.GetClassOfType(typeof(TypeA));
        }

        [Fact]
        public void BodyTypeDependenciesFound()
        {
            var bodyTypeDependencies =
                _methodWithTypeA.GetBodyTypeMemberDependencies().ToList();

            Assert.Equal(3, bodyTypeDependencies.Count);
            Assert.Contains(_typeA, bodyTypeDependencies.Select(dependency => (Class) dependency.Target));
        }
    }

    public class ClassWithBodyTypeA
    {
        public void MethodWithTypeA()
        {
            var typeA = new TypeA();
            var typeB = new TypeB();
            var typeC = typeA.MethodReturnsTypeC();
        }
    }

    public class TypeA
    {
        public TypeC MethodReturnsTypeC()
        {
            return new TypeC();
        }
    }

    public class TypeB
    {
    }

    public class TypeC
    {
    }
}