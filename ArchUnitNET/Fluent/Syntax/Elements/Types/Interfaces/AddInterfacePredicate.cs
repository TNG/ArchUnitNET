using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public abstract class AddInterfacePredicate<TNextElement>
        : AddTypePredicate<TNextElement, Interface>,
            IAddInterfacePredicate<TNextElement, Interface>
    {
        internal AddInterfacePredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }
    }
}
