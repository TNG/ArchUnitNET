using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class FieldMembersShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<FieldMembersShould, FieldMember>
    {
        internal FieldMembersShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IOrderedCondition<FieldMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

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
