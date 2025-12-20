using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfacesConjunction
        : GivenObjectsConjunction<
            GivenInterfaces,
            GivenInterfacesThat,
            InterfacesShould,
            GivenInterfacesConjunctionWithDescription,
            Interface
        >
    {
        internal GivenInterfacesConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IPredicate<Interface> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenInterfaces As(string description) =>
            new GivenInterfaces(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Interface>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenInterfacesConjunctionWithDescription Because(string reason) =>
            new GivenInterfacesConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

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
