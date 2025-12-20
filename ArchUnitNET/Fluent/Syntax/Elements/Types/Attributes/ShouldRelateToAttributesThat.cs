using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public sealed class ShouldRelateToAttributesThat<TNextElement, TRuleType>
        : AddAttributePredicate<TNextElement>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, Attribute> _relationCondition;

        public ShouldRelateToAttributesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, Attribute> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<Attribute> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<Attribute>(ObjectProvider, predicate)
                )
            );
    }
}
