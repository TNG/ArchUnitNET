using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ClassNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class ClassAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture => StaticTestArchitectures.ClassArchitecture;

    public Class RegularClass;
    public Type RegularClassSystemType = typeof(RegularClass);

    public Class OtherRegularClass;
    public Type OtherRegularClassSystemType = typeof(OtherRegularClass);

    public Class AbstractClass;
    public Type AbstractClassSystemType = typeof(AbstractClass);

    public Class OtherAbstractClass;
    public Type OtherAbstractClassSystemType = typeof(OtherAbstractClass);

    public Class SealedClass;
    public Type SealedClassSystemType = typeof(SealedClass);

    public Class OtherSealedClass;
    public Type OtherSealedClassSystemType = typeof(OtherSealedClass);

    public Class RecordClass;
    public Type RecordClassSystemType = typeof(RecordClass);

    public Class OtherRecordClass;
    public Type OtherRecordClassSystemType = typeof(OtherRecordClass);

    public Class ImmutableClass;
    public Type ImmutableClassSystemType = typeof(ImmutableClass);

    public Class OtherImmutableClass;
    public Type OtherImmutableClassSystemType = typeof(OtherImmutableClass);

    public Class MutableClass;
    public Type MutableClassSystemType = typeof(MutableClass);

    public Class OtherMutableClass;
    public Type OtherMutableClassSystemType = typeof(OtherMutableClass);

    public Class ClassWithoutMembers;
    public Type ClassWithoutMembersSystemType = typeof(ClassWithoutMembers);

    public Class ClassWithOnlyStaticMembers;
    public Type ClassWithOnlyStaticMembersSystemType = typeof(ClassWithOnlyStaticMembers);

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
    }
}
