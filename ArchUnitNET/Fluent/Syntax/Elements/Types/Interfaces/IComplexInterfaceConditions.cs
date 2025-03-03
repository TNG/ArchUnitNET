using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public interface IComplexInterfaceConditions
        : IComplexTypeConditions<InterfacesShouldConjunction, Interface>,
            IInterfaceConditions<InterfacesShouldConjunction, Interface> { }
}
