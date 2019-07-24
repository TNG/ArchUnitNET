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
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using Mono.Cecil;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class ArchitectureCacheKeyTests
    {
        private readonly string _baseClassModuleName;
        private readonly string _architectureCacheTestsClassModuleName;
        private readonly string _memberDependencyTests;
        private readonly string _attributeDependencyTests;
        private readonly ArchitectureCacheKey _architectureCacheKey;
        private readonly ArchitectureCacheKey _duplicateArchitectureCacheKey;
        private readonly object _objectReferenceDuplicateArchitectureCacheKey;
        private readonly object _duplicateObjectArchitectureCacheKey;

        public ArchitectureCacheKeyTests()
        {
            _baseClassModuleName = ModuleDefinition
                .ReadModule(typeof(BaseClass).Assembly.Location, new ReaderParameters()).Name;

            _architectureCacheTestsClassModuleName = ModuleDefinition
                .ReadModule(typeof(ArchitectureCacheTests).Assembly.Location, new ReaderParameters()).Name;

            _memberDependencyTests = typeof(BodyTypeMemberDependencyTests).Namespace;
            _attributeDependencyTests = typeof(AttributeDependencyTests).Namespace;
            _architectureCacheKey = new ArchitectureCacheKey();
            _duplicateArchitectureCacheKey = new ArchitectureCacheKey();
        }

        [Fact]
        public void DuplicateAssemblies()
        {
            _architectureCacheKey.Add(_baseClassModuleName, null);

            var duplicateCacheKey = new ArchitectureCacheKey();
            duplicateCacheKey.Add(_baseClassModuleName, null);
            duplicateCacheKey.Add(_baseClassModuleName, null);

            Assert.Equal(_architectureCacheKey, duplicateCacheKey);
        }

        [Fact]
        public void DuplicateAssembliesDifferentOrder()
        {
            _architectureCacheKey.Add(_baseClassModuleName, null);
            _architectureCacheKey.Add(_architectureCacheTestsClassModuleName, null);

            var reverseOrderCacheKey = new ArchitectureCacheKey();
            reverseOrderCacheKey.Add(_architectureCacheTestsClassModuleName, null);
            reverseOrderCacheKey.Add(_baseClassModuleName, null);

            Assert.Equal(_architectureCacheKey, reverseOrderCacheKey);
        }

        [Fact]
        public void DuplicateFilteredNamespaces()
        {
            _architectureCacheKey.Add(_baseClassModuleName, _memberDependencyTests);

            var duplicateCacheKey = new ArchitectureCacheKey();
            duplicateCacheKey.Add(_baseClassModuleName, _memberDependencyTests);
            duplicateCacheKey.Add(_baseClassModuleName, _memberDependencyTests);

            Assert.Equal(_architectureCacheKey, duplicateCacheKey);
        }

        [Fact]
        public void DuplicateNamespacesDifferentOrder()
        {
            _architectureCacheKey.Add(_baseClassModuleName, _memberDependencyTests);
            _architectureCacheKey.Add(_baseClassModuleName, _attributeDependencyTests);

            var reverseOrderCacheKey = new ArchitectureCacheKey();
            reverseOrderCacheKey.Add(_baseClassModuleName, _attributeDependencyTests);
            reverseOrderCacheKey.Add(_baseClassModuleName, _memberDependencyTests);

            Assert.Equal(_architectureCacheKey, reverseOrderCacheKey);
        }

        [Fact]
        public void SameArchitecturesProduceSameArchitectureCacheKey()
        {
            _architectureCacheKey.Add(_baseClassModuleName, null);
            _duplicateArchitectureCacheKey.Add(_baseClassModuleName, null);
            
            Assert.Equal(_architectureCacheKey, _duplicateArchitectureCacheKey);
        }

        [Fact]
        public void SameObjectReferenceIsSameArchitectureCacheKet()
        {
            _architectureCacheKey.Add(_baseClassModuleName, null);
            object referenceDuplicate = _architectureCacheKey;
            
            Assert.True(_architectureCacheKey.Equals(referenceDuplicate));
        }

        [Fact]
        public void ArchitectureCacheKeyIsEqualToItself()
        {
            Assert.True(_architectureCacheKey.Equals(_architectureCacheKey));
        }

        [Fact]
        public void ArchitectureCacheKeyDoesNotEqualNull()
        {
            Assert.False(_architectureCacheKey.Equals((object) null));
        }

        [Fact]
        public void ArchitectureCacheKeyContentEquivalentsAreEqual()
        {
            object contentEquivalent = new ArchitectureCacheKey();
            Assert.True(_architectureCacheKey.Equals(contentEquivalent));
        }
    }
}