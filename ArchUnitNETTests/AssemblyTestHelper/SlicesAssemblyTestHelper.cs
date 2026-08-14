using ArchUnitNET.Domain;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class SlicesAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.SlicesTestArchitecture;
}
