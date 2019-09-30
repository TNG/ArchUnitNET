using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        GivenObjectsConjunction<TGivenRuleTypeThat, TRuleTypeShould, TGivenRuleTypeConjunctionWithReason, TRuleType>
        : GivenObjectsConjunctionWithReason<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunctionWithReason Because(string reason)
        {
            _ruleCreator.AddFilterReason(reason);
            return Create<TGivenRuleTypeConjunctionWithReason, TRuleType>(_ruleCreator);
        }
    }
}