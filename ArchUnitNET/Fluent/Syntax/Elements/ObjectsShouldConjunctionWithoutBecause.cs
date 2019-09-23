using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShouldConjunctionWithoutBecause<TRuleTypeShould, TRuleType> : ArchRule<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunctionWithoutBecause(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShould AndShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.And);
            return CreateSyntaxElement<TRuleTypeShould, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould OrShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.Or);
            return CreateSyntaxElement<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}