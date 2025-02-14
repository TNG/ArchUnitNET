using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldConjunction
        : ObjectsShouldConjunction<
            InterfacesShould,
            InterfacesShouldConjunctionWithDescription,
            Interface
        >
    {
        public InterfacesShouldConjunction(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
