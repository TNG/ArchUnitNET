using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<AttributesShould, Attribute>
    {
        internal AttributesShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IOrderedCondition<Attribute> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

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
