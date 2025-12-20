using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public sealed class GivenMembersThat : AddMemberPredicate<GivenMembersConjunction, IMember>
    {
        [CanBeNull]
        private readonly IPredicate<IMember> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenMembersThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IPredicate<IMember> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenMembersConjunction CreateNextElement(
            IPredicate<IMember> predicate
        ) =>
            new GivenMembersConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<IMember>(_leftPredicate, _logicalConjunction, predicate)
            );
    }
}
