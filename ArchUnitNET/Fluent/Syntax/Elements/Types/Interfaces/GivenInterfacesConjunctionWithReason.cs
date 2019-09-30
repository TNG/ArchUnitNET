using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunctionWithReason : GivenObjectsConjunctionWithReason<GivenInterfacesThat,
        InterfacesShould, Interface>
    {
        public GivenInterfacesConjunctionWithReason(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}