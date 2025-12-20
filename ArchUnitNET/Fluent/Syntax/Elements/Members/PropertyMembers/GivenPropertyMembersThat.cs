using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public sealed class GivenPropertyMembersThat
        : AddPropertyMemberPredicate<GivenPropertyMembersConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<PropertyMember> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenPropertyMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenPropertyMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider,
            IPredicate<PropertyMember> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenPropertyMembersConjunction CreateNextElement(
            IPredicate<PropertyMember> predicate
        ) =>
            new GivenPropertyMembersConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<PropertyMember>(
                        _leftPredicate,
                        _logicalConjunction,
                        predicate
                    )
            );
    }
}
