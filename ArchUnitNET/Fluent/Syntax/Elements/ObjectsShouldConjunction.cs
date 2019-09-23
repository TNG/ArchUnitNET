using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        ObjectsShouldConjunction<TRuleTypeShould, TRuleTypeShouldConjunctionWithoutBecause, TRuleType> :
            ObjectsShouldConjunctionWithoutBecause<TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunction(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunctionWithoutBecause Because(string reason)
        {
            _ruleCreator.AddConditionReason(reason);
            return CreateSyntaxElement<TRuleTypeShouldConjunctionWithoutBecause, TRuleType>(_ruleCreator);
        }
    }
}