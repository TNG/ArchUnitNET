using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldConjunction
        : ObjectsShouldConjunction<MembersShould, MembersShouldConjunctionWithDescription, IMember>
    {
        public MembersShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IOrderedCondition<IMember> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override MembersShouldConjunctionWithDescription As(string description) =>
            new MembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override MembersShouldConjunctionWithDescription Because(string reason) =>
            new MembersShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

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
