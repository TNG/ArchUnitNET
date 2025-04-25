using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesThat
        : GivenTypesThat<GivenInterfacesConjunction, Interface>,
            IInterfacePredicates<GivenInterfacesConjunction, Interface>
    {
        public GivenInterfacesThat(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
