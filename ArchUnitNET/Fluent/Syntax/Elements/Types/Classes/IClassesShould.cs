using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassesShould : ITypesShould<ClassesShouldConjunction, Class>
    {
        ClassesShouldConjunction BeAbstract();
        ClassesShouldConjunction BeSealed();
        ClassesShouldConjunction BeValueTypes();
        ClassesShouldConjunction BeEnums();
        ClassesShouldConjunction BeStructs();


        //Negations


        ClassesShouldConjunction NotBeAbstract();
        ClassesShouldConjunction NotBeSealed();
        ClassesShouldConjunction NotBeValueTypes();
        ClassesShouldConjunction NotBeEnums();
        ClassesShouldConjunction NotBeStructs();
    }
}