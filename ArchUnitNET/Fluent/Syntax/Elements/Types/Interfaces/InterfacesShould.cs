using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShould : AddInterfaceCondition<InterfacesShouldConjunction>
    {
        private readonly IOrderedCondition<Interface> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public InterfacesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public InterfacesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IOrderedCondition<Interface> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override InterfacesShouldConjunction CreateNextElement(
            IOrderedCondition<Interface> condition
        ) =>
            new InterfacesShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<Interface>(
                        _leftCondition,
                        _logicalConjunction,
                        condition
                    )
            );
    }
}
