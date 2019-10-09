using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShould : TypesShould<InterfacesShouldConjunction, Interface>, IComplexInterfaceConditions
    {
        public InterfacesShould(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}