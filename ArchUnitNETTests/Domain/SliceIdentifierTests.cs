using System.Collections.Generic;
using ArchUnitNET.Domain;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class SliceIdentifierTests
    {
        [Fact]
        public void Of_SetsIdentifierAndNamespace()
        {
            var id = SliceIdentifier.Of("Slice1", "Prefix.");
            Assert.Equal("Slice1", id.Description);
            Assert.Equal("Prefix.", id.NameSpace);
            Assert.False(id.Ignored);
        }

        [Fact]
        public void Of_WithoutNamespace_HasNullNamespace()
        {
            var id = SliceIdentifier.Of("Slice1");
            Assert.Equal("Slice1", id.Description);
            Assert.Null(id.NameSpace);
            Assert.False(id.Ignored);
        }

        [Fact]
        public void Ignore_CreatesIgnoredIdentifier()
        {
            var id = SliceIdentifier.Ignore();
            Assert.True(id.Ignored);
            Assert.Equal("Ignored", id.Description);
        }

        [Fact]
        public void CompareTo_SameIdentifier_ReturnsTrue()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice1");
            Assert.True(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_DifferentIdentifier_ReturnsFalse()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice2");
            Assert.False(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_BothIgnored_ReturnsTrue()
        {
            var a = SliceIdentifier.Ignore();
            var b = SliceIdentifier.Ignore();
            Assert.True(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_OneIgnored_ReturnsFalse()
        {
            var normal = SliceIdentifier.Of("Slice1");
            var ignored = SliceIdentifier.Ignore();
            Assert.False(normal.CompareTo(ignored));
            Assert.False(ignored.CompareTo(normal));
        }

        [Fact]
        public void CompareTo_Null_ReturnsFalse()
        {
            var id = SliceIdentifier.Of("Slice1");
            Assert.False(id.CompareTo(null));
        }

        [Fact]
        public void Equals_SameValues_AreEqual()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice1");
            Assert.Equal(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentValues_AreNotEqual()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice2");
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Comparer_GroupsIdenticalIdentifiers()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice1");
            Assert.True(SliceIdentifier.Comparer.Equals(a, b));
        }

        [Fact]
        public void Comparer_GroupsIgnoredIdentifiers()
        {
            var a = SliceIdentifier.Ignore();
            var b = SliceIdentifier.Ignore();
            Assert.True(SliceIdentifier.Comparer.Equals(a, b));
        }

        [Fact]
        public void Comparer_DistinguishesDifferentIdentifiers()
        {
            var a = SliceIdentifier.Of("Slice1");
            var b = SliceIdentifier.Of("Slice2");
            Assert.False(SliceIdentifier.Comparer.Equals(a, b));
        }

        // --- Multi-part identifier tests ---

        [Fact]
        public void Of_SinglePart_HasOnePart()
        {
            var id = SliceIdentifier.Of("Slice1");
            Assert.Single(id.Parts);
            Assert.Equal("Slice1", id.Parts[0]);
            Assert.Equal("Slice1", id.Description);
        }

        [Fact]
        public void Of_MultiPart_HasMultipleParts()
        {
            var id = SliceIdentifier.Of(new List<string> { "Foo", "Bar.Baz" });
            Assert.Equal(2, id.Parts.Count);
            Assert.Equal("Foo", id.Parts[0]);
            Assert.Equal("Bar.Baz", id.Parts[1]);
        }

        [Fact]
        public void Of_MultiPart_DescriptionJoinsWithDot()
        {
            var id = SliceIdentifier.Of(new List<string> { "Foo", "Bar.Baz" });
            Assert.Equal("Foo.Bar.Baz", id.Description);
        }

        [Fact]
        public void Of_MultiPart_WithNamespace()
        {
            var id = SliceIdentifier.Of(new List<string> { "App.Foo", "Bar.Baz" }, "App.");
            Assert.Equal("App.", id.NameSpace);
            Assert.Equal("App.Foo.Bar.Baz", id.Description);
        }

        [Fact]
        public void CompareTo_SameMultiParts_ReturnsTrue()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            Assert.True(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_DifferentMultiParts_ReturnsFalse()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Baz" });
            Assert.False(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_DifferentPartCounts_ReturnsFalse()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            Assert.False(a.CompareTo(b));
        }

        [Fact]
        public void CompareTo_SingleVsMultiPart_SameFirstPart_ReturnsFalse()
        {
            var a = SliceIdentifier.Of("Foo");
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            Assert.False(a.CompareTo(b));
        }

        [Fact]
        public void Equals_SameMultiParts_AreEqual()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            Assert.Equal(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Equals_DifferentMultiParts_AreNotEqual()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Baz" });
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Comparer_GroupsSameMultiPartIdentifiers()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            Assert.True(SliceIdentifier.Comparer.Equals(a, b));
        }

        [Fact]
        public void Comparer_DistinguishesDifferentMultiPartIdentifiers()
        {
            var a = SliceIdentifier.Of(new List<string> { "Foo", "Bar" });
            var b = SliceIdentifier.Of(new List<string> { "Foo", "Baz" });
            Assert.False(SliceIdentifier.Comparer.Equals(a, b));
        }

        [Fact]
        public void Ignore_HasSingleIgnoredPart()
        {
            var id = SliceIdentifier.Ignore();
            Assert.Single(id.Parts);
            Assert.Equal("Ignored", id.Parts[0]);
        }
    }
}
