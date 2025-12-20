using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class GivenTypesThat : AddTypePredicate<GivenTypesConjunction, IType>
    {
        [CanBeNull]
        private readonly IPredicate<IType> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenTypesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenTypesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IPredicate<IType> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenTypesConjunction CreateNextElement(IPredicate<IType> predicate) =>
            new GivenTypesConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<IType>(_leftPredicate, _logicalConjunction, predicate)
            );
    }
}
