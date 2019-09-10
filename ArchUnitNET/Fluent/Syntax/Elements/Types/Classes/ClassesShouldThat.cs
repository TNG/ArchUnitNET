using System;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Class, TRuleType>,
        IClassesThat<TRuleTypeShouldConjunction> where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ClassesShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<TRuleType, Class, bool> relationCondition) : base(
            ruleCreator, architecture => architecture.Classes, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, cls => cls.IsAbstract);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, cls => !cls.IsAbstract);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}