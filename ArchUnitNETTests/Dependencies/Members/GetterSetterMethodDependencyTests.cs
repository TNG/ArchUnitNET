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

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace ArchUnitNETTests.Dependencies.Members
{
    public class GetterSetterMethodDependencyTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly PropertyMember _acceptedCaseProperty;
        private readonly PropertyMember _firstUnacceptedCaseProperty;
        private readonly PropertyMember _secondUnacceptedCaseProperty;

        public GetterSetterMethodDependencyTests()
        {
            var getterExampleClass = _architecture.GetClassOfType(typeof(GetterMethodDependencyExamples));
            getterExampleClass.RequiredNotNull();
            _acceptedCaseProperty = getterExampleClass.GetPropertyMembersWithName("AcceptedCase").First();
            _firstUnacceptedCaseProperty = getterExampleClass.GetPropertyMembersWithName("FirstUnacceptedCase").First();
            _secondUnacceptedCaseProperty = getterExampleClass.GetPropertyMembersWithName("SecondUnacceptedCase")
                .First();
        }

        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.SetterTestData))]
        public void AssertSetterMethodDependencies(FieldMember backingField, PropertyMember backedProperty,
            Class expectedTarget)
        {
            backingField.MemberDependencies
                .ForEach(dependency =>
                {
                    Assert.Contains(dependency, backedProperty.MemberDependencies);
                    Assert.Contains(dependency, backedProperty.Setter.MemberDependencies);
                });
            
            var backingFieldDependencyTargets = backingField.MemberDependencies.Select(dependency => dependency.Target);
            Assert.Contains(expectedTarget, backingFieldDependencyTargets);
        }
        
        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.GetterTestData))]
        public void AssertGetterMethodDependencies(PropertyMember propertyMember, Class mockTargetClass,
            MethodCallDependency expectedDependency)
        {
            Assert.NotEmpty(propertyMember.MemberDependencies);
            Assert.Single(propertyMember.GetMethodCallDependencies());
            Assert.Contains(mockTargetClass,
                propertyMember.GetMethodCallDependencies().Select(dependency => dependency.Target));
            Assert.Contains(expectedDependency.TargetMember.FullName,
                propertyMember.GetMethodCallDependencies()
                    .Select(dependency => dependency.TargetMember.FullName));
        }

        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.AccessMethodDependenciesByPropertyTestData))]
        public void AccessorMethodDependenciesByProperty(PropertyMember accessedProperty, MethodMember accessorMethod)
        {
            accessorMethod.MemberDependencies.ForEach(dependency =>
            {
                Assert.Contains(dependency, accessedProperty.MemberDependencies);
            });
        }
    }
}