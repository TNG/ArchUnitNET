using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributes : GivenObjects<GivenAttributesThat, AttributesShould, Attribute>
    {
        internal GivenAttributes(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenAttributesThat That() =>
            new GivenAttributesThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override AttributesShould Should() =>
            new AttributesShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
