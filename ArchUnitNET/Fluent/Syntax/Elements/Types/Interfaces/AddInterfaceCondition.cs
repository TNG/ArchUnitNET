using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public abstract class AddInterfaceCondition<TNextElement>
        : AddTypeCondition<Interface, TNextElement>,
            IInterfaceConditions<TNextElement, Interface>
    {
        internal AddInterfaceCondition(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }
    }
}
