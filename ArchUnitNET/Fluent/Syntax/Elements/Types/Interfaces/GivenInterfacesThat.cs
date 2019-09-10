using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesThat : GivenTypesThat<GivenInterfacesConjunction, Interface>,
        IInterfacesThat<GivenInterfacesConjunction>
    {
        public GivenInterfacesThat(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}