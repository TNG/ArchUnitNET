using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class
        InterfacesShouldConjunctionWithoutBecause : ObjectsShouldConjunctionWithoutBecause<InterfacesShould, Interface>
    {
        public InterfacesShouldConjunctionWithoutBecause(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}