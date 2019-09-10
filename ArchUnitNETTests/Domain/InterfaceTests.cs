/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Types;
using ArchUnitNETTests.Fluent.Extensions;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNETTests.Domain
{
    public class InterfaceTests
    {
        public InterfaceTests()
        {
            _interfaceEquivalencyTestData = new InterfaceEquivalencyTestData(typeof(ITestInterface));
            _parentInterface = Architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));
            _childInterface = Architecture.GetInterfaceOfType(typeof(ITestInterface));
            _interfaceImplementingClass = Architecture.GetClassOfType(typeof(InheritedType));
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly InterfaceEquivalencyTestData _interfaceEquivalencyTestData;
        private readonly Interface _parentInterface;
        private readonly Interface _childInterface;
        private readonly Class _interfaceImplementingClass;

        private class InterfaceEquivalencyTestData
        {
            public InterfaceEquivalencyTestData([NotNull] Type originType)
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
            Assert.True(_childInterface.Implements(_parentInterface));
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
            Assert.Equal(Public, Architecture.GetInterfaceOfType(typeof(IPublicTestInterface)).Visibility);
            Assert.Equal(Internal, Architecture.GetInterfaceOfType(typeof(IInternalTestInterface)).Visibility);
            Assert.Equal(Public, Architecture.GetInterfaceOfType(typeof(INestedPublicTestInterface)).Visibility);
            Assert.Equal(Private, Architecture.GetInterfaceOfType(typeof(INestedPrivateTestInterface)).Visibility);
            Assert.Equal(Protected,
                Architecture.GetInterfaceOfType(typeof(INestedProtectedTestInterface)).Visibility);
            Assert.Equal(Internal, Architecture.GetInterfaceOfType(typeof(INestedInternalTestInterface)).Visibility);
            Assert.Equal(ProtectedInternal,
                Architecture.GetInterfaceOfType(typeof(INestedProtectedInternalTestInterface)).Visibility);
            Assert.Equal(PrivateProtected,
                Architecture.GetInterfaceOfType(typeof(INestedPrivateProtectedTestInterface)).Visibility);
        }

        [Fact]
        public void InterfacesHaveCorrectIsNestedProperty()
        {
            Assert.False(Architecture.GetInterfaceOfType(typeof(IPublicTestInterface)).IsNested);
            Assert.False(Architecture.GetInterfaceOfType(typeof(IInternalTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedPublicTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedPrivateTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedProtectedTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedInternalTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedProtectedInternalTestInterface)).IsNested);
            Assert.True(Architecture.GetInterfaceOfType(typeof(INestedPrivateProtectedTestInterface)).IsNested);
        }

        // ReSharper disable MemberCanBePrivate.Global

        public interface INestedPublicTestInterface
        {
        }

        private interface INestedPrivateTestInterface
        {
        }

        protected interface INestedProtectedTestInterface
        {
        }

        internal interface INestedInternalTestInterface
        {
        }

        protected internal interface INestedProtectedInternalTestInterface
        {
        }

        private protected interface INestedPrivateProtectedTestInterface
        {
        }
    }

    public interface IPublicTestInterface
    {
    }

    internal interface IInternalTestInterface
    {
    }
}