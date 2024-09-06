using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ArchUnitNET.Fluent;

namespace ArchUnitNETTests.AssemblyTestHelper;

public static class AssemblyTestHelperExtensions
{
    public static void AssertNoViolations(this IArchRule archRule, AssemblyTestHelper testHelper)
    {
        testHelper.AssertNoViolations(archRule);
    }

    public static void AssertAnyViolations(this IArchRule archRule, AssemblyTestHelper testHelper)
    {
        testHelper.AssertAnyViolations(archRule);
    }

    public static void AssertOnlyViolations(this IArchRule archRule, AssemblyTestHelper testHelper)
    {
        testHelper.AssertOnlyViolations(archRule);
    }
}
