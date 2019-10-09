using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IComplexAttributeConditions : IComplexTypeConditions<AttributesShouldConjunction, Attribute>,
        IAttributeConditions<AttributesShouldConjunction>
    {
    }
}