using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembers : GivenObjects<GivenMembersThat, MembersShould, IMember>
    {
        internal GivenMembers(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenMembersThat That() =>
            new GivenMembersThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override MembersShould Should() =>
            new MembersShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
