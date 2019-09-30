﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class ShouldRelateToInterfacesThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToTypesThat<TRuleTypeShouldConjunction, Interface, TRuleType>,
        IInterfacesThat<TRuleTypeShouldConjunction> where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToInterfacesThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }
    }
}