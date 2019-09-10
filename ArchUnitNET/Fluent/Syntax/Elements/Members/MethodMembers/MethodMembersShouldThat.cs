using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class MethodMembersShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        MembersShouldThat<TRuleTypeShouldConjunction, MethodMember, TRuleType>,
        IMethodMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public MethodMembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<TRuleType, MethodMember, bool> relationCondition) : base(ruleCreator,
            architecture => architecture.MethodMembers, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.IsConstructor());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, member => member.IsVirtual);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.IsConstructor());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, member => !member.IsVirtual);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}