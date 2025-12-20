using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<MembersShould, IMember>
    {
        internal MembersShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IOrderedCondition<IMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override MembersShould AndShould() =>
            new MembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override MembersShould OrShould() =>
            new MembersShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
