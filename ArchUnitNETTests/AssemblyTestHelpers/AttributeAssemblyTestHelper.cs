using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using AttributeNamespace;

namespace ArchUnitNETTests;

public class AttributeAssemblyTestHelpers : AssemblyTestHelper
{
    public override Architecture Architecture
    {
        get => StaticTestArchitectures.AttributeArchitecture;
    }

    public string NonExistentAttributeValue = "NotTheValueOfAnyAttribute";

    public object Attribute1Argument1Value = "Attribute1Argument1";
    public object Attribute1Argument2Value = "Attribute1Argument2";
    public object Attribute1NamedArgument1Value = "Attribute1NamedArgument1";
    public object Attribute1NamedArgument2Value = "Attribute1NamedArgument2";
    public object Attribute2Argument1Value = "Attribute2Argument1";
    public object Attribute2Argument2Value = "Attribute2Argument2";
    public object Attribute2NamedArgument1Value = "Attribute2NamedArgument1";
    public object Attribute2NamedArgument2Value = "Attribute2NamedArgument2";

    public Attribute Attribute1;
    public System.Type Attribute1SystemType = typeof(Attribute1);

    public Attribute Attribute2;
    public System.Type Attribute2SystemType = typeof(Attribute2);

    public Attribute UnusedAttribute;
    public System.Type UnusedAttributeSystemType = typeof(UnusedAttribute);

    public Class ClassWithoutAttributes;
    public System.Type ClassWithoutAttributesSystemType = typeof(ClassWithoutAttributes);

    public Class OtherClassWithoutAttributes;
    public System.Type OtherClassWithoutAttributesSystemType = typeof(OtherClassWithoutAttributes);

    public Class ClassWithSingleAttribute;
    public System.Type ClassWithSingleAttributeSystemType = typeof(ClassWithSingleAttribute);

    public Class OtherClassWithSingleAttribute;
    public System.Type OtherClassWithSingleAttributeSystemType =
        typeof(OtherClassWithSingleAttribute);

    public Class ClassWithAttributes;
    public System.Type ClassWithAttributesSystemType = typeof(ClassWithAttributes);

    public Class OtherClassWithAttributes;
    public System.Type OtherClassWithAttributesSystemType = typeof(OtherClassWithAttributes);

    public Class ClassWithPositionalArguments;
    public System.Type ClassWithPositionalArgumentsSystemType =
        typeof(ClassWithPositionalArguments);

    public Class OtherClassWithPositionalArguments;
    public System.Type OtherClassWithPositionalArgumentsSystemType =
        typeof(OtherClassWithPositionalArguments);

    public Class ClassWithNamedArguments;
    public System.Type ClassWithNamedArgumentsSystemType = typeof(ClassWithNamedArguments);

    public Class OtherClassWithNamedArguments;
    public System.Type OtherClassWithNamedArgumentsSystemType =
        typeof(OtherClassWithNamedArguments);

    public AttributeAssemblyTestHelpers()
    {
        Attribute1 = Architecture.GetAttributeOfType(typeof(Attribute1));
        Attribute2 = Architecture.GetAttributeOfType(typeof(Attribute2));
        UnusedAttribute = Architecture.GetAttributeOfType(typeof(UnusedAttribute));
        ClassWithoutAttributes = Architecture.GetClassOfType(typeof(ClassWithoutAttributes));
        OtherClassWithoutAttributes = Architecture.GetClassOfType(
            typeof(OtherClassWithoutAttributes)
        );
        ClassWithSingleAttribute = Architecture.GetClassOfType(typeof(ClassWithSingleAttribute));
        OtherClassWithSingleAttribute = Architecture.GetClassOfType(
            typeof(OtherClassWithSingleAttribute)
        );
        ClassWithAttributes = Architecture.GetClassOfType(typeof(ClassWithAttributes));
        OtherClassWithAttributes = Architecture.GetClassOfType(typeof(OtherClassWithAttributes));
        ClassWithPositionalArguments = Architecture.GetClassOfType(
            typeof(ClassWithPositionalArguments)
        );
        OtherClassWithPositionalArguments = Architecture.GetClassOfType(
            typeof(OtherClassWithPositionalArguments)
        );
        ClassWithNamedArguments = Architecture.GetClassOfType(typeof(ClassWithNamedArguments));
        OtherClassWithNamedArguments = Architecture.GetClassOfType(
            typeof(OtherClassWithNamedArguments)
        );
    }
}
