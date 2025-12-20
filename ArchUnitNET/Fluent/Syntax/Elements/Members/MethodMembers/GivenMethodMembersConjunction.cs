using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersConjunction
        : GivenObjectsConjunction<
            GivenMethodMembers,
            GivenMethodMembersThat,
            MethodMembersShould,
            GivenMethodMembersConjunctionWithDescription,
            MethodMember
        >
    {
        internal GivenMethodMembersConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IPredicate<MethodMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenMethodMembers As(string description) =>
            new GivenMethodMembers(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<MethodMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescription(description)
            );

        public override GivenMethodMembersConjunctionWithDescription Because(string reason) =>
            new GivenMethodMembersConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

        public override GivenMethodMembersThat And() =>
            new GivenMethodMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenMethodMembersThat Or() =>
            new GivenMethodMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override MethodMembersShould Should() =>
            new MethodMembersShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<MethodMember>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
