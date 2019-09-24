/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNETTests.Domain.StaticTestTypes;


namespace ArchUnitNETTests.Domain
{
    public class ClassTests
    {
        public ClassTests()
        {
            _baseClass = Architecture.GetClassOfType(typeof(BaseClass));
            _childClass = Architecture.GetClassOfType(typeof(ChildClass));
            _duplicateChildClass = _baseClass;
            var backingType = Architecture.GetTypeOfType(typeof(PropertyType));
            _misMatchType =
                new Type(backingType.FullName, backingType.Name, backingType.Assembly, backingType.Namespace,
                    backingType.Visibility, backingType.IsNested);

            _implementsInterface = Architecture.GetClassOfType(typeof(InheritingType));
            _implementedInterface = Architecture.GetInterfaceOfType(typeof(IInheritingInterface));
            _chainedInterface = Architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));

            _classEquivalencyTestData = new ClassEquivalencyTestData(typeof(ClassWithConstructors));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Class _baseClass;
        private readonly Class _childClass;
        private readonly Class _duplicateChildClass;
        private readonly Type _misMatchType;

        private readonly Class _implementsInterface;
        private readonly Interface _implementedInterface;
        private readonly Interface _chainedInterface;

        private readonly ClassEquivalencyTestData _classEquivalencyTestData;

        private class ClassEquivalencyTestData
        {
            public ClassEquivalencyTestData([NotNull] System.Type originType)
            {
                OriginClass = Architecture.GetClassOfType(originType).RequiredNotNull();
                DuplicateClass = Architecture.GetClassOfType(originType).RequiredNotNull();
                ClassReferenceDuplicate = OriginClass;
                ObjectReferenceDuplicate = OriginClass;
            }

            [NotNull] public Class OriginClass { get; }
            [NotNull] public object DuplicateClass { get; }
            [NotNull] public Class ClassReferenceDuplicate { get; }
            [NotNull] public object ObjectReferenceDuplicate { get; }
        }

        [Fact]
        public void AssignableToDirectlyImplementedInterfaces()
        {
            Assert.True(_implementsInterface.IsAssignableTo(_implementedInterface));
        }

        [Fact]
        public void AssignableToIndirectlyImplementedInterfaces()
        {
            Assert.True(_implementsInterface.IsAssignableTo(_chainedInterface));
        }

        [Fact]
        public void AssignableToParentClass()
        {
            Assert.True(_childClass.IsAssignableTo(_baseClass));
        }

        [Fact]
        public void AssignableToSameClass()
        {
            Assert.True(_childClass.IsAssignableTo(_duplicateChildClass));
        }

        [Fact]
        public void ClassDoesNotEqualNull()
        {
            Assert.False(_classEquivalencyTestData.OriginClass.Equals(null));
        }

        [Fact]
        public void ClassesAreAssignedCorrectVisibility()
        {
            Assert.Equal(Public, StaticTestTypes.PublicTestClass.Visibility);
            Assert.Equal(Internal, StaticTestTypes.InternalTestClass.Visibility);
            Assert.Equal(Public, NestedPublicTestClass.Visibility);
            Assert.Equal(Private, NestedPrivateTestClass.Visibility);
            Assert.Equal(Protected, NestedProtectedTestClass.Visibility);
            Assert.Equal(Internal, NestedInternalTestClass.Visibility);
            Assert.Equal(ProtectedInternal, NestedProtectedInternalTestClass.Visibility);
            Assert.Equal(PrivateProtected, NestedPrivateProtectedTestClass.Visibility);
        }

        [Fact]
        public void ClassesHaveCorrectIsEnumProperty()
        {
            Assert.True(StaticTestTypes.TestEnum.IsEnum);
            Assert.False(StaticTestTypes.TestStruct.IsEnum);
            Assert.False(StaticTestTypes.PublicTestClass.IsEnum);
        }

        [Fact]
        public void ClassesHaveCorrectIsNestedProperty()
        {
            Assert.False(StaticTestTypes.PublicTestClass.IsNested);
            Assert.False(StaticTestTypes.InternalTestClass.IsNested);
            Assert.True(NestedPublicTestClass.IsNested);
            Assert.True(NestedPrivateTestClass.IsNested);
            Assert.True(NestedProtectedTestClass.IsNested);
            Assert.True(NestedInternalTestClass.IsNested);
            Assert.True(NestedProtectedInternalTestClass.IsNested);
            Assert.True(NestedPrivateProtectedTestClass.IsNested);
        }

        [Fact]
        public void ClassesHaveCorrectIsSealedProperty()
        {
            Assert.True(StaticTestTypes.SealedTestClass.IsSealed);
            Assert.False(StaticTestTypes.PublicTestClass.IsSealed);
        }

        [Fact]
        public void ClassesHaveCorrectIsStructProperty()
        {
            Assert.True(StaticTestTypes.TestStruct.IsStruct);
            Assert.False(StaticTestTypes.TestEnum.IsStruct);
            Assert.False(StaticTestTypes.PublicTestClass.IsStruct);
        }

        [Fact]
        public void ClassesHaveCorrectIsValueTypeProperty()
        {
            Assert.True(StaticTestTypes.TestEnum.IsValueType);
            Assert.True(StaticTestTypes.TestStruct.IsValueType);
            Assert.False(StaticTestTypes.PublicTestClass.IsValueType);
        }

        [Fact]
        public void ClassHasConsistentHashCode()
        {
            var hash = _classEquivalencyTestData.OriginClass.GetHashCode();
            var duplicateHash = _classEquivalencyTestData.DuplicateClass.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }

        [Fact]
        public void DuplicateClassesAreEqual()
        {
            Assert.Equal(_classEquivalencyTestData.OriginClass,
                _classEquivalencyTestData.DuplicateClass);
        }

        [Fact]
        public void DuplicateClassObjectReferencesAreEqual()
        {
            Assert.Equal(_classEquivalencyTestData.OriginClass,
                _classEquivalencyTestData.ObjectReferenceDuplicate);
        }

        [Fact]
        public void DuplicateClassReferencesAreEqual()
        {
            Assert.True(_classEquivalencyTestData.OriginClass
                .Equals(_classEquivalencyTestData.ClassReferenceDuplicate));
        }

        [Fact]
        public void NotAssignableToUnrelatedType()
        {
            Assert.False(_childClass.IsAssignableTo(_misMatchType));
        }

        [Fact]
        public void ParentDependenciesAreInherited()
        {
            _baseClass.Dependencies.ForEach(parentDependency =>
            {
                Assert.Contains(parentDependency, _childClass.DependenciesIncludingInherited);
            });
        }
    }
}