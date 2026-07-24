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

public class ClassWithGetOnlyProperty
{
    public string GetOnlyProperty { get; } = "";
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

public class ClassWithPrivateGetterProperty
{
    public string PropertyWithPrivateGetter { private get; set; } = "";
}

public class ClassWithProtectedGetterProperty
{
    public string PropertyWithProtectedGetter { protected get; set; } = "";
}

public class ClassWithInternalGetterProperty
{
    public string PropertyWithInternalGetter { internal get; set; } = "";
}

public class ClassWithProtectedInternalGetterProperty
{
    public string PropertyWithProtectedInternalGetter { protected internal get; set; } = "";
}

public class ClassWithPrivateProtectedGetterProperty
{
    public string PropertyWithPrivateProtectedGetter { private protected get; set; } = "";
}

public class ClassWithPrivateSetterProperty
{
    public string PropertyWithPrivateSetter { get; private set; } = "";
}

public class ClassWithProtectedSetterProperty
{
    public string PropertyWithProtectedSetter { get; protected set; } = "";
}

public class ClassWithInternalSetterProperty
{
    public string PropertyWithInternalSetter { get; internal set; } = "";
}

public class ClassWithProtectedInternalSetterProperty
{
    public string PropertyWithProtectedInternalSetter { get; protected internal set; } = "";
}

public class ClassWithPrivateProtectedSetterProperty
{
    public string PropertyWithPrivateProtectedSetter { get; private protected set; } = "";
}
