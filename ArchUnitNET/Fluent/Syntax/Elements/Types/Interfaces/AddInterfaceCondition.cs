using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public abstract class AddInterfaceCondition<TNextElement>
        : AddTypeCondition<TNextElement, Interface>,
            IAddInterfaceCondition<TNextElement, Interface>
    {
        internal AddInterfaceCondition(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }
    }
}
