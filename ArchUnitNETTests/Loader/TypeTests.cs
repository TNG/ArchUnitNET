//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Domain.Dependencies.Attributes;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class TypeTests
    {
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

        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Type _type;
        private readonly Type _duplicateType;
        private readonly object _duplicateReference;
        private readonly Interface _exampleInterface;
        private readonly Class _expectedAttributeClass;

        [Theory]
        [ClassData(typeof(TypeTestBuild.TypeModelingTestData))]
        public void ToStringAsExpected(Type type)
        {
            Assert.Equal(type.ToString(), type.FullName);
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

        [Fact]
        public void AccessAttributes()
        {
            //Setup, Act
            var attribute = new Attribute(_expectedAttributeClass);
            _type.Attributes.Add(attribute);

            //Assert
            Assert.Contains(attribute, _type.Attributes);
        }

        [Fact]
        public void IsAssignableToDuplicate()
        {
            Assert.True(_type.IsAssignableTo(_duplicateType));
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
        public void IsAssignableToItself()
        {
            Assert.True(_type.IsAssignableTo(_type));
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
    }

    [Example]
    public class AssignClass : IExample
    {
    }

    public interface IExample
    {
    }
}