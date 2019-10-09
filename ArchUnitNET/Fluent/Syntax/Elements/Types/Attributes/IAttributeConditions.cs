using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributeConditions : ITypeConditions<AttributesShouldConjunction, Attribute>
    {
        AttributesShouldConjunction BeAbstract();
        AttributesShouldConjunction BeSealed();


        //Negations


        AttributesShouldConjunction NotBeAbstract();
        AttributesShouldConjunction NotBeSealed();
    }
}