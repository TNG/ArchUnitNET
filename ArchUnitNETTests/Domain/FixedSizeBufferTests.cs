using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class FixedSizeBufferTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(FixedSizeBufferTests).Assembly)
            .Build();

        private readonly IType _structWithUnsafeContent;

        public FixedSizeBufferTests()
        {
            _structWithUnsafeContent = Architecture.GetITypeOfType(
                typeof(StructWithFixedSizeBuffer)
            );
        }

        [Fact]
        public void HandleFixedSizeBuffersCorrectly()
        {
            var fieldMembers = _structWithUnsafeContent.GetFieldMembers().ToList();
            Assert.Single(fieldMembers);
            Assert.False(fieldMembers[0].Type.Type.IsCompilerGenerated);
            var fieldTypeDependency = _structWithUnsafeContent.GetFieldTypeDependencies().First();
            Assert.Equal(typeof(char).FullName, fieldTypeDependency.Target.FullName);
            Assert.Equal(new List<int> { 1 }, fieldTypeDependency.TargetArrayDimensions);
            Assert.True(fieldTypeDependency.TargetIsArray);
        }
    }

    public struct StructWithFixedSizeBuffer
    {
        public unsafe fixed char FixedCharArray[256];
    }
}
