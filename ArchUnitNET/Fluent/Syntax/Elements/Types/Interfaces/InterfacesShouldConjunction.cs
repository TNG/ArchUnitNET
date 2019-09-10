using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldConjunction : ObjectsShouldConjunction<InterfacesShould, Interface>
    {
        public InterfacesShouldConjunction(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}