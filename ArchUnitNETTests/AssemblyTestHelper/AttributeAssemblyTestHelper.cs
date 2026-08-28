using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using AttributeNamespace;

namespace ArchUnitNETTests.AssemblyTestHelper;

public class AttributeAssemblyTestHelper : AssemblyTestHelper
{
    public sealed override Architecture Architecture =>
        StaticTestArchitectures.AttributeArchitecture;

    public readonly Attribute Attribute1;
    public readonly System.Type Attribute1SystemType = typeof(Attribute1);

    public readonly Attribute Attribute2;
    public readonly System.Type Attribute2SystemType = typeof(Attribute2);

    private readonly Attribute Attribute3;

    private readonly Attribute OnceUsedAttribute;

    public readonly Attribute UnusedAttribute;
    public readonly System.Type UnusedAttributeSystemType = typeof(UnusedAttribute);

    public readonly Class ClassWithoutAttributes;

    public readonly Class ClassWithSingleAttribute;

    private readonly Class ClassWithSingleUniquelyUsedAttribute;

    public readonly Class ClassWithTwoAttributes;

    public readonly Class ClassWithThreeAttributes;

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
    public readonly object Attribute2TypeArgumentSystemType = typeof(TypeArgument2);

    public readonly Class ClassWithSingleAttributeWithArguments;

    public readonly Class ClassWithTwoAttributesWithArguments;

    private readonly Class ClassWithThreeAttributesWithArguments;

    public readonly Class ClassWithSingleAttributeWithNamedArguments;

    public readonly Class ClassWithTwoAttributesWithNamedArguments;

    private readonly Class ClassWithThreeAttributesWithNamedArguments;

    public readonly Attribute RegularAttribute;

    public readonly Attribute OtherRegularAttribute;

    public readonly Attribute AbstractAttribute;

    public readonly Attribute OtherAbstractAttribute;

    public readonly Attribute SealedAttribute;

    public readonly Attribute OtherSealedAttribute;

    public AttributeAssemblyTestHelper()
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
        RegularAttribute = Architecture.GetAttributeOfType(typeof(RegularAttribute));
        OtherRegularAttribute = Architecture.GetAttributeOfType(typeof(OtherRegularAttribute));
        AbstractAttribute = Architecture.GetAttributeOfType(typeof(AbstractAttribute));
        OtherAbstractAttribute = Architecture.GetAttributeOfType(typeof(OtherAbstractAttribute));
        SealedAttribute = Architecture.GetAttributeOfType(typeof(SealedAttribute));
        OtherSealedAttribute = Architecture.GetAttributeOfType(typeof(OtherSealedAttribute));
    }
}
