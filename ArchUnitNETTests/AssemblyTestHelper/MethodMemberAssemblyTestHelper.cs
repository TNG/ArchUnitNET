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

    public Class RegularClass;
    public Type RegularClassSystemType = typeof(RegularClass);

    public Class OtherRegularClass;
    public Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    public Class ClassWithVirtualMethod;
    public Class OtherClassWithVirtualMethod;
    public Class ClassWithNonVirtualMethod;
    public Class ClassWithStringReturnType;
    public Class ClassWithRegularClassReturnType;
    public Class ClassWithOtherRegularClassReturnType;
    public Class ClassWithGenericReturnType;

    public MethodMember VirtualMethod;
    public MethodMember OtherVirtualMethod;
    public MethodMember NonVirtualMethod;
    public MethodMember ClassWithVirtualMethodConstructor;
    public MethodMember ClassWithNonVirtualMethodConstructor;
    public MethodMember MethodReturningString;
    public MethodMember MethodReturningRegularClass;
    public MethodMember MethodReturningOtherRegularClass;
    public MethodMember MethodReturningGenericClass;
    public MethodMember MethodReturningTwoArgGenericClass;

    public Class ClassWithAmbiguousReturnType;
    public MethodMember MethodWithAmbiguousReturnType;
    public IType AmbiguousReturnType;

    public Class MethodDependencyClass;
    public Type MethodDependencyClassSystemType = typeof(MethodDependencyClass);

    public Class OtherCallingClass;
    public Type OtherCallingClassSystemType = typeof(OtherCallingClass);

    public MethodMember CalledMethod;
    public MethodMember OtherCalledMethod;
    public MethodMember MethodWithoutDependencies;
    public MethodMember MethodCallingCalledMethod;
    public MethodMember AnotherMethodCallingCalledMethod;

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
