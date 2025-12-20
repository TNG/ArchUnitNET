using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunction
        : GivenObjectsConjunction<
            GivenMembers,
            GivenMembersThat,
            MembersShould,
            GivenMembersConjunctionWithDescription,
            IMember
        >
    {
        internal GivenMembersConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IPredicate<IMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenMembers As(string description) =>
            new GivenMembers(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<IMember>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenMembersConjunctionWithDescription Because(string reason) =>
            new GivenMembersConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

        public override GivenMembersThat And() =>
            new GivenMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenMembersThat Or() =>
            new GivenMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override MembersShould Should() =>
            new MembersShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<IMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
