using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembers
        : GivenObjects<GivenPropertyMembersThat, PropertyMembersShould, PropertyMember>
    {
        internal GivenPropertyMembers(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenPropertyMembersThat That() =>
            new GivenPropertyMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override PropertyMembersShould Should() =>
            new PropertyMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
