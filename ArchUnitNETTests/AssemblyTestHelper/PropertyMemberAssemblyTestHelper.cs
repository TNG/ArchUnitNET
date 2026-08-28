using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using PropertyMemberNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class PropertyMemberAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.PropertyMemberArchitecture;

    private Class ClassWithWritableProperty;
    private Class OtherClassWithWritableProperty;
    private Class ClassWithInitOnlyProperty;
    private Class OtherClassWithInitOnlyProperty;
    private Class ClassWithGetOnlyProperty;
    private Class OtherClassWithGetOnlyProperty;
    private Class ClassWithVirtualProperty;
    private Class OtherClassWithVirtualProperty;
    private Class ClassWithNonVirtualProperty;
    private Class ClassWithWriteOnlyProperty;
    private Class OtherClassWithWriteOnlyProperty;
    private Class ClassWithPrivateGetterProperty;
    private Class OtherClassWithPrivateGetterProperty;
    private Class ClassWithProtectedGetterProperty;
    private Class OtherClassWithProtectedGetterProperty;
    private Class ClassWithInternalGetterProperty;
    private Class OtherClassWithInternalGetterProperty;
    private Class ClassWithProtectedInternalGetterProperty;
    private Class OtherClassWithProtectedInternalGetterProperty;
    private Class ClassWithPrivateProtectedGetterProperty;
    private Class OtherClassWithPrivateProtectedGetterProperty;
    private Class ClassWithPrivateSetterProperty;
    private Class OtherClassWithPrivateSetterProperty;
    private Class ClassWithProtectedSetterProperty;
    private Class OtherClassWithProtectedSetterProperty;
    private Class ClassWithInternalSetterProperty;
    private Class OtherClassWithInternalSetterProperty;
    private Class ClassWithProtectedInternalSetterProperty;
    private Class OtherClassWithProtectedInternalSetterProperty;
    private Class ClassWithPrivateProtectedSetterProperty;
    private Class OtherClassWithPrivateProtectedSetterProperty;

    // Individual members
    public PropertyMember WritableProperty;
    public PropertyMember OtherWritableProperty;
    public PropertyMember InitOnlyProperty;
    public PropertyMember OtherInitOnlyProperty;
    public PropertyMember GetOnlyProperty;
    public PropertyMember OtherGetOnlyProperty;

    // Property members (PropertyMember type)
    public PropertyMember VirtualProperty;
    public PropertyMember OtherVirtualProperty;
    public PropertyMember NonVirtualProperty;
    public PropertyMember WriteOnlyProperty;
    public PropertyMember OtherWriteOnlyProperty;
    public PropertyMember PropertyWithPrivateGetter;
    public PropertyMember OtherPropertyWithPrivateGetter;
    public PropertyMember PropertyWithProtectedGetter;
    public PropertyMember OtherPropertyWithProtectedGetter;
    public PropertyMember PropertyWithInternalGetter;
    public PropertyMember OtherPropertyWithInternalGetter;
    public PropertyMember PropertyWithProtectedInternalGetter;
    public PropertyMember OtherPropertyWithProtectedInternalGetter;
    public PropertyMember PropertyWithPrivateProtectedGetter;
    public PropertyMember OtherPropertyWithPrivateProtectedGetter;
    public PropertyMember PropertyWithPrivateSetter;
    public PropertyMember OtherPropertyWithPrivateSetter;
    public PropertyMember PropertyWithProtectedSetter;
    public PropertyMember OtherPropertyWithProtectedSetter;
    public PropertyMember PropertyWithInternalSetter;
    public PropertyMember OtherPropertyWithInternalSetter;
    public PropertyMember PropertyWithProtectedInternalSetter;
    public PropertyMember OtherPropertyWithProtectedInternalSetter;
    public PropertyMember PropertyWithPrivateProtectedSetter;
    public PropertyMember OtherPropertyWithPrivateProtectedSetter;

    public PropertyMemberAssemblyTestHelper()
    {
        ClassWithWritableProperty = Architecture.GetClassOfType(typeof(ClassWithWritableProperty));
        OtherClassWithWritableProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithWritableProperty)
        );
        ClassWithInitOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithInitOnlyProperty));
        OtherClassWithInitOnlyProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithInitOnlyProperty)
        );
        ClassWithGetOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithGetOnlyProperty));
        OtherClassWithGetOnlyProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithGetOnlyProperty)
        );

        ClassWithVirtualProperty = Architecture.GetClassOfType(typeof(ClassWithVirtualProperty));
        OtherClassWithVirtualProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithVirtualProperty)
        );
        ClassWithNonVirtualProperty = Architecture.GetClassOfType(
            typeof(ClassWithNonVirtualProperty)
        );
        ClassWithWriteOnlyProperty = Architecture.GetClassOfType(
            typeof(ClassWithWriteOnlyProperty)
        );
        OtherClassWithWriteOnlyProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithWriteOnlyProperty)
        );
        ClassWithPrivateGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateGetterProperty)
        );
        OtherClassWithPrivateGetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithPrivateGetterProperty)
        );
        ClassWithProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedGetterProperty)
        );
        OtherClassWithProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithProtectedGetterProperty)
        );
        ClassWithInternalGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithInternalGetterProperty)
        );
        OtherClassWithInternalGetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithInternalGetterProperty)
        );
        ClassWithProtectedInternalGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedInternalGetterProperty)
        );
        OtherClassWithProtectedInternalGetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithProtectedInternalGetterProperty)
        );
        ClassWithPrivateProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateProtectedGetterProperty)
        );
        OtherClassWithPrivateProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithPrivateProtectedGetterProperty)
        );
        ClassWithPrivateSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateSetterProperty)
        );
        OtherClassWithPrivateSetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithPrivateSetterProperty)
        );
        ClassWithProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedSetterProperty)
        );
        OtherClassWithProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithProtectedSetterProperty)
        );
        ClassWithInternalSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithInternalSetterProperty)
        );
        OtherClassWithInternalSetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithInternalSetterProperty)
        );
        ClassWithProtectedInternalSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedInternalSetterProperty)
        );
        OtherClassWithProtectedInternalSetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithProtectedInternalSetterProperty)
        );
        ClassWithPrivateProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateProtectedSetterProperty)
        );
        OtherClassWithPrivateProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithPrivateProtectedSetterProperty)
        );

        // Individual members
        WritableProperty = ClassWithWritableProperty
            .GetPropertyMembersWithName("WritableProperty")
            .First();
        OtherWritableProperty = OtherClassWithWritableProperty
            .GetPropertyMembersWithName("OtherWritableProperty")
            .First();
        InitOnlyProperty = ClassWithInitOnlyProperty
            .GetPropertyMembersWithName("InitOnlyProperty")
            .First();
        OtherInitOnlyProperty = OtherClassWithInitOnlyProperty
            .GetPropertyMembersWithName("OtherInitOnlyProperty")
            .First();
        GetOnlyProperty = ClassWithGetOnlyProperty
            .GetPropertyMembersWithName("GetOnlyProperty")
            .First();
        OtherGetOnlyProperty = OtherClassWithGetOnlyProperty
            .GetPropertyMembersWithName("OtherGetOnlyProperty")
            .First();

        // Property members (PropertyMember type)
        VirtualProperty = ClassWithVirtualProperty
            .GetPropertyMembersWithName("VirtualProperty")
            .First();
        OtherVirtualProperty = OtherClassWithVirtualProperty
            .GetPropertyMembersWithName("OtherVirtualProperty")
            .First();
        NonVirtualProperty = ClassWithNonVirtualProperty
            .GetPropertyMembersWithName("NonVirtualProperty")
            .First();
        WriteOnlyProperty = ClassWithWriteOnlyProperty
            .GetPropertyMembersWithName("WriteOnlyProperty")
            .First();
        OtherWriteOnlyProperty = OtherClassWithWriteOnlyProperty
            .GetPropertyMembersWithName("OtherWriteOnlyProperty")
            .First();
        PropertyWithPrivateGetter = ClassWithPrivateGetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateGetter")
            .First();
        OtherPropertyWithPrivateGetter = OtherClassWithPrivateGetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithPrivateGetter")
            .First();
        PropertyWithProtectedGetter = ClassWithProtectedGetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedGetter")
            .First();
        OtherPropertyWithProtectedGetter = OtherClassWithProtectedGetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithProtectedGetter")
            .First();
        PropertyWithInternalGetter = ClassWithInternalGetterProperty
            .GetPropertyMembersWithName("PropertyWithInternalGetter")
            .First();
        OtherPropertyWithInternalGetter = OtherClassWithInternalGetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithInternalGetter")
            .First();
        PropertyWithProtectedInternalGetter = ClassWithProtectedInternalGetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedInternalGetter")
            .First();
        OtherPropertyWithProtectedInternalGetter = OtherClassWithProtectedInternalGetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithProtectedInternalGetter")
            .First();
        PropertyWithPrivateProtectedGetter = ClassWithPrivateProtectedGetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateProtectedGetter")
            .First();
        OtherPropertyWithPrivateProtectedGetter = OtherClassWithPrivateProtectedGetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithPrivateProtectedGetter")
            .First();
        PropertyWithPrivateSetter = ClassWithPrivateSetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateSetter")
            .First();
        OtherPropertyWithPrivateSetter = OtherClassWithPrivateSetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithPrivateSetter")
            .First();
        PropertyWithProtectedSetter = ClassWithProtectedSetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedSetter")
            .First();
        OtherPropertyWithProtectedSetter = OtherClassWithProtectedSetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithProtectedSetter")
            .First();
        PropertyWithInternalSetter = ClassWithInternalSetterProperty
            .GetPropertyMembersWithName("PropertyWithInternalSetter")
            .First();
        OtherPropertyWithInternalSetter = OtherClassWithInternalSetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithInternalSetter")
            .First();
        PropertyWithProtectedInternalSetter = ClassWithProtectedInternalSetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedInternalSetter")
            .First();
        OtherPropertyWithProtectedInternalSetter = OtherClassWithProtectedInternalSetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithProtectedInternalSetter")
            .First();
        PropertyWithPrivateProtectedSetter = ClassWithPrivateProtectedSetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateProtectedSetter")
            .First();
        OtherPropertyWithPrivateProtectedSetter = OtherClassWithPrivateProtectedSetterProperty
            .GetPropertyMembersWithName("OtherPropertyWithPrivateProtectedSetter")
            .First();
    }
}
