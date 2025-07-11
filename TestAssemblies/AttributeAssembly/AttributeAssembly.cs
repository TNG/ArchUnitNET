using System.Diagnostics.CodeAnalysis;

namespace AttributeNamespace;

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute1 : System.Attribute
{
    public Attribute1(string parameter1 = "", int parameter2 = -1, System.Type? parameter3 = null)
    { }

    public System.Type? NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; } = "";

    public int NamedParameter3 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute2 : System.Attribute
{
    public Attribute2(System.Type? parameter1 = null, string parameter2 = "", int parameter3 = -1)
    { }

    public string NamedParameter1 { get; set; } = "";

    public int NamedParameter2 { get; set; }

    public System.Type? NamedParameter3 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute3 : System.Attribute
{
    public Attribute3(int parameter3 = -1, System.Type? parameter1 = null, string parameter2 = "")
    { }

    public int NamedParameter1 { get; set; }

    public System.Type? NamedParameter2 { get; set; }

    public string NamedParameter3 { get; set; } = "";
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class UnusedAttribute : System.Attribute
{
    public UnusedAttribute(
        int unusedParameter1 = -1,
        System.Type? unusedParameter2 = null,
        string unusedParameter3 = ""
    ) { }

    public int UnusedNamedParameter1 { get; set; }

    public System.Type? UnusedNamedParameter2 { get; set; }

    public string UnusedNamedParameter3 { get; set; } = "";
}

public class ClassWithoutAttributes { }

[Attribute1]
public class ClassWithSingleAttribute { }

[Attribute1]
[Attribute2]
public class ClassWithTwoAttributes { }

[Attribute1]
[Attribute2]
[Attribute3]
public class ClassWithThreeAttributes { }

public class TypeArgument1 { };

public class TypeArgument2 { };

public class TypeArgument3 { };

public class UnusedTypeArgument { };

[Attribute1("Argument1", 1, typeof(TypeArgument1))]
public class ClassWithSingleAttributeWithArguments { }

[Attribute1("Argument1", 1, typeof(TypeArgument1))]
[Attribute2(typeof(TypeArgument2), "Argument2", 2)]
public class ClassWithTwoAttributesWithArguments { }

[Attribute1("Argument1", 1, typeof(TypeArgument1))]
[Attribute2(typeof(TypeArgument2), "Argument2", 2)]
[Attribute3(3, typeof(TypeArgument3), "Argument3")]
public class ClassWithThreeAttributesWithArguments { }

[Attribute1(
    NamedParameter1 = typeof(TypeArgument1),
    NamedParameter2 = "Argument1",
    NamedParameter3 = 1
)]
public class ClassWithSingleAttributeWithNamedArguments { }

[Attribute1(
    NamedParameter1 = typeof(TypeArgument1),
    NamedParameter2 = "Argument1",
    NamedParameter3 = 1
)]
[Attribute2(
    NamedParameter1 = "Argument2",
    NamedParameter2 = 2,
    NamedParameter3 = typeof(TypeArgument2)
)]
public class ClassWithTwoAttributesWithNamedArguments { }

[Attribute1(
    NamedParameter1 = typeof(TypeArgument1),
    NamedParameter2 = "Argument1",
    NamedParameter3 = 1
)]
[Attribute2(
    NamedParameter1 = "Argument2",
    NamedParameter2 = 2,
    NamedParameter3 = typeof(TypeArgument2)
)]
[Attribute3(
    NamedParameter1 = 3,
    NamedParameter2 = typeof(TypeArgument3),
    NamedParameter3 = "Argument3"
)]
public class ClassWithThreeAttributesWithNamedArguments { }

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class OnceUsedAttribute : System.Attribute
{
    public OnceUsedAttribute(
        string parameter1 = "",
        int parameter2 = -1,
        System.Type? parameter3 = null
    ) { }

    public System.Type? NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; } = "";

    public int NamedParameter3 { get; set; }
}

[OnceUsed]
public class ClassWithSingleUniquelyUsedAttribute { }
