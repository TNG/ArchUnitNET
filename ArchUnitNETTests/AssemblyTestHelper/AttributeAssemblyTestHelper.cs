using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using AttributeNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class AttributeAssemblyTestHelpers : AssemblyTestHelper
{
    public override Architecture Architecture
    {
        get => StaticTestArchitectures.AttributeArchitecture;
    }

    public string NonExistentAttributeValue = "NotTheValueOfAnyAttribute";

    public object Attribute1Parameter1Value = "Argument";
    public object Attribute1Parameter2Value = 0;
    public object Attribute1Parameter3Value = typeof(TypeArgument);
    public object Attribute1Parameter3InvalidValue = typeof(OtherTypeArgument);

    public string Attribute1NamedParameter1Name = "NamedParameter1";
    public object Attribute1NamedParameter1Value = typeof(NamedTypeArgument);
    public (string, object) Attribute1NamedParameter1Pair = (
        "NamedParameter1",
        typeof(NamedTypeArgument)
    );
    public (string, object) Attribute1NamedParameter1InvalidNamePair = (
        "OtherNamedParameter1",
        typeof(NamedTypeArgument)
    );
    public (string, object) Attribute1NamedParameter1InvalidValuePair = (
        "NamedParameter1",
        typeof(OtherNamedTypeArgument)
    );
    public string Attribute1NamedParameter2Name = "NamedParameter2";
    public object Attribute1NamedParameter2Value = "NamedArgument";
    public (string, object) Attribute1NamedParameter2Pair = ("NamedParameter2", "NamedArgument");
    public (string, object) Attribute1NamedParameter2InvalidNamePair = (
        "OtherNamedParameter2",
        "NamedArgument"
    );
    public (string, object) Attribute1NamedParameter2InvalidValuePair = (
        "NamedParameter2",
        "OtherNamedArgument"
    );
    public string Attribute1NamedParameter3Name = "NamedParameter3";
    public object Attribute1NamedParameter3Value = 1;
    public (string, object) Attribute1NamedParameter3Pair = ("NamedParameter3", 1);

    public object Attribute2Parameter1Value = typeof(OtherTypeArgument);
    public object Attribute2Parameter2Value = "OtherArgument";
    public object Attribute2Parameter3Value = 2;

    public string Attribute2NamedParameter1Name = "OtherNamedParameter1";
    public object Attribute2NamedParameter1Value = "OtherNamedArgument";
    public (string, object) Attribute2NamedParameter1Pair = (
        "OtherNamedParameter1",
        "OtherNamedArgument"
    );
    public string Attribute2NamedParameter2Name = "OtherNamedParameter2";
    public object Attribute2NamedParameter2Value = 3;
    public (string, object) Attribute2NamedParameter2Pair = ("OtherNamedParameter2", 3);
    public string Attribute2NamedParameter3Name = "OtherNamedParameter3";
    public object Attribute2NamedParameter3Value = typeof(OtherNamedTypeArgument);
    public (string, object) Attribute2NamedParameter3Pair = (
        "OtherNamedParameter3",
        typeof(OtherNamedTypeArgument)
    );

    public string UnusedParameterName = "UnusedParameter";
    public object UnusedParameterValue = "UnusedValueArgument";
    public object UnusedTypeParameterValue = typeof(UnusedTypeArgument);

    public Attribute Attribute1;
    public System.Type Attribute1SystemType = typeof(Attribute1);

    public Attribute Attribute2;
    public System.Type Attribute2SystemType = typeof(Attribute2);
    
    public Attribute Attribute3;
    public System.Type Attribute3SystemType = typeof(Attribute3);
    
    public Attribute OnceUsedAttribute;
    public System.Type OnceUsedAttributeSystemType = typeof(OnceUsedAttribute);

    public Attribute UnusedAttribute;
    public System.Type UnusedAttributeSystemType = typeof(UnusedAttribute);

    public Class ClassWithoutAttributes;
    public System.Type ClassWithoutAttributesSystemType = typeof(ClassWithoutAttributes);

    public Class ClassWithSingleAttribute;
    public System.Type ClassWithSingleAttributeSystemType = typeof(ClassWithSingleAttribute);

    public Class ClassWithSingleUniquelyUsedAttribute;
    public System.Type ClassWithSingleUniquelyUsedAttributeSystemType =
        typeof(ClassWithSingleUniquelyUsedAttribute);
    
    public Class ClassWithTwoAttributes;
    public System.Type ClassWithTwoAttributesSystemType = typeof(ClassWithTwoAttributes);
    
    public Class ClassWithThreeAttributes;
    public System.Type ClassWithThreeAttributesSystemType = typeof(ClassWithThreeAttributes);

    public Class ClassWithArguments;
    public System.Type ClassWithArgumentsSystemType = typeof(ClassWithArguments);

    public Class OtherClassWithArguments;
    public System.Type OtherClassWithArgumentsSystemType = typeof(OtherClassWithArguments);

    public AttributeAssemblyTestHelpers()
    {
        Attribute1 = Architecture.GetAttributeOfType(typeof(Attribute1));
        Attribute2 = Architecture.GetAttributeOfType(typeof(Attribute2));
        Attribute3 = Architecture.GetAttributeOfType(typeof(Attribute3));
        OnceUsedAttribute = Architecture.GetAttributeOfType(typeof(OnceUsedAttribute));
        UnusedAttribute = Architecture.GetAttributeOfType(typeof(UnusedAttribute));
        ClassWithoutAttributes = Architecture.GetClassOfType(typeof(ClassWithoutAttributes));
        ClassWithSingleAttribute = Architecture.GetClassOfType(typeof(ClassWithSingleAttribute));
        ClassWithSingleUniquelyUsedAttribute = Architecture.GetClassOfType(
            typeof(ClassWithSingleUniquelyUsedAttribute)
        );
        ClassWithTwoAttributes = Architecture.GetClassOfType(typeof(ClassWithTwoAttributes));
        ClassWithThreeAttributes = Architecture.GetClassOfType(typeof(ClassWithThreeAttributes));
        ClassWithArguments = Architecture.GetClassOfType(typeof(ClassWithArguments));
        OtherClassWithArguments = Architecture.GetClassOfType(typeof(OtherClassWithArguments));
    }
}
