using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould
        : AddPropertyMemberCondition<PropertyMembersShouldConjunction>
    {
        private readonly IOrderedCondition<PropertyMember> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public PropertyMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public PropertyMembersShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider,
            IOrderedCondition<PropertyMember> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override PropertyMembersShouldConjunction CreateNextElement(
            IOrderedCondition<PropertyMember> condition
        ) =>
            new PropertyMembersShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<PropertyMember>(
                        _leftCondition,
                        _logicalConjunction,
                        condition
                    )
            );
    }
}
