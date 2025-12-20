using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembersConjunction
        : GivenObjectsConjunction<
            GivenFieldMembers,
            GivenFieldMembersThat,
            FieldMembersShould,
            GivenFieldMembersConjunctionWithDescription,
            FieldMember
        >
    {
        internal GivenFieldMembersConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IPredicate<FieldMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenFieldMembers As(string description) =>
            new GivenFieldMembers(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<FieldMember>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenFieldMembersConjunctionWithDescription Because(string reason) =>
            new GivenFieldMembersConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

        public override GivenFieldMembersThat And() =>
            new GivenFieldMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenFieldMembersThat Or() =>
            new GivenFieldMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override FieldMembersShould Should() =>
            new FieldMembersShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<FieldMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
