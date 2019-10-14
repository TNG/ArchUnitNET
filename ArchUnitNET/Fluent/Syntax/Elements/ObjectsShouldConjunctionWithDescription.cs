using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShouldConjunctionWithDescription<TRuleTypeShould, TRuleType> : ArchRule<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunctionWithDescription(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShould AndShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.And);
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShould OrShould()
        {
            _ruleCreator.AddConditionConjunction(LogicalConjunctionDefinition.Or);
            return Create<TRuleTypeShould, TRuleType>(_ruleCreator);
        }
    }
}