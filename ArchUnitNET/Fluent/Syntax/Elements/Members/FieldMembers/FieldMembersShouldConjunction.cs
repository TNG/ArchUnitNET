using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldConjunction
        : ObjectsShouldConjunction<
            FieldMembersShould,
            FieldMembersShouldConjunctionWithDescription,
            FieldMember
        >
    {
        public FieldMembersShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IOrderedCondition<FieldMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override FieldMembersShouldConjunctionWithDescription As(string description) =>
            new FieldMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override FieldMembersShouldConjunctionWithDescription Because(string reason) =>
            new FieldMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

        public override FieldMembersShould AndShould() =>
            new FieldMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override FieldMembersShould OrShould() =>
            new FieldMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
