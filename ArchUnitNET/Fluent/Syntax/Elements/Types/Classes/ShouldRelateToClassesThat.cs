using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    // No fluent method currently returns this class - it can't be reached, only constructed
    // reflectively by a caller with a hand-built IArchRuleCreator. Kept for API symmetry with the
    // other ShouldRelateTo*That classes rather than removed as a breaking change.
    public sealed class ShouldRelateToClassesThat<TNextElement, TRuleType>
        : AddClassPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToClassesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TNextElement CreateNextElement(IPredicate<Class> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TNextElement, TRuleType>(_ruleCreator);
        }
    }
}
