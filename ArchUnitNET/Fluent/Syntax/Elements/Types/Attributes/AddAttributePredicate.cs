using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public abstract class AddAttributePredicate<TNextElement>
        : AddTypePredicate<TNextElement, Attribute>,
            IAddAttributePredicate<TNextElement, Attribute>
    {
        internal AddAttributePredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start

        public TNextElement AreAbstract() => CreateNextElement(AttributePredicatesDefinition.AreAbstract());
        public TNextElement AreSealed() => CreateNextElement(AttributePredicatesDefinition.AreSealed());

        //Negations

        public TNextElement AreNotAbstract() => CreateNextElement(AttributePredicatesDefinition.AreNotAbstract());
        public TNextElement AreNotSealed() => CreateNextElement(AttributePredicatesDefinition.AreNotSealed());

        // csharpier-ignore-end
    }
}
