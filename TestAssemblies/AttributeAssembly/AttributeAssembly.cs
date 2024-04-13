namespace AttributeNamespace;

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute1 : System.Attribute
{
    public Attribute1(string parameter1 = "", string parameter2 = "") { }

    public string NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class Attribute2 : System.Attribute
{
    public Attribute2(string parameter1 = "", string parameter2 = "") { }

    public string NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class UnusedAttribute : System.Attribute
{
    public UnusedAttribute(string parameter1 = "", string parameter2 = "") { }

    public string NamedParameter1 { get; set; }

    public string NamedParameter2 { get; set; }
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

[Attribute1("Attribute1Argument1", "Attribute1Argument2")]
[Attribute2("Attribute1Argument1", "Attribute1Argument2")]
public class ClassWithPositionalArguments { }

[Attribute1("Attribute1Argument1", "Attribute1Argument2")]
[Attribute2("Attribute1Argument1", "Attribute1Argument2")]
public class OtherClassWithPositionalArguments { }

[Attribute1(
    NamedParameter1 = "Attribute1NamedArgument1",
    NamedParameter2 = "Attribute1NamedArgument2"
)]
[Attribute2(
    NamedParameter1 = "Attribute1NamedArgument1",
    NamedParameter2 = "Attribute1NamedArgument2"
)]
public class ClassWithNamedArguments { }

[Attribute1(
    NamedParameter1 = "Attribute1NamedArgument1",
    NamedParameter2 = "Attribute1NamedArgument2"
)]
[Attribute2(
    NamedParameter1 = "Attribute1NamedArgument1",
    NamedParameter2 = "Attribute1NamedArgument2"
)]
public class OtherClassWithNamedArguments { }
