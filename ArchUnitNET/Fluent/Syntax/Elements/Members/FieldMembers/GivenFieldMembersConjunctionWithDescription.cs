using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenFieldMembersThat,
            FieldMembersShould,
            FieldMember
        >
    {
        internal GivenFieldMembersConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider,
            IPredicate<FieldMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

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
