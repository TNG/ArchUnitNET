using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunction : GivenObjectsConjunction<GivenInterfacesThat, InterfacesShould,
        GivenInterfacesConjunction, Interface>
    {
        public GivenInterfacesConjunction(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}