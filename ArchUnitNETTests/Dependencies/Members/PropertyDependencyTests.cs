/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ArchUnitNETTests.Dependencies.Members
{
    public class PropertyDependencyTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _classWithPropertyA;
        private readonly PropertyMember _propertyAMember;
        private readonly PropertyMember _privatePropertyAMember;
        private readonly Class _propertyType;

        public PropertyDependencyTests()
        {
            _classWithPropertyA = _architecture.GetClassOfType(typeof(ClassWithPropertyA));
            _propertyType = _architecture.GetClassOfType(typeof(PropertyType));
            _propertyAMember = _classWithPropertyA.Members["PropertyA"] as PropertyMember;
            _privatePropertyAMember = _classWithPropertyA.Members["PrivatePropertyA"] as PropertyMember;
        }

        [Fact]
        public void ClassDependencyForPropertyMemberTypesAreCreated()
        {
            var expectedDependency = new PropertyTypeDependency(_propertyAMember);

            Assert.True(_classWithPropertyA.HasDependency(expectedDependency));
        }

        [Fact]
        public void PrivatePropertyMembersAreCreatedWithCorrectVisibility()
        {
            Assert.Equal(Visibility.Private, _privatePropertyAMember?.GetterVisibility);
            Assert.Equal(Visibility.Private, _privatePropertyAMember?.SetterVisibility);
        }

        [Fact]
        public void PropertyDependenciesAddedToClassDependencies()
        {
            var propertyMembers = _classWithPropertyA.GetPropertyMembers();
            propertyMembers.ForEach(propertyMember =>
                Assert.True(_classWithPropertyA.HasDependencies(propertyMember.MemberDependencies)));
        }

        [Fact]
        public void PropertyMembersAreCreated()
        {
            Assert.Equal(_classWithPropertyA, _propertyAMember?.DeclaringType);
            Assert.Equal(Visibility.Public, _propertyAMember?.GetterVisibility);
            Assert.Equal(_propertyType, _propertyAMember?.Type);
        }
    }

    public class ClassWithPropertyA
    {
        public PropertyType PropertyA { get; private set; }

        private PropertyType PrivatePropertyA { get; set; }
    }

    public class PropertyType
    {
        private object _field;

        public PropertyType()
        {
            _field = null;
        }

        public PropertyType(object field)
        {
            _field = field;
        }
    }
}