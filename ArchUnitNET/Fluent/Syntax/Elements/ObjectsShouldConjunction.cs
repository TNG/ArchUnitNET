using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        ObjectsShouldConjunction<TRuleTypeShould, TRuleTypeShouldConjunctionWithReason, TRuleType> :
            ObjectsShouldConjunctionWithReason<TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunctionWithReason Because(string reason)
        {
            _ruleCreator.AddConditionReason(reason);
            return Create<TRuleTypeShouldConjunctionWithReason, TRuleType>(_ruleCreator);
        }
    }
}