using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenMembersThat, MembersShould, IMember>
    {
        internal GivenMembersConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider,
            IPredicate<IMember> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

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
