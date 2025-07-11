using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using TypeDependencyNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class DependencyAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.DependencyArchitecture;

    public Class BaseClass;
    public Type BaseClassSystemType = typeof(BaseClass);

    public Class ChildClass;
    public Type ChildClassSystemType = typeof(ChildClass);

    public Class OtherChildClass;
    public Type OtherChildClassSystemType = typeof(OtherChildClass);

    public Class BaseClassWithMember;
    public Type BaseClassWithMemberSystemType = typeof(BaseClassWithMember);

    public Class ChildClassWithMember;
    public Type ChildClassWithMemberSystemType = typeof(ChildClassWithMember);

    public Class OtherChildClassWithMember;
    public Type OtherChildClassWithMemberSystemType = typeof(OtherChildClassWithMember);

    public Class BaseClassWithMultipleDependencies;
    public Type BaseClassWithMultipleDependenciesSystemType =
        typeof(BaseClassWithMultipleDependencies);

    public Class ChildClass1;
    public Type ChildClass1SystemType = typeof(ChildClass1);

    public Class ChildClass2;
    public Type ChildClass2SystemType = typeof(ChildClass2);

    public Class OtherBaseClass;
    public Type OtherBaseClassSystemType = typeof(OtherBaseClass);

    public Class ClassWithMultipleDependencies;
    public Type ClassWithMultipleDependenciesSystemType = typeof(ClassWithMultipleDependencies);

    public Class ClassWithoutDependencies;
    public Type ClassWithoutDependenciesSystemType = typeof(ClassWithoutDependencies);

    public Class OtherClassWithoutDependencies;
    public Type OtherClassWithoutDependenciesSystemType = typeof(OtherClassWithoutDependencies);

    public MethodMember MethodWithSingleDependency;

    public MethodMember CalledMethod;

    public MethodMember MethodWithMultipleDependencies;

    public MethodMember CalledMethod1;

    public MethodMember CalledMethod2;

    public MethodMember CalledMethod3;

    public MethodMember UnusedMethod;

    public MethodMember MethodWithoutDependencies;

    public DependencyAssemblyTestHelper()
    {
        BaseClass = Architecture.GetClassOfType(typeof(BaseClass));
        ChildClass = Architecture.GetClassOfType(typeof(ChildClass));
        OtherChildClass = Architecture.GetClassOfType(typeof(OtherChildClass));
        BaseClassWithMember = Architecture.GetClassOfType(typeof(BaseClassWithMember));
        ChildClassWithMember = Architecture.GetClassOfType(typeof(ChildClassWithMember));
        OtherChildClassWithMember = Architecture.GetClassOfType(typeof(OtherChildClassWithMember));
        BaseClassWithMultipleDependencies = Architecture.GetClassOfType(
            typeof(BaseClassWithMultipleDependencies)
        );
        ChildClass1 = Architecture.GetClassOfType(typeof(ChildClass1));
        ChildClass2 = Architecture.GetClassOfType(typeof(ChildClass2));
        OtherBaseClass = Architecture.GetClassOfType(typeof(OtherBaseClass));
        ClassWithMultipleDependencies = Architecture.GetClassOfType(
            typeof(ClassWithMultipleDependencies)
        );
        ClassWithoutDependencies = Architecture.GetClassOfType(typeof(ClassWithoutDependencies));
        OtherClassWithoutDependencies = Architecture.GetClassOfType(
            typeof(OtherClassWithoutDependencies)
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
    }
}
