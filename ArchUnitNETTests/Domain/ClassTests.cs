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
using ArchUnitNETTests.Dependencies.Members;
using ArchUnitNETTests.Dependencies.Types;
using ArchUnitNETTests.Fluent;
using JetBrains.Annotations;
using Xunit;
using Type = ArchUnitNET.Core.Type;

namespace ArchUnitNETTests.Domain
{
    public class ClassTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly Class _baseClass;
        private readonly Class _childClass;
        private readonly Class _duplicateChildClass;
        private readonly Type _misMatchType;

        private readonly Class _implementsInterface;
        private readonly Interface _implementedInterface;
        private readonly Interface _chainedInterface;
        
        private readonly ClassEquivalencyTestData _classEquivalencyTestData;

        public ClassTests()
        {
            _baseClass = Architecture.GetClassOfType(typeof(BaseClass));
            _childClass = Architecture.GetClassOfType(typeof(ChildClass));
            _duplicateChildClass = _baseClass;
            var backingType = Architecture.GetTypeOfType(typeof(PropertyType));
            _misMatchType =
                new Type(backingType.FullName, backingType.Name, backingType.Assembly, backingType.Namespace);

            _implementsInterface = Architecture.GetClassOfType(typeof(InheritingType));
            _implementedInterface = Architecture.GetInterfaceOfType(typeof(ITestInterface));
            _chainedInterface = Architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));
            
            _classEquivalencyTestData = new ClassEquivalencyTestData(typeof(ClassWithConstructors));
        }

        [Fact]
        public void AssignableToParentClass()
        {
            Assert.True(_childClass.IsAssignableTo(_baseClass));
        }
        
        [Fact]
        public void NotAssignableToUnrelatedType()
        {
            Assert.False(_childClass.IsAssignableTo(_misMatchType));
        }

        [Fact]
        public void AssignableToSameClass()
        {
            Assert.True(_childClass.IsAssignableTo(_duplicateChildClass));
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
        public void ParentDependenciesAreInherited()
        {
            _baseClass.Dependencies.ForEach(parentDependency =>
                {
                    Assert.Contains(parentDependency, _childClass.DependenciesIncludingInherited);
                });
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
        public void ClassDoesNotEqualNull()
        {
            Assert.False(_classEquivalencyTestData.OriginClass.Equals(null));
        }
        
        [Fact]
        public void ClassHasConsistentHashCode()
        {
            var hash = _classEquivalencyTestData.OriginClass.GetHashCode();
            var duplicateHash = _classEquivalencyTestData.DuplicateClass.GetHashCode();
            Assert.Equal(hash, duplicateHash);
        }
        
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
    }
}