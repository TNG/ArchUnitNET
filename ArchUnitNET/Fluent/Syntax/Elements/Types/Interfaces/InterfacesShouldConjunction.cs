using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldConjunction
        : ObjectsShouldConjunction<
            InterfacesShould,
            InterfacesShouldConjunctionWithDescription,
            Interface
        >
    {
        public InterfacesShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IOrderedCondition<Interface> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override InterfacesShouldConjunctionWithDescription As(string description) =>
            new InterfacesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override InterfacesShouldConjunctionWithDescription Because(string reason) =>
            new InterfacesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

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
