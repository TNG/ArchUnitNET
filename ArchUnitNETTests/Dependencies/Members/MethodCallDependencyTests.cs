/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
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
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedVariable
// ReSharper disable NotAccessedField.Local

namespace ArchUnitNETTests.Dependencies.Members
{
    public class MethodCallDependencyTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _classWithConstructors;
        private readonly MethodMember _methodAMember;
        private readonly MethodMember _methodBMember;

        public MethodCallDependencyTests()
        {
            _classWithConstructors = _architecture.GetClassOfType(typeof(ClassWithConstructors));
            _methodAMember = _architecture.GetClassOfType(typeof(ClassWithMethodA))
                .GetMethodMembersWithName(nameof(ClassWithMethodA.MethodA).BuildMethodMemberName()).FirstOrDefault();
            _methodBMember = _architecture.GetClassOfType(typeof(ClassWithMethodB))
                .GetMethodMembersWithName(nameof(ClassWithMethodB.MethodB).BuildMethodMemberName()).FirstOrDefault();
        }

        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.ConstructorTestData))]
        public void ConstructorsAddedToClass(Class classWithConstructors)
        {
            //Setup
            var constructorMembers = classWithConstructors.GetConstructors();

            //Assert
            constructorMembers.ForEach(constructor =>
            {
                Assert.Contains(constructor, classWithConstructors.Constructors);
            });
        }

        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.MethodCallDependencyTestData))]
        public void MethodCallDependenciesAreFound(IMember originMember, MethodCallDependency expectedDependency)
        {
            Assert.True(originMember.HasMemberDependency(expectedDependency));
            Assert.Contains(expectedDependency, originMember.GetMethodCallDependencies());
        }
    }

    public class ClassWithMethodA
    {
        public static void MethodA()
        {
            var classWithMethodB = new ClassWithMethodB();
            ClassWithMethodB.MethodB();
        }
    }

    public class ClassWithMethodB
    {
        public static void MethodB()
        {
            var classWithMethodA = new ClassWithMethodA();
            ClassWithMethodA.MethodA();
        }
    }

    public class ClassWithConstructors
    {
        private FieldType _fieldTest;
        private FieldType _privateFieldTest;

        public ClassWithConstructors() : this(new FieldType())
        {
        }

        private ClassWithConstructors(FieldType fieldTest) : this(fieldTest, fieldTest)
        {
        }

        private ClassWithConstructors(FieldType fieldTest, FieldType privateFieldTest)
        {
            _fieldTest = fieldTest;
            _privateFieldTest = privateFieldTest;
        }
    }
}