using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunction
        : GivenObjectsConjunction<
            GivenInterfacesThat,
            InterfacesShould,
            GivenInterfacesConjunctionWithDescription,
            Interface
        >
    {
        public GivenInterfacesConjunction(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
