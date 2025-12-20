using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembers
        : GivenObjects<GivenMethodMembersThat, MethodMembersShould, MethodMember>
    {
        internal GivenMethodMembers(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<MethodMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenMethodMembersThat That() =>
            new GivenMethodMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override MethodMembersShould Should() =>
            new MethodMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
