using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public class GivenFieldMembers
        : GivenObjects<GivenFieldMembersThat, FieldMembersShould, FieldMember>
    {
        internal GivenFieldMembers(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<FieldMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenFieldMembersThat That() =>
            new GivenFieldMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override FieldMembersShould Should() =>
            new FieldMembersShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
