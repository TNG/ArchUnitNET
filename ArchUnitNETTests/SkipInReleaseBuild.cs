using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Xunit;

namespace ArchUnitNETTests
{
    public sealed class SkipInReleaseBuild : FactAttribute
    {
        public SkipInReleaseBuild(
            [CanBeNull] [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = -1
        )
            : base(sourceFilePath, sourceLineNumber)
        {
#if !DEBUG
            Skip = "This test only works in debug build";
#endif
        }
    }

    public sealed class SkipInReleaseBuildTheory : TheoryAttribute
    {
        public SkipInReleaseBuildTheory(
            [CanBeNull] [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = -1
        )
            : base(sourceFilePath, sourceLineNumber)
        {
#if !DEBUG
            Skip = "This test only works in debug build";
#endif
        }
    }
}
