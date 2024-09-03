namespace AttributeNamespace;

public class TypeArgument { };

public class OtherTypeArgument { };

public class NamedTypeArgument { }

public class OtherNamedTypeArgument { }

public class UnusedTypeArgument { }

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute1 : System.Attribute
{
    public Attribute1(string parameter1 = "", int parameter2 = -1, System.Type parameter3 = null)
    { }

    public System.Type NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; }

    public int NamedParameter3 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute2 : System.Attribute
{
    public Attribute2(System.Type parameter1 = null, string parameter2 = "", int parameter3 = -1)
    { }

    public string OtherNamedParameter1 { get; set; }

    public int OtherNamedParameter2 { get; set; }

    public System.Type OtherNamedParameter3 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class UnusedAttribute : System.Attribute
{
    public UnusedAttribute(
        int unusedParameter1 = -1,
        System.Type unusedParameter2 = null,
        string unusedParameter3 = ""
    ) { }

    public int UnusedNamedParameter1 { get; set; }

    public System.Type UnusedNamedParameter2 { get; set; }

    public string UnusedNamedParameter3 { get; set; }
}

public class ClassWithoutAttributes { }

public class OtherClassWithoutAttributes { }

[Attribute1]
public class ClassWithSingleAttribute { }

[Attribute1]
public class OtherClassWithSingleAttribute { }

[Attribute1]
[Attribute2]
public class ClassWithAttributes { }

[Attribute1]
[Attribute2]
public class OtherClassWithAttributes { }

[Attribute1(
    "Argument",
    0,
    typeof(TypeArgument),
    NamedParameter1 = typeof(NamedTypeArgument),
    NamedParameter2 = "NamedArgument",
    NamedParameter3 = 1
)]
[Attribute2(
    typeof(OtherTypeArgument),
    "OtherArgument",
    2,
    OtherNamedParameter1 = "OtherNamedArgument",
    OtherNamedParameter2 = 3,
    OtherNamedParameter3 = typeof(OtherNamedTypeArgument)
)]
public class ClassWithArguments { }

[Attribute1(
    "Argument",
    0,
    typeof(TypeArgument),
    NamedParameter1 = typeof(NamedTypeArgument),
    NamedParameter2 = "NamedArgument",
    NamedParameter3 = 1
)]
[Attribute2(
    typeof(OtherTypeArgument),
    "OtherArgument",
    2,
    OtherNamedParameter1 = "OtherNamedArgument",
    OtherNamedParameter2 = 3,
    OtherNamedParameter3 = typeof(OtherNamedTypeArgument)
)]
public class OtherClassWithArguments { }
