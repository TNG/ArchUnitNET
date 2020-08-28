//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class BodyTypeMemberDependencyTests
    {
        public BodyTypeMemberDependencyTests()
        {
            var classWithBodyTypeA = _architecture.GetClassOfType(typeof(ClassWithBodyTypeA));
            _methodWithTypeA = classWithBodyTypeA
                .Members[nameof(ClassWithBodyTypeA.MethodWithTypeA).BuildMethodMemberName()] as MethodMember;
            _typeA = _architecture.GetClassOfType(typeof(TypeA));
        }

        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly MethodMember _methodWithTypeA;
        private readonly Class _typeA;

        [Fact]
        public void BodyTypeDependenciesFound()
        {
            var bodyTypeDependencies =
                _methodWithTypeA.GetBodyTypeMemberDependencies().ToList();

            Assert.Equal("AABC", ClassWithBodyTypeA.MethodWithTypeA());
            Assert.True(bodyTypeDependencies.Count >= 3);
            Assert.Contains(_typeA, bodyTypeDependencies.Select(dependency => (Class) dependency.Target));
        }
    }

    public class ClassWithBodyTypeA
    {
        public static string MethodWithTypeA()
        {
            var typeA = new TypeA();
            var typeA2 = new TypeA();
            var typeB = new TypeB();
            var typeC = typeA.MethodReturnsTypeC();
            return typeA.ToString() + typeA2 + typeB + typeC;
        }
    }

    public class TypeA
    {
        public TypeC MethodReturnsTypeC()
        {
            return new TypeC();
        }

        public override string ToString()
        {
            return "A";
        }
    }

    public class TypeB
    {
        public override string ToString()
        {
            return "B";
        }
    }

    public class TypeC
    {
        public override string ToString()
        {
            return "C";
        }
    }
}