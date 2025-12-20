using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<InterfacesShould, Interface>
    {
        internal InterfacesShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IOrderedCondition<Interface> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override InterfacesShould AndShould() =>
            new InterfacesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override InterfacesShould OrShould() =>
            new InterfacesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
