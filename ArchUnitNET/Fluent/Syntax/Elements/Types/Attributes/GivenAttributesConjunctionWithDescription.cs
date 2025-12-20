using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenAttributesThat, AttributesShould, Attribute>
    {
        internal GivenAttributesConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IPredicate<Attribute> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenAttributesThat And() =>
            new GivenAttributesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenAttributesThat Or() =>
            new GivenAttributesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override AttributesShould Should() =>
            new AttributesShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Attribute>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
