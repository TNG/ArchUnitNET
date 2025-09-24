using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using InterfaceAssembly;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class InterfaceAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.InterfaceArchitecture;

    public Interface BaseInterface;
    public Type BaseInterfaceSystemType = typeof(IBaseInterface);

    public Interface OtherBaseInterface;
    public Type OtherBaseInterfaceSystemType = typeof(IOtherBaseInterface);

    public Interface ChildInterface;
    public Type ChildInterfaceSystemType = typeof(IChildInterface);

    public Interface OtherChildInterface;
    public Type OtherChildInterfaceSystemType = typeof(IOtherChildInterface);

    public Interface InterfaceWithMultipleDependencies;
    public Type InterfaceWithMultipleDependenciesSystemType =
        typeof(IInterfaceWithMultipleDependencies);

    public Interface InterfaceWithoutDependencies;
    public Type InterfaceWithoutDependenciesSystemType = typeof(IInterfaceWithoutDependencies);

    public InterfaceAssemblyTestHelper()
    {
        BaseInterface = Architecture.GetInterfaceOfType(typeof(IBaseInterface));
        OtherBaseInterface = Architecture.GetInterfaceOfType(typeof(IOtherBaseInterface));
        ChildInterface = Architecture.GetInterfaceOfType(typeof(IChildInterface));
        OtherChildInterface = Architecture.GetInterfaceOfType(typeof(IOtherChildInterface));
        InterfaceWithMultipleDependencies = Architecture.GetInterfaceOfType(
            typeof(IInterfaceWithMultipleDependencies)
        );
        InterfaceWithoutDependencies = Architecture.GetInterfaceOfType(
            typeof(IInterfaceWithoutDependencies)
        );
    }
}
