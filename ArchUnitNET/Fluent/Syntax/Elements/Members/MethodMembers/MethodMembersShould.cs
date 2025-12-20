using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShould : AddMethodMemberCondition<MethodMembersShouldConjunction>
    {
        private readonly IOrderedCondition<MethodMember> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public MethodMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public MethodMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IOrderedCondition<MethodMember> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override MethodMembersShouldConjunction CreateNextElement(
            IOrderedCondition<MethodMember> condition
        ) =>
            new MethodMembersShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<MethodMember>(
                        _leftCondition,
                        _logicalConjunction,
                        condition
                    )
            );
    }
}
