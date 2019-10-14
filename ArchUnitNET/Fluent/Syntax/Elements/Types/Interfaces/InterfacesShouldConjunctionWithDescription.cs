using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class
        InterfacesShouldConjunctionWithDescription : ObjectsShouldConjunctionWithDescription<InterfacesShould, Interface
        >
    {
        public InterfacesShouldConjunctionWithDescription(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}