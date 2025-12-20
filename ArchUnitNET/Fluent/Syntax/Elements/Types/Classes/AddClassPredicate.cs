using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public abstract class AddClassPredicate<TNextElement>
        : AddTypePredicate<TNextElement, Class>,
            IAddClassPredicate<TNextElement, Class>
    {
        internal AddClassPredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start

        public TNextElement AreAbstract() => CreateNextElement(ClassPredicatesDefinition.AreAbstract());
        public TNextElement AreSealed() => CreateNextElement(ClassPredicatesDefinition.AreSealed());
        public TNextElement AreRecord() => CreateNextElement(ClassPredicatesDefinition.AreRecord());
        public TNextElement AreImmutable() => CreateNextElement(ClassPredicatesDefinition.AreImmutable());

        //Negations

        public TNextElement AreNotAbstract() => CreateNextElement(ClassPredicatesDefinition.AreNotAbstract());
        public TNextElement AreNotSealed() => CreateNextElement(ClassPredicatesDefinition.AreNotSealed());
        public TNextElement AreNotRecord() => CreateNextElement(ClassPredicatesDefinition.AreNotRecord());
        public TNextElement AreNotImmutable() => CreateNextElement(ClassPredicatesDefinition.AreNotImmutable());

        // csharpier-ignore-end
    }
}
