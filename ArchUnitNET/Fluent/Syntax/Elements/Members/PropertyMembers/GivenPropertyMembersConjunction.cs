using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersConjunction
        : GivenObjectsConjunction<
            GivenPropertyMembers,
            GivenPropertyMembersThat,
            PropertyMembersShould,
            GivenPropertyMembersConjunctionWithDescription,
            PropertyMember
        >
    {
        internal GivenPropertyMembersConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider,
            IPredicate<PropertyMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenPropertyMembers As(string description) =>
            new GivenPropertyMembers(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<PropertyMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescription(description)
            );

        public override GivenPropertyMembersConjunctionWithDescription Because(string reason) =>
            new GivenPropertyMembersConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

        public override GivenPropertyMembersThat And() =>
            new GivenPropertyMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenPropertyMembersThat Or() =>
            new GivenPropertyMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override PropertyMembersShould Should() =>
            new PropertyMembersShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<PropertyMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
