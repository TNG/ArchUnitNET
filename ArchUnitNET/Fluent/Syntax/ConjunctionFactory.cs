using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class ConjunctionFactory
    {
        public static TConjunction Create<TConjunction, TRuleType>(IArchRuleCreator<TRuleType> ruleCreator)
            where TRuleType : ICanBeAnalyzed
        {
            return (TConjunction) Activator.CreateInstance(typeof(TConjunction), ruleCreator);
        }
    }
}