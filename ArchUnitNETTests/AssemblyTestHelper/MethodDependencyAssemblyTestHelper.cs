using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class MethodDependencyAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.MethodDependencyArchitecture;

    public readonly Class MethodDependencyClass;
    public readonly System.Type MethodDependencyClassSystemType =
        typeof(MethodDependencyNamespace.MethodDependencyClass);

    public readonly Class OtherCallingClass;
    public readonly System.Type OtherCallingClassSystemType =
        typeof(MethodDependencyNamespace.OtherCallingClass);

    public readonly MethodMember MethodWithSingleDependency;

    public readonly MethodMember CalledMethod;

    public readonly MethodMember MethodWithMultipleDependencies;

    public readonly MethodMember CalledMethod1;

    public readonly MethodMember CalledMethod2;

    private readonly MethodMember CalledMethod3;

    public readonly MethodMember MethodWithoutDependencies;

    public readonly MethodMember MethodCallingCalledMethod;

    public readonly MethodMember AnotherMethodCallingCalledMethod;

    public MethodDependencyAssemblyTestHelper()
    {
        MethodDependencyClass = Architecture.GetClassOfType(
            typeof(MethodDependencyNamespace.MethodDependencyClass)
        );
        OtherCallingClass = Architecture.GetClassOfType(
            typeof(MethodDependencyNamespace.OtherCallingClass)
        );
        MethodWithSingleDependency = Architecture
            .MethodMembers.WhereNameIs("MethodWithSingleDependency()")
            .First();
        CalledMethod = Architecture.MethodMembers.WhereNameIs("CalledMethod()").First();
        MethodWithMultipleDependencies = Architecture
            .MethodMembers.WhereNameIs("MethodWithMultipleDependencies()")
            .First();
        CalledMethod1 = Architecture.MethodMembers.WhereNameIs("CalledMethod1()").First();
        CalledMethod2 = Architecture.MethodMembers.WhereNameIs("CalledMethod2()").First();
        CalledMethod3 = Architecture.MethodMembers.WhereNameIs("CalledMethod3()").First();
        MethodWithoutDependencies = Architecture
            .MethodMembers.WhereNameIs("MethodWithoutDependencies()")
            .First();
        MethodCallingCalledMethod = Architecture
            .MethodMembers.WhereNameIs("MethodCallingCalledMethod()")
            .First();
        AnotherMethodCallingCalledMethod = Architecture
            .MethodMembers.WhereNameIs("AnotherMethodCallingCalledMethod()")
            .First();
    }
}
