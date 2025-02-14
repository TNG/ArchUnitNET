using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfaces : GivenObjects<GivenInterfacesThat, InterfacesShould, Interface>
    {
        public GivenInterfaces(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
