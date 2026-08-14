using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class SliceTests
    {
        [Fact]
        public void Equals_SameIdentifier_DifferentTypeInstances_AreEqual()
        {
            // Bug: Slice.Equals compared Types by reference, so two slices with the
            // same identifier but different IEnumerable<IType> instances were not equal.
            // A slice is uniquely identified by its SliceIdentifier.
            var slice1 = new Slice(SliceIdentifier.Of("Slice1"), new List<IType>());
            var slice2 = new Slice(SliceIdentifier.Of("Slice1"), new List<IType>());
            Assert.Equal(slice1, slice2);
            Assert.Equal(slice1.GetHashCode(), slice2.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentIdentifier_AreNotEqual()
        {
            var slice1 = new Slice(SliceIdentifier.Of("A"), new List<IType>());
            var slice2 = new Slice(SliceIdentifier.Of("B"), new List<IType>());
            Assert.NotEqual(slice1, slice2);
        }

        [Fact]
        public void ContainsNamespace_WithNamespace_ReturnsTrue()
        {
            var id = SliceIdentifier.Of("Slice1", "Prefix.");
            var slice = new Slice(id, new List<IType>());
            Assert.True(slice.ContainsNamespace());
        }

        [Fact]
        public void ContainsNamespace_WithoutNamespace_ReturnsFalse()
        {
            var id = SliceIdentifier.Of("Slice1");
            var slice = new Slice(id, new List<IType>());
            Assert.False(slice.ContainsNamespace());
        }

        [Fact]
        public void Description_ReturnsIdentifierDescription()
        {
            var slice = new Slice(SliceIdentifier.Of("MySlice"), new List<IType>());
            Assert.Equal("MySlice", slice.Description);
        }

        [Fact]
        public void ToString_ReturnsDescription()
        {
            var slice = new Slice(SliceIdentifier.Of("MySlice"), new List<IType>());
            Assert.Equal("MySlice", slice.ToString());
        }
    }
}
