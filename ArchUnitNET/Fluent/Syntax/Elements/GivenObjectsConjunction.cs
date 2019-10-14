using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        GivenObjectsConjunction<TGivenRuleTypeThat, TRuleTypeShould, TGivenRuleTypeConjunctionWithReason, TRuleType>
        : GivenObjectsConjunctionWithDescription<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunctionWithReason Because(string reason)
        {
            _ruleCreator.AddPredicateReason(reason);
            return Create<TGivenRuleTypeConjunctionWithReason, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunctionWithReason As(string description)
        {
            _ruleCreator.SetCustomPredicateDescription(description);
            return Create<TGivenRuleTypeConjunctionWithReason, TRuleType>(_ruleCreator);
        }
    }
}