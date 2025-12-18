using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TRuleType>
        : AddTypePredicate<TRuleTypeShouldConjunction, TRuleType, IType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToTypesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        protected override TRuleTypeShouldConjunction CreateNextElement(IPredicate<IType> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
