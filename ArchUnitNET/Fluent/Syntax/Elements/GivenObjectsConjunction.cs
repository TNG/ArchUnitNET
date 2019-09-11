﻿using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class GivenObjectsConjunction<TGivenRuleTypeThat, TRuleTypeShould, TRuleType> : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeThat And()
        {
            _ruleCreator.AddObjectFilterConjunction(LogicalConjunctionDefinition.And);
            return CreateSyntaxElement<TGivenRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeThat Or()
        {
            _ruleCreator.AddObjectFilterConjunction(LogicalConjunctionDefinition.Or);
            return CreateSyntaxElement<TGivenRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould Should()
        {
            return CreateSyntaxElement<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}