using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunction : GivenObjectsConjunction<GivenInterfacesThat, InterfacesShould, Interface>
    {
        public GivenInterfacesConjunction(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}