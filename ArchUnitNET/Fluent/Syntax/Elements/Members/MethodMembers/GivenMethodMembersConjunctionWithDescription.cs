using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<
            GivenMethodMembersThat,
            MethodMembersShould,
            MethodMember
        >
    {
        internal GivenMethodMembersConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider,
            IPredicate<MethodMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

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
