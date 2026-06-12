using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class MethodDependencyAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.MethodDependencyArchitecture;

    public MethodMember MethodWithSingleDependency;

    public MethodMember CalledMethod;

    public MethodMember MethodWithMultipleDependencies;

    public MethodMember CalledMethod1;

    public MethodMember CalledMethod2;

    public MethodMember CalledMethod3;

    public MethodMember MethodWithoutDependencies;

    public MethodDependencyAssemblyTestHelper()
    {
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
    }
}
