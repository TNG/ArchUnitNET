using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public sealed class GivenAttributesThat : AddAttributePredicate<GivenAttributesConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<Attribute> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenAttributesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenAttributesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Attribute> objectProvider,
            IPredicate<Attribute> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenAttributesConjunction CreateNextElement(
            IPredicate<Attribute> predicate
        ) =>
            new GivenAttributesConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<Attribute>(
                        _leftPredicate,
                        _logicalConjunction,
                        predicate
                    )
            );
    }
}
