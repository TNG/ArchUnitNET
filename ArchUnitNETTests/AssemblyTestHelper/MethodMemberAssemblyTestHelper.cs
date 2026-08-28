using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using MethodMemberNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class MethodMemberAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.MethodMemberArchitecture;

    public readonly Class RegularClass;
    public readonly Type RegularClassSystemType = typeof(RegularClass);

    public readonly Class OtherRegularClass;
    public readonly Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    private readonly Class ClassWithVirtualMethod;
    private readonly Class OtherClassWithVirtualMethod;
    private readonly Class ClassWithNonVirtualMethod;
    private readonly Class ClassWithStringReturnType;
    private readonly Class ClassWithRegularClassReturnType;
    private readonly Class ClassWithOtherRegularClassReturnType;
    private readonly Class ClassWithGenericReturnType;

    public readonly MethodMember VirtualMethod;
    public readonly MethodMember OtherVirtualMethod;
    public readonly MethodMember NonVirtualMethod;
    public readonly MethodMember ClassWithVirtualMethodConstructor;
    public readonly MethodMember ClassWithNonVirtualMethodConstructor;
    public readonly MethodMember MethodReturningString;
    public readonly MethodMember MethodReturningRegularClass;
    public readonly MethodMember MethodReturningOtherRegularClass;
    public readonly MethodMember MethodReturningGenericClass;
    public readonly MethodMember MethodReturningTwoArgGenericClass;

    private readonly Class ClassWithAmbiguousReturnType;
    public readonly MethodMember MethodWithAmbiguousReturnType;
    public readonly IType AmbiguousReturnType;

    public readonly Class MethodDependencyClass;
    public readonly Type MethodDependencyClassSystemType = typeof(MethodDependencyClass);

    public readonly Class OtherCallingClass;
    public readonly Type OtherCallingClassSystemType = typeof(OtherCallingClass);

    public readonly MethodMember CalledMethod;
    public readonly MethodMember OtherCalledMethod;
    public readonly MethodMember MethodWithoutDependencies;
    public readonly MethodMember MethodCallingCalledMethod;
    public readonly MethodMember AnotherMethodCallingCalledMethod;

    public MethodMemberAssemblyTestHelper()
    {
        RegularClass = Architecture.GetClassOfType(typeof(RegularClass));
        OtherRegularClass = Architecture.GetClassOfType(typeof(OtherRegularClass));

        ClassWithVirtualMethod = Architecture.GetClassOfType(typeof(ClassWithVirtualMethod));
        OtherClassWithVirtualMethod = Architecture.GetClassOfType(
            typeof(OtherClassWithVirtualMethod)
        );
        ClassWithNonVirtualMethod = Architecture.GetClassOfType(typeof(ClassWithNonVirtualMethod));
        ClassWithStringReturnType = Architecture.GetClassOfType(typeof(ClassWithStringReturnType));
        ClassWithRegularClassReturnType = Architecture.GetClassOfType(
            typeof(ClassWithRegularClassReturnType)
        );
        ClassWithOtherRegularClassReturnType = Architecture.GetClassOfType(
            typeof(ClassWithOtherRegularClassReturnType)
        );
        ClassWithGenericReturnType = Architecture.GetClassOfType(
            typeof(ClassWithGenericReturnType)
        );

        VirtualMethod = ClassWithVirtualMethod.GetMethodMembersWithName("VirtualMethod()").First();
        OtherVirtualMethod = OtherClassWithVirtualMethod
            .GetMethodMembersWithName("OtherVirtualMethod()")
            .First();
        NonVirtualMethod = ClassWithNonVirtualMethod
            .GetMethodMembersWithName("NonVirtualMethod()")
            .First();
        ClassWithVirtualMethodConstructor = ClassWithVirtualMethod
            .GetMethodMembersWithName(".ctor()")
            .First();
        ClassWithNonVirtualMethodConstructor = ClassWithNonVirtualMethod
            .GetMethodMembersWithName(".ctor()")
            .First();
        MethodReturningString = ClassWithStringReturnType
            .GetMethodMembersWithName("MethodReturningString()")
            .First();
        MethodReturningRegularClass = ClassWithRegularClassReturnType
            .GetMethodMembersWithName("MethodReturningRegularClass()")
            .First();
        MethodReturningOtherRegularClass = ClassWithOtherRegularClassReturnType
            .GetMethodMembersWithName("MethodReturningOtherRegularClass()")
            .First();
        MethodReturningGenericClass = ClassWithGenericReturnType
            .GetMethodMembersWithName("MethodReturningGenericClass()")
            .First();
        MethodReturningTwoArgGenericClass = ClassWithGenericReturnType
            .GetMethodMembersWithName("MethodReturningTwoArgGenericClass()")
            .First();

        ClassWithAmbiguousReturnType = Architecture.GetClassOfType(
            typeof(DuplicateFullNameAssembly.ClassWithAmbiguousReturnType)
        );
        MethodWithAmbiguousReturnType = ClassWithAmbiguousReturnType
            .GetMethodMembersWithName("MethodWithAmbiguousReturnType()")
            .First();
        AmbiguousReturnType = MethodWithAmbiguousReturnType.ReturnType;

        MethodDependencyClass = Architecture.GetClassOfType(typeof(MethodDependencyClass));
        OtherCallingClass = Architecture.GetClassOfType(typeof(OtherCallingClass));

        CalledMethod = MethodDependencyClass.GetMethodMembersWithName("CalledMethod()").First();
        OtherCalledMethod = MethodDependencyClass
            .GetMethodMembersWithName("OtherCalledMethod()")
            .First();
        MethodWithoutDependencies = MethodDependencyClass
            .GetMethodMembersWithName("MethodWithoutDependencies()")
            .First();
        MethodCallingCalledMethod = OtherCallingClass
            .GetMethodMembersWithName("MethodCallingCalledMethod()")
            .First();
        AnotherMethodCallingCalledMethod = OtherCallingClass
            .GetMethodMembersWithName("AnotherMethodCallingCalledMethod()")
            .First();
    }
}
