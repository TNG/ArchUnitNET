using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributesShould : ITypesShould<AttributesShouldConjunction, Attribute>
    {
        AttributesShouldConjunction BeAbstract();
        AttributesShouldConjunction BeSealed();


        //Negations


        AttributesShouldConjunction NotBeAbstract();
        AttributesShouldConjunction NotBeSealed();
    }
}