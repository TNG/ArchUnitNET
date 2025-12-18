using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public abstract class AddAttributePredicate<TNextElement, TRelatedType>
        : AddTypePredicate<TNextElement, TRelatedType, Attribute>,
            IAddAttributePredicate<TNextElement, Attribute>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddAttributePredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start

        public TNextElement AreAbstract() => CreateNextElement(AttributePredicatesDefinition.AreAbstract());
        public TNextElement AreSealed() => CreateNextElement(AttributePredicatesDefinition.AreSealed());

        //Negations

        public TNextElement AreNotAbstract() => CreateNextElement(AttributePredicatesDefinition.AreNotAbstract());
        public TNextElement AreNotSealed() => CreateNextElement(AttributePredicatesDefinition.AreNotSealed());

        // csharpier-ignore-end
    }
}
