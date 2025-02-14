using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IComplexClassConditions
        : IComplexTypeConditions<ClassesShouldConjunction, Class>,
            IClassConditions<ClassesShouldConjunction, Class> { }
}
