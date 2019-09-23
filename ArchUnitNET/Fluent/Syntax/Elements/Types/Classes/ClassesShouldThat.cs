using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Class, TRuleType>,
        IClassesThat<TRuleTypeShouldConjunction> where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ClassesShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.Classes)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, ClassesFilterDefinition.AreAbstract());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, ClassesFilterDefinition.AreNotAbstract());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}