using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShould : TypesShould<InterfacesShouldConjunction, Interface>, IInterfaceConditions
    {
        public InterfacesShould(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}