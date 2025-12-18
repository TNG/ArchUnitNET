using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public abstract class AddInterfacePredicate<TNextElement, TRelatedType>
        : AddTypePredicate<TNextElement, TRelatedType, Interface>,
            IAddInterfacePredicate<TNextElement, Interface>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddInterfacePredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }
    }
}
