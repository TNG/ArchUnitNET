using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public sealed class GivenFieldMembersThat
        : AddFieldMemberPredicate<GivenFieldMembersConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<FieldMember> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenFieldMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenFieldMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IPredicate<FieldMember> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenFieldMembersConjunction CreateNextElement(
            IPredicate<FieldMember> predicate
        ) =>
            new GivenFieldMembersConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<FieldMember>(
                        _leftPredicate,
                        _logicalConjunction,
                        predicate
                    )
            );
    }
}
