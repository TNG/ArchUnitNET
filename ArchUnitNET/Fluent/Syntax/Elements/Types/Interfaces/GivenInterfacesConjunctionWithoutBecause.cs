using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunctionWithoutBecause : GivenObjectsConjunctionWithoutBecause<GivenInterfacesThat,
        InterfacesShould, Interface>
    {
        public GivenInterfacesConjunctionWithoutBecause(IArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}