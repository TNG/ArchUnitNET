using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public sealed class GivenMethodMembersThat
        : AddMethodMemberPredicate<GivenMethodMembersConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<MethodMember> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenMethodMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenMethodMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IPredicate<MethodMember> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenMethodMembersConjunction CreateNextElement(
            IPredicate<MethodMember> predicate
        ) =>
            new GivenMethodMembersConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<MethodMember>(
                        _leftPredicate,
                        _logicalConjunction,
                        predicate
                    )
            );
    }
}
