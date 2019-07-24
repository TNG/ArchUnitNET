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