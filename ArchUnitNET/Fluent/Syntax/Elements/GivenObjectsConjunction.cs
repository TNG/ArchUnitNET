using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class
        GivenObjectsConjunction<TGivenRuleTypeThat, TRuleTypeShould, TGivenRuleTypeConjunctionWithoutBecause, TRuleType>
        : GivenObjectsConjunctionWithoutBecause<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunctionWithoutBecause Because(string reason)
        {
            _ruleCreator.AddFilterReason(reason);
            return Create<TGivenRuleTypeConjunctionWithoutBecause, TRuleType>(_ruleCreator);
        }
    }
}