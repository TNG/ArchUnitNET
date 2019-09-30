using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        ObjectsShouldConjunction<TRuleTypeShould, TRuleTypeShouldConjunctionWithoutBecause, TRuleType> :
            ObjectsShouldConjunctionWithoutBecause<TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunctionWithoutBecause Because(string reason)
        {
            _ruleCreator.AddConditionReason(reason);
            return Create<TRuleTypeShouldConjunctionWithoutBecause, TRuleType>(_ruleCreator);
        }
    }
}