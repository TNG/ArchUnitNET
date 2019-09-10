using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class ActivatorHandler
    {
        public static TConjunction CreateSyntaxElement<TConjunction, TObject>(ArchRuleCreator<TObject> ruleCreator)
            where TObject : ICanBeAnalyzed
        {
            return (TConjunction) Activator.CreateInstance(typeof(TConjunction), ruleCreator);
        }

        public static TObjectsShouldThat
            CreateSyntaxElement<TObjectsShouldThat, TObjectsShouldConjunction, TObject, TRuleType>(
                ArchRuleCreator<TRuleType> ruleCreator,
                Func<Architecture, IEnumerable<TObject>> referenceObjectProvider,
                Func<TRuleType, TObject, bool> relationCondition)
            where TObjectsShouldThat : ObjectsShouldThat<TObjectsShouldConjunction, TObject, TRuleType>
            where TObject : ICanBeAnalyzed
            where TRuleType : ICanBeAnalyzed
        {
            return (TObjectsShouldThat) Activator.CreateInstance(typeof(TObjectsShouldThat), ruleCreator,
                referenceObjectProvider, relationCondition);
        }
    }
}