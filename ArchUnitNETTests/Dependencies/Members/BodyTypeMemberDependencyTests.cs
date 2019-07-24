/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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