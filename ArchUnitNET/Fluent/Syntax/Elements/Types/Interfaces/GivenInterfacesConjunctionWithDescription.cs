using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenInterfacesThat, InterfacesShould, Interface>
    {
        internal GivenInterfacesConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IPredicate<Interface> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenInterfacesThat And() =>
            new GivenInterfacesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenInterfacesThat Or() =>
            new GivenInterfacesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override InterfacesShould Should() =>
            new InterfacesShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Interface>(
                    ObjectProvider,
                    Predicate
                ).WithDescriptionSuffix("should")
            );
    }
}
