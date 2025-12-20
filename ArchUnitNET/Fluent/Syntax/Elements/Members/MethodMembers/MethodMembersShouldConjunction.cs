using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldConjunction
        : ObjectsShouldConjunction<
            MethodMembersShould,
            MethodMembersShouldConjunctionWithDescription,
            MethodMember
        >
    {
        public MethodMembersShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IOrderedCondition<MethodMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override MethodMembersShouldConjunctionWithDescription As(string description) =>
            new MethodMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override MethodMembersShouldConjunctionWithDescription Because(string reason) =>
            new MethodMembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

        public override MethodMembersShould AndShould() =>
            new MethodMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override MethodMembersShould OrShould() =>
            new MethodMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
