using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
    public sealed class ShouldRelateToFieldMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : AddFieldMemberPredicate<TRuleTypeShouldConjunction>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly PartialConditionConjunction<
            TRuleTypeShouldConjunction,
            TRuleType
        > _partialConditionConjunction;

        private readonly RelationCondition<TRuleType, FieldMember> _relationCondition;

        public ShouldRelateToFieldMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> relatedObjectProvider,
            PartialConditionConjunction<
                TRuleTypeShouldConjunction,
                TRuleType
            > partialConditionConjunction,
            RelationCondition<TRuleType, FieldMember> relationCondition
        )
            : base(partialArchRuleConjunction, relatedObjectProvider)
        {
            _partialConditionConjunction = partialConditionConjunction;
            _relationCondition = relationCondition;
        }

        protected override TRuleTypeShouldConjunction CreateNextElement(
            IPredicate<FieldMember> predicate
        ) =>
            _partialConditionConjunction.CreateNextElement(
                _relationCondition.GetCondition(
                    new PredicateObjectProvider<FieldMember>(ObjectProvider, predicate)
                )
            );
    }
}
