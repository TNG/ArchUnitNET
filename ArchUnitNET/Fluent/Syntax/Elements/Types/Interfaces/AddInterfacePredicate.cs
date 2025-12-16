using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public abstract class AddInterfacePredicate<TRelatedType, TNextElement>
        : AddTypePredicate<Interface, TRelatedType, TNextElement>,
            IInterfacePredicates<TNextElement, Interface>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddInterfacePredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }
    }
}
