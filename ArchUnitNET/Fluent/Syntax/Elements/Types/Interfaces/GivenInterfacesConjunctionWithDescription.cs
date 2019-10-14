using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunctionWithDescription : GivenObjectsConjunctionWithDescription<GivenInterfacesThat,
        InterfacesShould, Interface>
    {
        public GivenInterfacesConjunctionWithDescription(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}