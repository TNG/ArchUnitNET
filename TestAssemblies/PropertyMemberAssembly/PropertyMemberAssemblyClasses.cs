namespace PropertyMemberNamespace;

public class ClassWithWritableProperty
{
    public string WritableProperty { get; set; } = "";
}

public class OtherClassWithWritableProperty
{
    public string OtherWritableProperty { get; set; } = "";
}

public class ClassWithInitOnlyProperty
{
    public string InitOnlyProperty { get; init; } = "";
}

public class OtherClassWithInitOnlyProperty
{
    public string OtherInitOnlyProperty { get; init; } = "";
}

public class ClassWithGetOnlyProperty
{
    public string GetOnlyProperty { get; } = "";
}

public class OtherClassWithGetOnlyProperty
{
    public string OtherGetOnlyProperty { get; } = "";
}

public class ClassWithVirtualProperty
{
    public virtual string VirtualProperty { get; set; } = "";
}

public class OtherClassWithVirtualProperty
{
    public virtual string OtherVirtualProperty { get; set; } = "";
}

public class ClassWithNonVirtualProperty
{
    public string NonVirtualProperty { get; set; } = "";
}

public class ClassWithWriteOnlyProperty
{
    private string _writeOnlyField = "";

    public string WriteOnlyProperty
    {
        set => _writeOnlyField = value;
    }
}

public class OtherClassWithWriteOnlyProperty
{
    private string _otherWriteOnlyField = "";

    public string OtherWriteOnlyProperty
    {
        set => _otherWriteOnlyField = value;
    }
}

public class ClassWithPrivateGetterProperty
{
    public string PropertyWithPrivateGetter { private get; set; } = "";
}

public class OtherClassWithPrivateGetterProperty
{
    public string OtherPropertyWithPrivateGetter { private get; set; } = "";
}

public class ClassWithProtectedGetterProperty
{
    public string PropertyWithProtectedGetter { protected get; set; } = "";
}

public class OtherClassWithProtectedGetterProperty
{
    public string OtherPropertyWithProtectedGetter { protected get; set; } = "";
}

public class ClassWithInternalGetterProperty
{
    public string PropertyWithInternalGetter { internal get; set; } = "";
}

public class OtherClassWithInternalGetterProperty
{
    public string OtherPropertyWithInternalGetter { internal get; set; } = "";
}

public class ClassWithProtectedInternalGetterProperty
{
    public string PropertyWithProtectedInternalGetter { protected internal get; set; } = "";
}

public class OtherClassWithProtectedInternalGetterProperty
{
    public string OtherPropertyWithProtectedInternalGetter { protected internal get; set; } = "";
}

public class ClassWithPrivateProtectedGetterProperty
{
    public string PropertyWithPrivateProtectedGetter { private protected get; set; } = "";
}

public class OtherClassWithPrivateProtectedGetterProperty
{
    public string OtherPropertyWithPrivateProtectedGetter { private protected get; set; } = "";
}

public class ClassWithPrivateSetterProperty
{
    public string PropertyWithPrivateSetter { get; private set; } = "";
}

public class OtherClassWithPrivateSetterProperty
{
    public string OtherPropertyWithPrivateSetter { get; private set; } = "";
}

public class ClassWithProtectedSetterProperty
{
    public string PropertyWithProtectedSetter { get; protected set; } = "";
}

public class OtherClassWithProtectedSetterProperty
{
    public string OtherPropertyWithProtectedSetter { get; protected set; } = "";
}

public class ClassWithInternalSetterProperty
{
    public string PropertyWithInternalSetter { get; internal set; } = "";
}

public class OtherClassWithInternalSetterProperty
{
    public string OtherPropertyWithInternalSetter { get; internal set; } = "";
}

public class ClassWithProtectedInternalSetterProperty
{
    public string PropertyWithProtectedInternalSetter { get; protected internal set; } = "";
}

public class OtherClassWithProtectedInternalSetterProperty
{
    public string OtherPropertyWithProtectedInternalSetter { get; protected internal set; } = "";
}

public class ClassWithPrivateProtectedSetterProperty
{
    public string PropertyWithPrivateProtectedSetter { get; private protected set; } = "";
}

public class OtherClassWithPrivateProtectedSetterProperty
{
    public string OtherPropertyWithPrivateProtectedSetter { get; private protected set; } = "";
}
