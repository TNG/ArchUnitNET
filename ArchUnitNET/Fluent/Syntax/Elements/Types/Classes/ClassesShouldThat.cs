using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Class, TRuleType>,
        IClassesThat<TRuleTypeShouldConjunction> where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ClassesShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreSealed()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreValueTypes()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreEnums()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreStructs()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreNotAbstract());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotSealed()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreNotSealed());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotValueTypes()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreNotValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotEnums()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreNotEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotStructs()
        {
            _ruleCreator.ContinueComplexCondition(ClassesFilterDefinition.AreNotStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}