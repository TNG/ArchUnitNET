using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunction
        : GivenObjectsConjunction<
            GivenInterfacesThat,
            InterfacesShould,
            GivenInterfacesConjunction,
            Interface
        >
    {
        public GivenInterfacesConjunction(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
