using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public sealed class GivenClassesThat : AddClassPredicate<GivenClassesConjunction>
    {
        [CanBeNull]
        private readonly IPredicate<Class> _leftPredicate;

        [CanBeNull]
        private readonly LogicalConjunction _logicalConjunction;

        internal GivenClassesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        internal GivenClassesThat(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IPredicate<Class> leftPredicate,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftPredicate = leftPredicate;
            _logicalConjunction = logicalConjunction;
        }

        protected override GivenClassesConjunction CreateNextElement(IPredicate<Class> predicate) =>
            new GivenClassesConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftPredicate == null
                    ? predicate
                    : new CombinedPredicate<Class>(_leftPredicate, _logicalConjunction, predicate)
            );
    }
}
