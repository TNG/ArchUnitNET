using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNETTests.AssemblyTestHelper;

/// <summary>
/// A helper over an architecture with two loaded assemblies. It exists solely for the assembly-related
/// syntax elements: their moreAssemblies slot can only be tested with an assembly that is loaded but does
/// not match, and GetAssemblyOfAssembly throws for an assembly that is not loaded at all.
/// </summary>
public class TypeAndClassAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.TypeAndClassArchitecture;

    public readonly Class RegularClass;
    public readonly Class OtherRegularClass;

    public readonly System.Reflection.Assembly TypeReflectionAssembly =
        typeof(TypeNamespace.RegularClass).Assembly;
    public readonly System.Reflection.Assembly ClassReflectionAssembly =
        typeof(ClassNamespace.RegularClass).Assembly;

    public readonly Assembly TypeArchAssembly;
    public readonly Assembly ClassArchAssembly;

    public TypeAndClassAssemblyTestHelper()
    {
        RegularClass = Architecture.GetClassOfType(typeof(TypeNamespace.RegularClass));
        OtherRegularClass = Architecture.GetClassOfType(typeof(TypeNamespace.OtherRegularClass));

        TypeArchAssembly = Architecture.GetAssemblyOfAssembly(TypeReflectionAssembly);
        ClassArchAssembly = Architecture.GetAssemblyOfAssembly(ClassReflectionAssembly);
    }
}
