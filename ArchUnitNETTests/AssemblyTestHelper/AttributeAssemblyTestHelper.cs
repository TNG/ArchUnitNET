using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using AttributeNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class AttributeAssemblyTestHelpers : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.AttributeArchitecture;

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

    public readonly string UnusedAttributeStringValue = "NotTheValueOfAnyAttribute";
    public readonly int UnusedAttributeIntValue = 42;
    public readonly Class UnusedTypeArgument;
    public readonly System.Type UnusedTypeArgumentSystemType = typeof(UnusedTypeArgument);

    public readonly object Attribute1StringArgument = "Argument1";
    public readonly object Attribute1IntegerArgument = 1;
    public readonly object Attribute1TypeArgument;
    public readonly object Attribute1TypeArgumentSystemType = typeof(TypeArgument1);

    public readonly object Attribute2StringArgument = "Argument2";
    public readonly object Attribute2IntegerArgument = 2;
    public readonly object Attribute2TypeArgument;
    public readonly object Attribute2TypeArgumentSystemType = typeof(TypeArgument2);

    public readonly object Attribute3StringArgument = "Argument3";
    public readonly object Attribute3IntegerArgument = 3;
    public readonly object Attribute3TypeArgument;
    public readonly object Attribute3TypeArgumentSystemType = typeof(TypeArgument3);

    public Class ClassWithSingleAttributeWithArguments;
    public System.Type ClassWithSingleAttributeWithArgumentsSystemType =
        typeof(ClassWithSingleAttributeWithArguments);

    public Class ClassWithTwoAttributesWithArguments;
    public System.Type ClassWithTwoAttributesWithArgumentsSystemType =
        typeof(ClassWithTwoAttributesWithArguments);

    public Class ClassWithThreeAttributesWithArguments;
    public System.Type ClassWithThreeAttributesWithArgumentsSystemType =
        typeof(ClassWithThreeAttributesWithArguments);

    public Class ClassWithSingleAttributeWithNamedArguments;
    public System.Type ClassWithSingleAttributeWithNamedArgumentsSystemType =
        typeof(ClassWithSingleAttributeWithNamedArguments);

    public Class ClassWithTwoAttributesWithNamedArguments;
    public System.Type ClassWithTwoAttributesWithNamedArgumentsSystemType =
        typeof(ClassWithTwoAttributesWithNamedArguments);

    public Class ClassWithThreeAttributesWithNamedArguments;
    public System.Type ClassWithThreeAttributesWithNamedArgumentsSystemType =
        typeof(ClassWithThreeAttributesWithNamedArguments);

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
        UnusedTypeArgument = Architecture.GetClassOfType(typeof(UnusedTypeArgument));
        Attribute1TypeArgument = Architecture.GetClassOfType(typeof(TypeArgument1));
        Attribute2TypeArgument = Architecture.GetClassOfType(typeof(TypeArgument2));
        Attribute3TypeArgument = Architecture.GetClassOfType(typeof(TypeArgument3));
        ClassWithSingleAttributeWithArguments = Architecture.GetClassOfType(
            typeof(ClassWithSingleAttributeWithArguments)
        );
        ClassWithTwoAttributesWithArguments = Architecture.GetClassOfType(
            typeof(ClassWithTwoAttributesWithArguments)
        );
        ClassWithThreeAttributesWithArguments = Architecture.GetClassOfType(
            typeof(ClassWithThreeAttributesWithArguments)
        );
        ClassWithSingleAttributeWithNamedArguments = Architecture.GetClassOfType(
            typeof(ClassWithSingleAttributeWithNamedArguments)
        );
        ClassWithTwoAttributesWithNamedArguments = Architecture.GetClassOfType(
            typeof(ClassWithTwoAttributesWithNamedArguments)
        );
        ClassWithThreeAttributesWithNamedArguments = Architecture.GetClassOfType(
            typeof(ClassWithThreeAttributesWithNamedArguments)
        );
    }
}
