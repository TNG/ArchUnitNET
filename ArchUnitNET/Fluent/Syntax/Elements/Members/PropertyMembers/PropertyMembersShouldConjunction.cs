using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShouldConjunction
        : ObjectsShouldConjunction<
            PropertyMembersShould,
            PropertyMembersShouldConjunctionWithDescription,
            PropertyMember
        >
    {
        public PropertyMembersShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider,
            IOrderedCondition<PropertyMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override PropertyMembersShouldConjunctionWithDescription As(string description) =>
            new PropertyMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override PropertyMembersShouldConjunctionWithDescription Because(string reason) =>
            new PropertyMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

        public override PropertyMembersShould AndShould() =>
            new PropertyMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override PropertyMembersShould OrShould() =>
            new PropertyMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
