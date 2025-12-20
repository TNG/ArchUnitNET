using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<PropertyMembersShould, PropertyMember>
    {
        internal PropertyMembersShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider,
            IOrderedCondition<PropertyMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

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
