using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class
        InterfacesShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<InterfacesShould, Interface>
    {
        public InterfacesShouldConjunctionWithReason(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}