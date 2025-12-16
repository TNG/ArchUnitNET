using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShould
        : AddInterfaceCondition<InterfacesShouldConjunction>
    {
        public InterfacesShould(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }

        protected override InterfacesShouldConjunction CreateNextElement(IOrderedCondition<Interface> condition)
        {
            _ruleCreator.AddCondition(condition);
            return new InterfacesShouldConjunction(_ruleCreator);
        }
    }
}
