using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Domain.Extensions
{
    public class NullableExtensionTests
    {
        [Fact]
        public void RequiredNotNullReturnsThisWhenNotNull()
        {
            var nonNullObject = new object();
            Assert.Equal(nonNullObject, nonNullObject.RequiredNotNull());
        }

        [Fact]
        public void RequiredNotNullThrowsExceptionWhenNull()
        {
            Assert.Throws<InvalidStateException>(() => ((object)null).RequiredNotNull());
        }
    }
}
