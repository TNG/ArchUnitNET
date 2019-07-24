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

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Fluent;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests.Core
{
    public class TypeTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;
        private readonly Type _type;
        private readonly Type _duplicateType;
        private readonly object _duplicateReference;
        private readonly Interface _exampleInterface;
        private readonly Class _expectedAttributeClass;

        public TypeTests()
        {
            _type = _architecture.GetClassOfType(typeof(AssignClass)).CreateShallowStubType();
            _type.RequiredNotNull();
            
            _duplicateType = _architecture.GetClassOfType(typeof(AssignClass)).CreateShallowStubType();
            _duplicateType.RequiredNotNull();
            
            _duplicateReference = _type;
            
            _exampleInterface = _architecture.GetInterfaceOfType(typeof(IExample));
            _exampleInterface.RequiredNotNull();
            _expectedAttributeClass = _architecture.GetClassOfType(typeof(ExampleAttribute));
        }

        [Theory]
        [ClassData(typeof(TypeTestBuild.TypeModelingTestData))]
        public void ToStringAsExpected(Type type)
        {
            Assert.Equal(type.ToString(), type.FullName);
        }

        [Fact]
        public void IsAssignableToItself()
        {
            Assert.True(_type.IsAssignableTo(_type));
        }
        
        [Fact]
        public void IsAssignableToDuplicate()
        {
            Assert.True(_type.IsAssignableTo(_duplicateType));
        }
        
        [Fact]
        public void IsAssignableToReferenceCopy()
        {
            Assert.True(_type.IsAssignableTo(_duplicateReference as IType));
        }

        [Fact]
        public void NotAssignableToNull()
        {
            Assert.False(_type.IsAssignableTo(null));
        }

        [Fact]
        public void IsAssignableToImplementedInterface()
        {
            //Setup, Act
            var interfaceDependency = new ImplementsInterfaceDependency(_type, _exampleInterface);
            _type.Dependencies.Add(interfaceDependency);
            
            //Assert
            Assert.True(_type.IsAssignableTo(_exampleInterface));
        }

        [Fact]
        public void AccessAttributes()
        {
            //Setup, Act
            var attribute = new Attribute(_expectedAttributeClass);
            _type.Attributes.Add(attribute);
            
            //Assert
            Assert.Contains(attribute, _type.Attributes);
        }
        
        [Theory]
        [ClassData(typeof(TypeTestBuild.TypeEquivalencyModelingTestData))]
        public void TypeEquivalencyTests(IType type, object duplicateType, IType typeCopy,
            [CanBeNull] object referenceCopy)
        {
            DuplicateTypesAreEqual(type, duplicateType);
            DuplicateTypeObjectReferencesAreEqual(type, referenceCopy);
            DuplicateTypeReferencesAreEqual(type, typeCopy);
            TypeDoesNotEqualNull(type);
            TypeHasConsistentHashCode(type, duplicateType);
        }
        
        private static void DuplicateTypesAreEqual([NotNull] IType type, [NotNull] object duplicateType)
        {
            type.RequiredNotNull();
            duplicateType.RequiredNotNull();

            Assert.Equal(type, duplicateType);
        }

        private static void DuplicateTypeObjectReferencesAreEqual([NotNull] IType type,
            object objectReferenceDuplicate)
        {
            type.RequiredNotNull();
            objectReferenceDuplicate.RequiredNotNull();

            Assert.Equal(type, objectReferenceDuplicate);
        }

        private static void DuplicateTypeReferencesAreEqual([NotNull] IType type,
            [NotNull] IType typeReferenceDuplicate)
        {
            type.RequiredNotNull();
            typeReferenceDuplicate.RequiredNotNull();
            
            Assert.True(type.Equals(typeReferenceDuplicate));
        }

        private static void TypeDoesNotEqualNull([NotNull] IType type)
        {
            type.RequiredNotNull();
            
            Assert.False(type.Equals(null));
        }

        private static void TypeHasConsistentHashCode([NotNull] IType type,
            [NotNull] object duplicateType)
        {
            type.RequiredNotNull();
            duplicateType.RequiredNotNull();
            
            var hash = type.GetHashCode();
            var duplicateHash = duplicateType.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }
    }

    [Example]
    public class AssignClass : IExample
    {
    }

    public interface IExample
    {
    }
}