using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public abstract class AddClassCondition<TNextElement>
        : AddTypeCondition<TNextElement, Class>,
            IAddClassCondition<TNextElement, Class>
    {
        internal AddClassCondition(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public TNextElement BeAbstract() =>
            CreateNextElement(ClassConditionsDefinition.BeAbstract());

        public TNextElement BeSealed() => CreateNextElement(ClassConditionsDefinition.BeSealed());

        public TNextElement BeRecord() => CreateNextElement(ClassConditionsDefinition.BeRecord());

        public TNextElement BeImmutable() =>
            CreateNextElement(ClassConditionsDefinition.BeImmutable());

        //Negations

        public TNextElement NotBeAbstract() =>
            CreateNextElement(ClassConditionsDefinition.NotBeAbstract());

        public TNextElement NotBeSealed() =>
            CreateNextElement(ClassConditionsDefinition.NotBeSealed());

        public TNextElement NotBeRecord() =>
            CreateNextElement(ClassConditionsDefinition.NotBeRecord());

        public TNextElement NotBeImmutable() =>
            CreateNextElement(ClassConditionsDefinition.NotBeImmutable());
    }
}
