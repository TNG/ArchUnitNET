using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public sealed class GivenInterfacesThat : AddInterfacePredicate<GivenInterfacesConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<Interface> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenInterfacesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenInterfacesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider,
            IPredicate<Interface> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenInterfacesConjunction CreateNextElement(
            IPredicate<Interface> predicate
        ) =>
            new GivenInterfacesConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<Interface>(
                        _leftPredicate,
                        _logicalConjunction,
                        predicate
                    )
            );
    }
}
