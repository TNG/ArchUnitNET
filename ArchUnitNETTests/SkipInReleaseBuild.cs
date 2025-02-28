using Xunit;

namespace ArchUnitNETTests
{
    public sealed class SkipInReleaseBuild : FactAttribute
    {
        public SkipInReleaseBuild()
        {
#if !DEBUG
            Skip = "This test only works in debug build";
#endif
        }
    }

    public sealed class SkipInReleaseBuildTheory : TheoryAttribute
    {
        public SkipInReleaseBuildTheory()
        {
#if !DEBUG
            Skip = "This test only works in debug build";
#endif
        }
    }
}
