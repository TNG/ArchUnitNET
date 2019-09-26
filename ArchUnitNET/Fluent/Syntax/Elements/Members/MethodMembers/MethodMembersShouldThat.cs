using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        MembersShouldThat<TRuleTypeShouldConjunction, MethodMember, TRuleType>,
        IMethodMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public MethodMembersShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreConstructors());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreVirtual());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreNoConstructors());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMembersFilterDefinition.AreNotVirtual());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}