/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Dependencies.Types;
using ArchUnitNETTests.Fluent;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class InterfaceTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly InterfaceEquivalencyTestData _interfaceEquivalencyTestData;
        private readonly Interface _parentInterface;
        private readonly Interface _childInterface;
        private readonly Class _interfaceImplementingClass;

        public InterfaceTests()
        {
            _interfaceEquivalencyTestData = new InterfaceEquivalencyTestData(typeof(ITestInterface));
            _parentInterface = Architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));
            _childInterface = Architecture.GetInterfaceOfType(typeof(ITestInterface));
            _interfaceImplementingClass = Architecture.GetClassOfType(typeof(InheritedType));
        }

        [Fact]
        public void ImplementedInterfaceRecognized()
        {
            Assert.True(_childInterface.Implements(_parentInterface));
        }

        [Fact]
        public void ChildInterfaceAssignableToParent()
        {
            Assert.True(_childInterface.IsAssignableTo(_parentInterface));
        }

        [Fact]
        public void InterfaceNotAssignableToClass()
        {
            Assert.False(_childInterface.IsAssignableTo(_interfaceImplementingClass));
        }
        
        [Fact]
        public void DuplicateInterfacesAreEqual()
        {
            Assert.Equal(_interfaceEquivalencyTestData.OriginInterface,
                _interfaceEquivalencyTestData.DuplicateInterface);
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
        
        private class InterfaceEquivalencyTestData
        {
            public InterfaceEquivalencyTestData([NotNull] System.Type originType)
            {
                OriginInterface = Architecture.GetInterfaceOfType(originType).RequiredNotNull();
                DuplicateInterface = Architecture.GetInterfaceOfType(originType).RequiredNotNull();
                InterfaceReferenceDuplicate = OriginInterface;
                ObjectReferenceDuplicate = OriginInterface;
            }

            [NotNull] public Interface OriginInterface { get; }
            [NotNull] public object DuplicateInterface { get; }
            [NotNull] public Interface InterfaceReferenceDuplicate { get; }
            [NotNull] public object ObjectReferenceDuplicate { get; }
        }
    }
}