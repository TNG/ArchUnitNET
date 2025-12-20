using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class FieldMembersShould : AddFieldMemberCondition<FieldMembersShouldConjunction>
    {
        private readonly IOrderedCondition<FieldMember> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public FieldMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public FieldMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IOrderedCondition<FieldMember> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override FieldMembersShouldConjunction CreateNextElement(
            IOrderedCondition<FieldMember> condition
        ) =>
            new FieldMembersShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<FieldMember>(
                        _leftCondition,
                        _logicalConjunction,
                        condition
                    )
            );
    }
}
