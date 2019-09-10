using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfaces : GivenObjects<GivenInterfacesThat, InterfacesShould, Interface>
    {
        public GivenInterfaces(ArchRuleCreator<Interface> ruleCreator) : base(ruleCreator)
        {
        }
    }
}