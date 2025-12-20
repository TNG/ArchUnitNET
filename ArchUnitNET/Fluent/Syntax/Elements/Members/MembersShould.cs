using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public sealed class MembersShould : AddMemberCondition<MembersShouldConjunction, IMember>
    {
        private readonly IOrderedCondition<IMember> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public MembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public MembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IOrderedCondition<IMember> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override MembersShouldConjunction CreateNextElement(
            IOrderedCondition<IMember> condition
        ) =>
            new MembersShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<IMember>(_leftCondition, _logicalConjunction, condition)
            );
    }
}
