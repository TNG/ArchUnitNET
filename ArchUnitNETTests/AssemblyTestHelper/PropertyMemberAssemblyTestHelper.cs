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

    public Class ClassWithWritableProperty;
    public Type ClassWithWritablePropertySystemType = typeof(ClassWithWritableProperty);

    public Class OtherClassWithWritableProperty;
    public Type OtherClassWithWritablePropertySystemType = typeof(OtherClassWithWritableProperty);

    public Class ClassWithInitOnlyProperty;
    public Type ClassWithInitOnlyPropertySystemType = typeof(ClassWithInitOnlyProperty);

    public Class ClassWithGetOnlyProperty;
    public Type ClassWithGetOnlyPropertySystemType = typeof(ClassWithGetOnlyProperty);

    public Class ClassWithVirtualProperty;
    public Type ClassWithVirtualPropertySystemType = typeof(ClassWithVirtualProperty);

    public Class OtherClassWithVirtualProperty;
    public Type OtherClassWithVirtualPropertySystemType = typeof(OtherClassWithVirtualProperty);

    public Class ClassWithNonVirtualProperty;
    public Type ClassWithNonVirtualPropertySystemType = typeof(ClassWithNonVirtualProperty);

    public Class ClassWithWriteOnlyProperty;
    public Type ClassWithWriteOnlyPropertySystemType = typeof(ClassWithWriteOnlyProperty);

    public Class ClassWithPrivateGetterProperty;
    public Type ClassWithPrivateGetterPropertySystemType = typeof(ClassWithPrivateGetterProperty);

    public Class ClassWithProtectedGetterProperty;
    public Type ClassWithProtectedGetterPropertySystemType =
        typeof(ClassWithProtectedGetterProperty);

    public Class ClassWithInternalGetterProperty;
    public Type ClassWithInternalGetterPropertySystemType = typeof(ClassWithInternalGetterProperty);

    public Class ClassWithProtectedInternalGetterProperty;
    public Type ClassWithProtectedInternalGetterPropertySystemType =
        typeof(ClassWithProtectedInternalGetterProperty);

    public Class ClassWithPrivateProtectedGetterProperty;
    public Type ClassWithPrivateProtectedGetterPropertySystemType =
        typeof(ClassWithPrivateProtectedGetterProperty);

    public Class ClassWithPrivateSetterProperty;
    public Type ClassWithPrivateSetterPropertySystemType = typeof(ClassWithPrivateSetterProperty);

    public Class ClassWithProtectedSetterProperty;
    public Type ClassWithProtectedSetterPropertySystemType =
        typeof(ClassWithProtectedSetterProperty);

    public Class ClassWithInternalSetterProperty;
    public Type ClassWithInternalSetterPropertySystemType = typeof(ClassWithInternalSetterProperty);

    public Class ClassWithProtectedInternalSetterProperty;
    public Type ClassWithProtectedInternalSetterPropertySystemType =
        typeof(ClassWithProtectedInternalSetterProperty);

    public Class ClassWithPrivateProtectedSetterProperty;
    public Type ClassWithPrivateProtectedSetterPropertySystemType =
        typeof(ClassWithPrivateProtectedSetterProperty);

    // Individual members
    public IMember WritableProperty;
    public IMember OtherWritableProperty;
    public IMember InitOnlyProperty;
    public IMember GetOnlyProperty;

    // Property members (PropertyMember type)
    public PropertyMember VirtualProperty;
    public PropertyMember OtherVirtualProperty;
    public PropertyMember NonVirtualProperty;
    public PropertyMember WriteOnlyProperty;
    public PropertyMember PropertyWithPrivateGetter;
    public PropertyMember PropertyWithProtectedGetter;
    public PropertyMember PropertyWithInternalGetter;
    public PropertyMember PropertyWithProtectedInternalGetter;
    public PropertyMember PropertyWithPrivateProtectedGetter;
    public PropertyMember PropertyWithPrivateSetter;
    public PropertyMember PropertyWithProtectedSetter;
    public PropertyMember PropertyWithInternalSetter;
    public PropertyMember PropertyWithProtectedInternalSetter;
    public PropertyMember PropertyWithPrivateProtectedSetter;

    public PropertyMemberAssemblyTestHelper()
    {
        ClassWithWritableProperty = Architecture.GetClassOfType(typeof(ClassWithWritableProperty));
        OtherClassWithWritableProperty = Architecture.GetClassOfType(
            typeof(OtherClassWithWritableProperty)
        );
        ClassWithInitOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithInitOnlyProperty));
        ClassWithGetOnlyProperty = Architecture.GetClassOfType(typeof(ClassWithGetOnlyProperty));

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
        ClassWithPrivateGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateGetterProperty)
        );
        ClassWithProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedGetterProperty)
        );
        ClassWithInternalGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithInternalGetterProperty)
        );
        ClassWithProtectedInternalGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedInternalGetterProperty)
        );
        ClassWithPrivateProtectedGetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateProtectedGetterProperty)
        );
        ClassWithPrivateSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateSetterProperty)
        );
        ClassWithProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedSetterProperty)
        );
        ClassWithInternalSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithInternalSetterProperty)
        );
        ClassWithProtectedInternalSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithProtectedInternalSetterProperty)
        );
        ClassWithPrivateProtectedSetterProperty = Architecture.GetClassOfType(
            typeof(ClassWithPrivateProtectedSetterProperty)
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
        GetOnlyProperty = ClassWithGetOnlyProperty
            .GetPropertyMembersWithName("GetOnlyProperty")
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
        PropertyWithPrivateGetter = ClassWithPrivateGetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateGetter")
            .First();
        PropertyWithProtectedGetter = ClassWithProtectedGetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedGetter")
            .First();
        PropertyWithInternalGetter = ClassWithInternalGetterProperty
            .GetPropertyMembersWithName("PropertyWithInternalGetter")
            .First();
        PropertyWithProtectedInternalGetter = ClassWithProtectedInternalGetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedInternalGetter")
            .First();
        PropertyWithPrivateProtectedGetter = ClassWithPrivateProtectedGetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateProtectedGetter")
            .First();
        PropertyWithPrivateSetter = ClassWithPrivateSetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateSetter")
            .First();
        PropertyWithProtectedSetter = ClassWithProtectedSetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedSetter")
            .First();
        PropertyWithInternalSetter = ClassWithInternalSetterProperty
            .GetPropertyMembersWithName("PropertyWithInternalSetter")
            .First();
        PropertyWithProtectedInternalSetter = ClassWithProtectedInternalSetterProperty
            .GetPropertyMembersWithName("PropertyWithProtectedInternalSetter")
            .First();
        PropertyWithPrivateProtectedSetter = ClassWithPrivateProtectedSetterProperty
            .GetPropertyMembersWithName("PropertyWithPrivateProtectedSetter")
            .First();
    }
}
