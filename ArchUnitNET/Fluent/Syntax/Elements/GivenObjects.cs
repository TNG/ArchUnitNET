using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class GivenObjects<TRuleTypeThat, TRuleTypeShould, TRuleType> : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjects(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeThat That()
        {
            return CreateSyntaxElement<TRuleTypeThat, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould Should()
        {
            return CreateSyntaxElement<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}