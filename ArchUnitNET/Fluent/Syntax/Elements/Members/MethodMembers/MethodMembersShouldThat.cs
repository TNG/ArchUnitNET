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
        public MethodMembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.MethodMembers)
        {
        }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.IsConstructor(), "are no constructors");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, member => member.IsVirtual, "are virtual");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => !member.IsConstructor(), "are no constructors");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, member => !member.IsVirtual,
                "are not virtual");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}