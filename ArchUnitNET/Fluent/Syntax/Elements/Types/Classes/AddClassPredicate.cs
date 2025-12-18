using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public abstract class AddClassPredicate<TNextElement, TRelatedType>
        : AddTypePredicate<TNextElement, TRelatedType, Class>,
            IAddClassPredicate<TNextElement, Class>
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddClassPredicate(IArchRuleCreator<TRelatedType> ruleCreator)
            : base(ruleCreator) { }

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
