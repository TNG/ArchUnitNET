using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ClassNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class ClassAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture => StaticTestArchitectures.ClassArchitecture;

    public readonly Class RegularClass;
    public readonly Type RegularClassSystemType = typeof(RegularClass);

    public readonly Class OtherRegularClass;
    public readonly Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    public readonly Class AbstractClass;

    public readonly Class OtherAbstractClass;

    public readonly Class SealedClass;

    public readonly Class OtherSealedClass;

    public readonly Class RecordClass;

    public readonly Class OtherRecordClass;

    public readonly Class ImmutableClass;

    public readonly Class OtherImmutableClass;

    public readonly Class MutableClass;

    public readonly Class OtherMutableClass;

    public readonly Class ClassWithoutMembers;

    public readonly Class ClassWithOnlyStaticMembers;

    public readonly Class ClassWithImmutableInstanceAndMutableStaticMember;

    public ClassAssemblyTestHelper()
    {
        RegularClass = Architecture.GetClassOfType(typeof(RegularClass));
        OtherRegularClass = Architecture.GetClassOfType(typeof(OtherRegularClass));
        AbstractClass = Architecture.GetClassOfType(typeof(AbstractClass));
        OtherAbstractClass = Architecture.GetClassOfType(typeof(OtherAbstractClass));
        SealedClass = Architecture.GetClassOfType(typeof(SealedClass));
        OtherSealedClass = Architecture.GetClassOfType(typeof(OtherSealedClass));
        RecordClass = Architecture.GetClassOfType(typeof(RecordClass));
        OtherRecordClass = Architecture.GetClassOfType(typeof(OtherRecordClass));
        ImmutableClass = Architecture.GetClassOfType(typeof(ImmutableClass));
        OtherImmutableClass = Architecture.GetClassOfType(typeof(OtherImmutableClass));
        MutableClass = Architecture.GetClassOfType(typeof(MutableClass));
        OtherMutableClass = Architecture.GetClassOfType(typeof(OtherMutableClass));
        ClassWithoutMembers = Architecture.GetClassOfType(typeof(ClassWithoutMembers));
        ClassWithOnlyStaticMembers = Architecture.GetClassOfType(
            typeof(ClassWithOnlyStaticMembers)
        );
        ClassWithImmutableInstanceAndMutableStaticMember = Architecture.GetClassOfType(
            typeof(ClassWithImmutableInstanceAndMutableStaticMember)
        );
    }
}
