using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShould : AddAttributeCondition<AttributesShouldConjunction>
    {
        private readonly IOrderedCondition<Attribute> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public AttributesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public AttributesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IOrderedCondition<Attribute> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override AttributesShouldConjunction CreateNextElement(
            IOrderedCondition<Attribute> condition
        ) =>
            new AttributesShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<Attribute>(
                        _leftCondition,
                        _logicalConjunction,
                        condition
                    )
            );
    }
}
