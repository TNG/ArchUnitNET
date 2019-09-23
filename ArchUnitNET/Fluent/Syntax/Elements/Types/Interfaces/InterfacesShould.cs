using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShould : TypesShould<InterfacesShouldConjunction, Interface>, IInterfacesShould
    {
        public InterfacesShould(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}