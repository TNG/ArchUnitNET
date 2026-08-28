using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using TypeNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class TypeAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture => StaticTestArchitectures.TypeArchitecture;

    // Enums
    public readonly IType SimpleEnum;

    public readonly IType OtherEnum;

    // Structs
    public readonly IType SimpleStruct;

    public readonly IType OtherStruct;

    // Regular classes
    public readonly Class RegularClass;
    public readonly Type RegularClassSystemType = typeof(RegularClass);

    public readonly Class OtherRegularClass;
    public readonly Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    // Member classes
    public readonly Class ClassWithProperty;
    public readonly Type ClassWithPropertySystemType = typeof(ClassWithProperty);

    public readonly Class ClassWithField;

    public readonly Class ClassWithMethod;

    public readonly Class ClassWithAllMembers;

    public readonly Class ClassWithoutMembers;

    // Nested classes
    public readonly Class OuterClassA;
    public readonly Type OuterClassASystemType = typeof(OuterClassA);

    public readonly Class OuterClassB;
    public readonly Type OuterClassBSystemType = typeof(OuterClassB);

    public readonly Class InnerClassA;

    public readonly Class OtherInnerClassA;

    public readonly Class InnerClassB;

    public readonly Class NonNestedClass;
    public readonly Type NonNestedClassSystemType = typeof(NonNestedClass);

    // Interfaces
    public readonly Interface TestInterface;
    public readonly Type TestInterfaceSystemType = typeof(ITestInterface);

    public readonly Interface OtherTestInterface;
    public readonly Type OtherTestInterfaceSystemType = typeof(IOtherTestInterface);

    public readonly Interface ChildTestInterface;

    public readonly Interface OtherChildTestInterface;
    public readonly Type OtherChildTestInterfaceSystemType = typeof(IOtherChildTestInterface);

    // Interface implementation
    public readonly Class ClassImplementingInterface;

    public readonly Class ClassNotImplementingInterface;

    // Assignability
    public readonly Class BaseClassForAssign;
    public readonly Type BaseClassForAssignSystemType = typeof(BaseClassForAssign);

    public readonly Class OtherBaseClassForAssign;
    public readonly Type OtherBaseClassForAssignSystemType = typeof(OtherBaseClassForAssign);

    public readonly Class DerivedClassForAssign;

    public readonly Class OtherDerivedClassForAssign;

    public readonly Class UnrelatedClassForAssign;

    public readonly Class OtherUnrelatedClassForAssign;

    // Member test classes
    public readonly Class ClassWithStaticField;
    public readonly Type ClassWithStaticFieldSystemType = typeof(ClassWithStaticField);

    public readonly Class ClassWithNonStaticField;
    public readonly Type ClassWithNonStaticFieldSystemType = typeof(ClassWithNonStaticField);

    public readonly Class ClassWithReadOnlyField;
    public readonly Type ClassWithReadOnlyFieldSystemType = typeof(ClassWithReadOnlyField);

    private readonly Class ClassWithWritableProperty;

    private readonly Class OtherClassWithWritableProperty;

    private readonly Class ClassWithInitOnlyProperty;

    private readonly Class ClassWithGetOnlyProperty;

    public readonly Class OtherClassWithStaticField;

    private readonly Class ClassWithStaticProperty;

    private readonly Class ClassWithStaticMethod;

    // Method member test classes
    private readonly Class ClassWithVirtualMethod;

    private readonly Class OtherClassWithVirtualMethod;

    private readonly Class ClassWithNonVirtualMethod;

    private readonly Class ClassWithStringReturnType;

    private readonly Class ClassWithIntReturnType;

    private readonly Class ClassWithRegularClassReturnType;

    private readonly Class ClassWithOtherRegularClassReturnType;

    // Generic return type test classes

    private readonly Class ClassWithGenericReturnType;

    // Individual members
    public readonly IMember StaticField;
    public readonly IMember NonStaticField;
    public readonly IMember ReadOnlyField;
    public readonly IMember WritableProperty;
    public readonly IMember OtherWritableProperty;
    public readonly IMember InitOnlyProperty;
    public readonly IMember GetOnlyProperty;
    public readonly IMember OtherStaticField;
    private readonly IMember StaticProperty;
    private readonly IMember StaticMethod;

    // Method members (MethodMember type)
    public readonly MethodMember VirtualMethod;
    public readonly MethodMember OtherVirtualMethod;
    public readonly MethodMember NonVirtualMethod;
    public readonly MethodMember MethodReturningString;
    private readonly MethodMember MethodReturningInt;
    public readonly MethodMember ClassWithVirtualMethodConstructor;
    public readonly MethodMember ClassWithNonVirtualMethodConstructor;
    public readonly MethodMember MethodReturningRegularClass;
    public readonly MethodMember MethodReturningOtherRegularClass;
    public readonly MethodMember MethodReturningGenericClass;
    private readonly MethodMember MethodReturningGenericClassWithOtherArg;
    public readonly MethodMember MethodReturningTwoArgGenericClass;

    public TypeAssemblyTestHelper()
    {
        SimpleEnum = Architecture.GetITypeOfType(typeof(SimpleEnum));
        OtherEnum = Architecture.GetITypeOfType(typeof(OtherEnum));
        SimpleStruct = Architecture.GetITypeOfType(typeof(SimpleStruct));
        OtherStruct = Architecture.GetITypeOfType(typeof(OtherStruct));

        RegularClass = Architecture.GetClassOfType(typeof(RegularClass));
        OtherRegularClass = Architecture.GetClassOfType(typeof(OtherRegularClass));

        ClassWithProperty = Architecture.GetClassOfType(typeof(ClassWithProperty));
        ClassWithField = Architecture.GetClassOfType(typeof(ClassWithField));
        ClassWithMethod = Architecture.GetClassOfType(typeof(ClassWithMethod));
        ClassWithAllMembers = Architecture.GetClassOfType(typeof(ClassWithAllMembers));
        ClassWithoutMembers = Architecture.GetClassOfType(typeof(ClassWithoutMembers));

        OuterClassA = Architecture.GetClassOfType(typeof(OuterClassA));
        OuterClassB = Architecture.GetClassOfType(typeof(OuterClassB));
        InnerClassA = Architecture.GetClassOfType(typeof(OuterClassA.InnerClassA));
        OtherInnerClassA = Architecture.GetClassOfType(typeof(OuterClassA.OtherInnerClassA));
        InnerClassB = Architecture.GetClassOfType(typeof(OuterClassB.InnerClassB));
        NonNestedClass = Architecture.GetClassOfType(typeof(NonNestedClass));

        TestInterface = Architecture.GetInterfaceOfType(typeof(ITestInterface));
        OtherTestInterface = Architecture.GetInterfaceOfType(typeof(IOtherTestInterface));
        ChildTestInterface = Architecture.GetInterfaceOfType(typeof(IChildTestInterface));
        OtherChildTestInterface = Architecture.GetInterfaceOfType(typeof(IOtherChildTestInterface));

        ClassImplementingInterface = Architecture.GetClassOfType(
            typeof(ClassImplementingInterface)
        );
        ClassNotImplementingInterface = Architecture.GetClassOfType(
            typeof(ClassNotImplementingInterface)
        );

        BaseClassForAssign = Architecture.GetClassOfType(typeof(BaseClassForAssign));
        OtherBaseClassForAssign = Architecture.GetClassOfType(typeof(OtherBaseClassForAssign));
        DerivedClassForAssign = Architecture.GetClassOfType(typeof(DerivedClassForAssign));
        OtherDerivedClassForAssign = Architecture.GetClassOfType(
            typeof(OtherDerivedClassForAssign)
        );
        UnrelatedClassForAssign = Architecture.GetClassOfType(typeof(UnrelatedClassForAssign));
        OtherUnrelatedClassForAssign = Architecture.GetClassOfType(
            typeof(OtherUnrelatedClassForAssign)
        );

        // Member test classes
        ClassWithStaticField = Architecture.GetClassOfType(typeof(ClassWithStaticField));
        ClassWithNonStaticField = Architecture.GetClassOfType(typeof(ClassWithNonStaticField));
        ClassWithReadOnlyField = Architecture.GetClassOfType(typeof(ClassWithReadOnlyField));
        ClassWithWritableProperty = Architecture.GetClassOfType(typeof(ClassWithWritableProperty));
        OtherClassWithWritableProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithWritableProperty)
        );
        ClassWithInitOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithInitOnlyProperty));
        ClassWithGetOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithGetOnlyProperty));
        OtherClassWithStaticField = Architecture.GetClassOfType(typeof(OtherClassWithStaticField));
        ClassWithStaticProperty = Architecture.GetClassOfType(typeof(ClassWithStaticProperty));
        ClassWithStaticMethod = Architecture.GetClassOfType(typeof(ClassWithStaticMethod));

        // Individual members
        StaticField = ClassWithStaticField.GetFieldMembersWithName("StaticField").First();
        NonStaticField = ClassWithNonStaticField.GetFieldMembersWithName("NonStaticField").First();
        ReadOnlyField = ClassWithReadOnlyField.GetFieldMembersWithName("ReadOnlyField").First();
        WritableProperty = ClassWithWritableProperty
            .GetPropertyMembersWithName("WritableProperty")
            .First();
        OtherWritableProperty = OtherClassWithWritableProperty
            .GetPropertyMembersWithName("OtherWritableProperty")
            .First();
        InitOnlyProperty = ClassWithInitOnlyProperty
            .GetPropertyMembersWithName("InitOnlyProperty")
            .First();
        GetOnlyProperty = ClassWithGetOnlyProperty
            .GetPropertyMembersWithName("GetOnlyProperty")
            .First();
        OtherStaticField = OtherClassWithStaticField
            .GetFieldMembersWithName("OtherStaticField")
            .First();
        StaticProperty = ClassWithStaticProperty
            .GetPropertyMembersWithName("StaticProperty")
            .First();
        StaticMethod = ClassWithStaticMethod.GetMethodMembersWithName("StaticMethod()").First();

        // Method member test classes
        ClassWithVirtualMethod = Architecture.GetClassOfType(typeof(ClassWithVirtualMethod));
        OtherClassWithVirtualMethod = Architecture.GetClassOfType(
            typeof(OtherClassWithVirtualMethod)
        );
        ClassWithNonVirtualMethod = Architecture.GetClassOfType(typeof(ClassWithNonVirtualMethod));
        ClassWithStringReturnType = Architecture.GetClassOfType(typeof(ClassWithStringReturnType));
        ClassWithIntReturnType = Architecture.GetClassOfType(typeof(ClassWithIntReturnType));
        ClassWithRegularClassReturnType = Architecture.GetClassOfType(
            typeof(ClassWithRegularClassReturnType)
        );
        ClassWithOtherRegularClassReturnType = Architecture.GetClassOfType(
            typeof(ClassWithOtherRegularClassReturnType)
        );

        // Generic return type test classes
        ClassWithGenericReturnType = Architecture.GetClassOfType(
            typeof(ClassWithGenericReturnType)
        );

        // Method members (MethodMember type)
        VirtualMethod = ClassWithVirtualMethod.GetMethodMembersWithName("VirtualMethod()").First();
        OtherVirtualMethod = OtherClassWithVirtualMethod
            .GetMethodMembersWithName("OtherVirtualMethod()")
            .First();
        NonVirtualMethod = ClassWithNonVirtualMethod
            .GetMethodMembersWithName("NonVirtualMethod()")
            .First();
        MethodReturningString = ClassWithStringReturnType
            .GetMethodMembersWithName("MethodReturningString()")
            .First();
        MethodReturningInt = ClassWithIntReturnType
            .GetMethodMembersWithName("MethodReturningInt()")
            .First();
        ClassWithVirtualMethodConstructor = ClassWithVirtualMethod
            .GetMethodMembersWithName(".ctor()")
            .First();
        ClassWithNonVirtualMethodConstructor = ClassWithNonVirtualMethod
            .GetMethodMembersWithName(".ctor()")
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
        MethodReturningGenericClassWithOtherArg = ClassWithGenericReturnType
            .GetMethodMembersWithName("MethodReturningGenericClassWithOtherArg()")
            .First();
        MethodReturningTwoArgGenericClass = ClassWithGenericReturnType
            .GetMethodMembersWithName("MethodReturningTwoArgGenericClass()")
            .First();
    }
}
