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

    private readonly Class ClassWithWritableProperty;
    private readonly Class OtherClassWithWritableProperty;
    private readonly Class ClassWithInitOnlyProperty;
    private readonly Class OtherClassWithInitOnlyProperty;
    private readonly Class ClassWithGetOnlyProperty;
    private readonly Class OtherClassWithGetOnlyProperty;
    private readonly Class ClassWithVirtualProperty;
    private readonly Class OtherClassWithVirtualProperty;
    private readonly Class ClassWithNonVirtualProperty;
    private readonly Class ClassWithWriteOnlyProperty;
    private readonly Class OtherClassWithWriteOnlyProperty;
    private readonly Class ClassWithPrivateGetterProperty;
    private readonly Class OtherClassWithPrivateGetterProperty;
    private readonly Class ClassWithProtectedGetterProperty;
    private readonly Class OtherClassWithProtectedGetterProperty;
    private readonly Class ClassWithInternalGetterProperty;
    private readonly Class OtherClassWithInternalGetterProperty;
    private readonly Class ClassWithProtectedInternalGetterProperty;
    private readonly Class OtherClassWithProtectedInternalGetterProperty;
    private readonly Class ClassWithPrivateProtectedGetterProperty;
    private readonly Class OtherClassWithPrivateProtectedGetterProperty;
    private readonly Class ClassWithPrivateSetterProperty;
    private readonly Class OtherClassWithPrivateSetterProperty;
    private readonly Class ClassWithProtectedSetterProperty;
    private readonly Class OtherClassWithProtectedSetterProperty;
    private readonly Class ClassWithInternalSetterProperty;
    private readonly Class OtherClassWithInternalSetterProperty;
    private readonly Class ClassWithProtectedInternalSetterProperty;
    private readonly Class OtherClassWithProtectedInternalSetterProperty;
    private readonly Class ClassWithPrivateProtectedSetterProperty;
    private readonly Class OtherClassWithPrivateProtectedSetterProperty;

    // Individual members
    public readonly PropertyMember WritableProperty;
    public readonly PropertyMember OtherWritableProperty;
    public readonly PropertyMember InitOnlyProperty;
    public readonly PropertyMember OtherInitOnlyProperty;
    public readonly PropertyMember GetOnlyProperty;
    public readonly PropertyMember OtherGetOnlyProperty;

    // Property members (PropertyMember type)
    public readonly PropertyMember VirtualProperty;
    public readonly PropertyMember OtherVirtualProperty;
    public readonly PropertyMember NonVirtualProperty;
    public readonly PropertyMember WriteOnlyProperty;
    public readonly PropertyMember OtherWriteOnlyProperty;
    public readonly PropertyMember PropertyWithPrivateGetter;
    public readonly PropertyMember OtherPropertyWithPrivateGetter;
    public readonly PropertyMember PropertyWithProtectedGetter;
    public readonly PropertyMember OtherPropertyWithProtectedGetter;
    public readonly PropertyMember PropertyWithInternalGetter;
    public readonly PropertyMember OtherPropertyWithInternalGetter;
    public readonly PropertyMember PropertyWithProtectedInternalGetter;
    public readonly PropertyMember OtherPropertyWithProtectedInternalGetter;
    public readonly PropertyMember PropertyWithPrivateProtectedGetter;
    public readonly PropertyMember OtherPropertyWithPrivateProtectedGetter;
    public readonly PropertyMember PropertyWithPrivateSetter;
    public readonly PropertyMember OtherPropertyWithPrivateSetter;
    public readonly PropertyMember PropertyWithProtectedSetter;
    public readonly PropertyMember OtherPropertyWithProtectedSetter;
    public readonly PropertyMember PropertyWithInternalSetter;
    public readonly PropertyMember OtherPropertyWithInternalSetter;
    public readonly PropertyMember PropertyWithProtectedInternalSetter;
    public readonly PropertyMember OtherPropertyWithProtectedInternalSetter;
    public readonly PropertyMember PropertyWithPrivateProtectedSetter;
    public readonly PropertyMember OtherPropertyWithPrivateProtectedSetter;

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
