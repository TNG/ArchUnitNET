//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class FieldDependencyTests
    {
        public FieldDependencyTests()
        {
            var architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            _classWithFieldA = architecture.GetClassOfType(typeof(ClassWithFieldA));
            _fieldAMember = _classWithFieldA.GetFieldMembersWithName(nameof(ClassWithFieldA.FieldA)).SingleOrDefault();
            _fieldType = architecture.GetClassOfType(typeof(FieldType));
            _privateFieldAMember = _classWithFieldA.GetFieldMembersWithName("_privateFieldA").SingleOrDefault();
        }

        private readonly Class _classWithFieldA;
        private readonly FieldMember _fieldAMember;
        private readonly Class _fieldType;
        private readonly FieldMember _privateFieldAMember;

        [Fact]
        public void ClassDependencyForFieldMemberTypesAreCreated()
        {
            //Setup, Act
            var expected = new FieldTypeDependency(_fieldAMember);

            //Assert
            Assert.True(_classWithFieldA.HasDependency(expected));
            Assert.Contains(expected, _classWithFieldA.GetFieldTypeDependencies());
        }

        [Fact]
        public void FieldDependenciesAddedToClassDependencies()
        {
            //Setup, Act
            var fieldMembers = _classWithFieldA.GetFieldMembers();

            //Assert
            Assert.All(fieldMembers, fieldMember =>
                Assert.True(_classWithFieldA.HasDependencies(fieldMember.MemberDependencies)));
        }

        [Fact]
        public void FieldMembersAreCreated()
        {
            Assert.Equal(_classWithFieldA, _fieldAMember?.DeclaringType);
            Assert.Equal(Public, _fieldAMember?.Visibility);
            Assert.Equal(_fieldType, _fieldAMember?.Type);
        }

        [Fact]
        public void PrivateFieldMembersAreCreatedWithCorrectVisibility()
        {
            Assert.Equal(Private, _privateFieldAMember?.Visibility);
        }
    }

#pragma warning disable 169
    public class ClassWithFieldA
    {
        private FieldType _privateFieldA;
        public FieldType FieldA;
    }

    public class FieldType
    {
    }
}