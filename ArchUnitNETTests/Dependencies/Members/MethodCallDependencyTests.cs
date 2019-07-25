/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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