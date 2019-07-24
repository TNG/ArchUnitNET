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
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;

namespace ArchUnitNETTests.Dependencies.Members
{
    public class FieldDependencyTests
    {
        private readonly Class _classWithFieldA;
        private readonly FieldMember _fieldAMember;
        private readonly Class _fieldType;
        private readonly FieldMember _privateFieldAMember;

        public FieldDependencyTests()
        {
            var architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;
            _classWithFieldA = architecture.GetClassOfType(typeof(ClassWithFieldA));
            _fieldAMember = _classWithFieldA.GetFieldMembersWithName(nameof(ClassWithFieldA.FieldA)).SingleOrDefault();
            _fieldType = architecture.GetClassOfType(typeof(FieldType));
            _privateFieldAMember = _classWithFieldA.GetFieldMembersWithName("_privateFieldA").SingleOrDefault();
        }

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
                _classWithFieldA.HasDependencies(fieldMember.MemberDependencies));
        }

        [Fact]
        public void FieldMembersAreCreated()
        {
            Assert.Equal(_classWithFieldA, _fieldAMember?.DeclaringType);
            Assert.Equal(Visibility.Public, _fieldAMember?.Visibility);
            Assert.Equal(_fieldType, _fieldAMember?.Type);
        }

        [Fact]
        public void PrivateFieldMembersAreCreatedWithCorrectVisibility()
        {
            Assert.Equal(Visibility.Private, _privateFieldAMember?.Visibility);
        }
    }

    public class ClassWithFieldA
    {
        public FieldType FieldA;
        private FieldType _privateFieldA;
    }

    public class FieldType
    {
    }
}