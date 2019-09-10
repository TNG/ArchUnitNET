using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassesShould : ITypesShould<ClassesShouldConjunction, Class>
    {
        ClassesShouldConjunction BeAbstract();


        //Negations


        ClassesShouldConjunction NotBeAbstract();
    }
}