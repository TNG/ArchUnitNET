using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public sealed class ShouldRelateToInterfacesThat<TNextElement, TRuleType>
        : AddInterfacePredicate<TNextElement>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, Interface> _relationCondition;

        public ShouldRelateToInterfacesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, Interface> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<Interface> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<Interface>(ObjectProvider, predicate)
                )
            );
    }
}
