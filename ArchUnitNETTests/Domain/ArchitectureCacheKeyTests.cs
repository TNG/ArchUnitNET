using ArchUnitNET.Domain;
using ArchUnitNETTests.Domain.Dependencies.Attributes;
using ArchUnitNETTests.Domain.Dependencies.Members;
using Mono.Cecil;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class ArchitectureCacheKeyTests
    {
        public ArchitectureCacheKeyTests()
        {
            _baseClassModuleName = ModuleDefinition
                .ReadModule(typeof(BaseClass).Assembly.Location, new ReaderParameters())
                .Name;

            _architectureCacheTestsClassModuleName = ModuleDefinition
                .ReadModule(
                    typeof(ArchitectureCacheTests).Assembly.Location,
                    new ReaderParameters()
                )
                .Name;

            _memberDependencyTests = typeof(BodyTypeMemberDependencyTests).Namespace;
            _attributeDependencyTests = typeof(AttributeDependencyTests).Namespace;
            _architectureCacheKey = new ArchitectureCacheKey();
            _duplicateArchitectureCacheKey = new ArchitectureCacheKey();
        }

        private readonly string _baseClassModuleName;
        private readonly string _architectureCacheTestsClassModuleName;
        private readonly string _memberDependencyTests;
        private readonly string _attributeDependencyTests;
        private readonly ArchitectureCacheKey _architectureCacheKey;
        private readonly ArchitectureCacheKey _duplicateArchitectureCacheKey;

        [Fact]
        public void ArchitectureCacheKeyContentEquivalentsAreEqual()
        {
            object contentEquivalent = new ArchitectureCacheKey();
            Assert.True(_architectureCacheKey.Equals(contentEquivalent));
        }

        [Fact]
        public void ArchitectureCacheKeyDoesNotEqualNull()
        {
            Assert.False(_architectureCacheKey.Equals((object)null));
        }

        [Fact]
        public void ArchitectureCacheKeyIsEqualToItself()
        {
            Assert.True(_architectureCacheKey.Equals(_architectureCacheKey));
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
    }
}
