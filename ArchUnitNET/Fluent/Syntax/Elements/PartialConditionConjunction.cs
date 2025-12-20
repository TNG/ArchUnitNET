using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class PartialConditionConjunction<TNextElement, TRuleType>
        : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected PartialConditionConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        internal abstract TNextElement CreateNextElement(IOrderedCondition<TRuleType> condition);
    }
}
