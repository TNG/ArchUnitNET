//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Domain
{
    public class InterfaceTests
    {
        public InterfaceTests()
        {
            _interfaceEquivalencyTestData = new InterfaceEquivalencyTestData(InheritingInterface);
            _parentInterface = InheritedTestInterface;
            _childInterface = InheritingInterface;
            _interfaceImplementingClass = StaticTestTypes.InheritedType;
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly InterfaceEquivalencyTestData _interfaceEquivalencyTestData;
        private readonly Interface _parentInterface;
        private readonly Interface _childInterface;
        private readonly Class _interfaceImplementingClass;

        private class InterfaceEquivalencyTestData
        {
            public InterfaceEquivalencyTestData([NotNull] Interface originType)
            {
                OriginInterface = originType;
                DuplicateInterface = originType;
                InterfaceReferenceDuplicate = OriginInterface;
                ObjectReferenceDuplicate = OriginInterface;
            }

            [NotNull] public Interface OriginInterface { get; }
            [NotNull] public object DuplicateInterface { get; }
            [NotNull] public Interface InterfaceReferenceDuplicate { get; }
            [NotNull] public object ObjectReferenceDuplicate { get; }
        }

        [Fact]
        public void ChildInterfaceAssignableToParent()
        {
            Assert.True(_childInterface.IsAssignableTo(_parentInterface));
        }

        [Fact]
        public void DuplicateInterfaceObjectReferencesAreEqual()
        {
            Assert.Equal(_interfaceEquivalencyTestData.OriginInterface,
                _interfaceEquivalencyTestData.ObjectReferenceDuplicate);
        }

        [Fact]
        public void DuplicateInterfaceReferencesAreEqual()
        {
            Assert.True(_interfaceEquivalencyTestData.OriginInterface
                .Equals(_interfaceEquivalencyTestData.InterfaceReferenceDuplicate));
        }

        [Fact]
        public void DuplicateInterfacesAreEqual()
        {
            Assert.Equal(_interfaceEquivalencyTestData.OriginInterface,
                _interfaceEquivalencyTestData.DuplicateInterface);
        }

        [Fact]
        public void ImplementedInterfaceRecognized()
        {
            Assert.True(_childInterface.ImplementsInterface(_parentInterface));
        }

        [Fact]
        public void InterfaceDoesNotEqualNull()
        {
            Assert.False(_interfaceEquivalencyTestData.OriginInterface.Equals(null));
        }

        [Fact]
        public void InterfaceHasConsistentHashCode()
        {
            var hash = _interfaceEquivalencyTestData.OriginInterface.GetHashCode();
            var duplicateHash = _interfaceEquivalencyTestData.DuplicateInterface.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }

        [Fact]
        public void InterfaceNotAssignableToClass()
        {
            Assert.False(_childInterface.IsAssignableTo(_interfaceImplementingClass));
        }

        [Fact]
        public void InterfacesAreAssignedCorrectVisibility()
        {
            Assert.Equal(Public, PublicTestInterface.Visibility);
            Assert.Equal(Internal, InternalTestInterface.Visibility);
            Assert.Equal(Public, NestedPublicTestInterface.Visibility);
            Assert.Equal(Private, NestedPrivateTestInterface.Visibility);
            Assert.Equal(Protected, NestedProtectedTestInterface.Visibility);
            Assert.Equal(Internal, NestedInternalTestInterface.Visibility);
            Assert.Equal(ProtectedInternal, NestedProtectedInternalTestInterface.Visibility);
            Assert.Equal(PrivateProtected, NestedPrivateProtectedTestInterface.Visibility);
        }

        [Fact]
        public void InterfacesHaveCorrectIsNestedProperty()
        {
            Assert.False(PublicTestInterface.IsNested);
            Assert.False(InternalTestInterface.IsNested);
            Assert.True(NestedPublicTestInterface.IsNested);
            Assert.True(NestedPrivateTestInterface.IsNested);
            Assert.True(NestedProtectedTestInterface.IsNested);
            Assert.True(NestedInternalTestInterface.IsNested);
            Assert.True(NestedProtectedInternalTestInterface.IsNested);
            Assert.True(NestedPrivateProtectedTestInterface.IsNested);
        }
    }
}