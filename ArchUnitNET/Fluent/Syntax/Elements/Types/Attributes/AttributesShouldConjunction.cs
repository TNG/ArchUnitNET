using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldConjunction
        : ObjectsShouldConjunction<
            AttributesShould,
            AttributesShouldConjunctionWithDescription,
            Attribute
        >
    {
        public AttributesShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IOrderedCondition<Attribute> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override AttributesShouldConjunctionWithDescription As(string description) =>
            new AttributesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override AttributesShouldConjunctionWithDescription Because(string reason) =>
            new AttributesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

        public override AttributesShould AndShould() =>
            new AttributesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override AttributesShould OrShould() =>
            new AttributesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
