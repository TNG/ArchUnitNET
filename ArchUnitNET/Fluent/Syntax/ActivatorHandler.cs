using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class ActivatorHandler
    {
        public static TConjunction CreateSyntaxElement<TConjunction, TObject>(ArchRuleCreator<TObject> ruleCreator)
            where TObject : ICanBeAnalyzed
        {
            return (TConjunction) Activator.CreateInstance(typeof(TConjunction), ruleCreator);
        }
    }
}