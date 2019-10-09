using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class ShouldRelateToMethodMembersThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToMembersThat<TRuleTypeShouldConjunction, MethodMember, TRuleType>,
        IMethodMemberPredicates<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToMethodMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreConstructors());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreNoConstructors());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreNotVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}