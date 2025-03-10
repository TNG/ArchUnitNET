//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
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
        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Class _expectedAttributeClass;
        private readonly Type _type;

        public TypeTests()
        {
            _type = _architecture.GetClassOfType(typeof(AssignClass)).CreateShallowStubType();
            _type.RequiredNotNull();
            _expectedAttributeClass = _architecture.GetClassOfType(typeof(ExampleAttribute));
        }

        [Theory]
        [ClassData(typeof(TypeTestBuild.TypeModelingTestData))]
        public void ToStringAsExpected(Type type)
        {
            Assert.Equal(type.ToString(), type.FullName);
        }

        [Theory]
        [ClassData(typeof(TypeTestBuild.TypeEquivalencyModelingTestData))]
        public void TypeEquivalencyTests(
            IType type,
            object duplicateType,
            IType typeCopy,
            [CanBeNull] object referenceCopy
        )
        {
            DuplicateTypesAreEqual(type, duplicateType);
            DuplicateTypeObjectReferencesAreEqual(type, referenceCopy);
            DuplicateTypeReferencesAreEqual(type, typeCopy);
            TypeDoesNotEqualNull(type);
            TypeHasConsistentHashCode(type, duplicateType);
        }

        private static void DuplicateTypesAreEqual(
            [NotNull] IType type,
            [NotNull] object duplicateType
        )
        {
            type.RequiredNotNull();
            duplicateType.RequiredNotNull();

            Assert.Equal(type, duplicateType);
        }

        private static void DuplicateTypeObjectReferencesAreEqual(
            [NotNull] IType type,
            object objectReferenceDuplicate
        )
        {
            type.RequiredNotNull();
            objectReferenceDuplicate.RequiredNotNull();

            Assert.Equal(type, objectReferenceDuplicate);
        }

        private static void DuplicateTypeReferencesAreEqual(
            [NotNull] IType type,
            [NotNull] IType typeReferenceDuplicate
        )
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

        private static void TypeHasConsistentHashCode(
            [NotNull] IType type,
            [NotNull] object duplicateType
        )
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
            var instance = new AttributeInstance(attribute);
            _type.AttributeInstances.Add(instance);

            //Assert
            Assert.Contains(attribute, _type.Attributes);
        }

        [Fact]
        public void NotAssignableToNull()
        {
            Assert.False(_type.IsAssignableTo((IType)null));
            Assert.False(_type.IsAssignableTo((string)null));
        }

        [Fact]
        public void TypesAreNotLoadedFromMultipleAssemblies()
        {
            var booleanType = _architecture
                .Types.Concat(_architecture.ReferencedTypes)
                .Where(type => type.FullName == "System.Boolean")
                .ToList();
            Assert.Single(booleanType);
            Assert.False(booleanType.First().Assembly.Name.StartsWith("ArchUnitNET"));
        }
    }

    [Example]
    public class AssignClass : IExample { }

    public interface IExample { }
}
