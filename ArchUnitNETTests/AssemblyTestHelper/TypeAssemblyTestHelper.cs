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
    public IType SimpleEnum;
    public Type SimpleEnumSystemType = typeof(SimpleEnum);

    public IType OtherEnum;
    public Type OtherEnumSystemType = typeof(OtherEnum);

    // Structs
    public IType SimpleStruct;
    public Type SimpleStructSystemType = typeof(SimpleStruct);

    public IType OtherStruct;
    public Type OtherStructSystemType = typeof(OtherStruct);

    // Regular classes
    public Class RegularClass;
    public Type RegularClassSystemType = typeof(RegularClass);

    public Class OtherRegularClass;
    public Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    // Member classes
    public Class ClassWithProperty;
    public Type ClassWithPropertySystemType = typeof(ClassWithProperty);

    public Class ClassWithField;
    public Type ClassWithFieldSystemType = typeof(ClassWithField);

    public Class ClassWithMethod;
    public Type ClassWithMethodSystemType = typeof(ClassWithMethod);

    public Class ClassWithAllMembers;
    public Type ClassWithAllMembersSystemType = typeof(ClassWithAllMembers);

    public Class ClassWithoutMembers;
    public Type ClassWithoutMembersSystemType = typeof(ClassWithoutMembers);

    // Nested classes
    public Class OuterClassA;
    public Type OuterClassASystemType = typeof(OuterClassA);

    public Class OuterClassB;
    public Type OuterClassBSystemType = typeof(OuterClassB);

    public Class InnerClassA;
    public Type InnerClassASystemType = typeof(OuterClassA.InnerClassA);

    public Class OtherInnerClassA;
    public Type OtherInnerClassASystemType = typeof(OuterClassA.OtherInnerClassA);

    public Class InnerClassB;
    public Type InnerClassBSystemType = typeof(OuterClassB.InnerClassB);

    public Class NonNestedClass;
    public Type NonNestedClassSystemType = typeof(NonNestedClass);

    // Interfaces
    public Interface TestInterface;
    public Type TestInterfaceSystemType = typeof(ITestInterface);

    public Interface OtherTestInterface;
    public Type OtherTestInterfaceSystemType = typeof(IOtherTestInterface);

    public Interface ChildTestInterface;
    public Type ChildTestInterfaceSystemType = typeof(IChildTestInterface);

    public Interface OtherChildTestInterface;
    public Type OtherChildTestInterfaceSystemType = typeof(IOtherChildTestInterface);

    public Interface MultiParentTestInterface;
    public Type MultiParentTestInterfaceSystemType = typeof(IMultiParentTestInterface);

    public Interface StandaloneTestInterface;
    public Type StandaloneTestInterfaceSystemType = typeof(IStandaloneTestInterface);

    // Interface implementation
    public Class ClassImplementingInterface;
    public Type ClassImplementingInterfaceSystemType = typeof(ClassImplementingInterface);

    public Class ClassNotImplementingInterface;
    public Type ClassNotImplementingInterfaceSystemType = typeof(ClassNotImplementingInterface);

    // Assignability
    public Class BaseClassForAssign;
    public Type BaseClassForAssignSystemType = typeof(BaseClassForAssign);

    public Class OtherBaseClassForAssign;
    public Type OtherBaseClassForAssignSystemType = typeof(OtherBaseClassForAssign);

    public Class DerivedClassForAssign;
    public Type DerivedClassForAssignSystemType = typeof(DerivedClassForAssign);

    public Class OtherDerivedClassForAssign;
    public Type OtherDerivedClassForAssignSystemType = typeof(OtherDerivedClassForAssign);

    public Class UnrelatedClassForAssign;
    public Type UnrelatedClassForAssignSystemType = typeof(UnrelatedClassForAssign);

    public Class OtherUnrelatedClassForAssign;
    public Type OtherUnrelatedClassForAssignSystemType = typeof(OtherUnrelatedClassForAssign);

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
        MultiParentTestInterface = Architecture.GetInterfaceOfType(
            typeof(IMultiParentTestInterface)
        );
        StandaloneTestInterface = Architecture.GetInterfaceOfType(typeof(IStandaloneTestInterface));

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
    }
}
