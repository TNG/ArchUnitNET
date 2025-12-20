using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesConjunction
        : GivenObjectsConjunction<
            GivenAttributes,
            GivenAttributesThat,
            AttributesShould,
            GivenAttributesConjunctionWithDescription,
            Attribute
        >
    {
        internal GivenAttributesConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IPredicate<Attribute> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenAttributes As(string description) =>
            new GivenAttributes(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Attribute>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenAttributesConjunctionWithDescription Because(string reason) =>
            new GivenAttributesConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

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
