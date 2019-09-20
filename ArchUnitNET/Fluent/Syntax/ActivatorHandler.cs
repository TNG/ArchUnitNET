﻿using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class ActivatorHandler
    {
        public static TConjunction CreateSyntaxElement<TConjunction, TRuleType>(ArchRuleCreator<TRuleType> ruleCreator)
            where TRuleType : ICanBeAnalyzed
        {
            return (TConjunction) Activator.CreateInstance(typeof(TConjunction), ruleCreator);
        }
    }
}