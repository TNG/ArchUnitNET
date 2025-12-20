using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<MethodMembersShould, MethodMember>
    {
        internal MethodMembersShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IOrderedCondition<MethodMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

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
