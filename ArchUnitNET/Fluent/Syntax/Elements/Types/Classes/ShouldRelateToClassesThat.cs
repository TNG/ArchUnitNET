using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
    public sealed class ShouldRelateToClassesThat<TNextElement, TRuleType>
        : AddClassPredicate<TNextElement>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TNextElement,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, Class> _relationCondition;

        public ShouldRelateToClassesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> relatedObjectProvider,
            PartialConditionConjunction<TNextElement, TRuleType> partialConditionConjunction,
            RelationCondition<TRuleType, Class> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TNextElement CreateNextElement(IPredicate<Class> predicate) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<Class>(ObjectProvider, predicate)
                )
            );
    }
}
