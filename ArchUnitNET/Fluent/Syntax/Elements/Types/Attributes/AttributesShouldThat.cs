using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Attribute, TRuleType>,
        IAttributesThat<TRuleTypeShouldConjunction>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        public AttributesShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            System.Func<TRuleType, Attribute, bool> relationCondition) : base(ruleCreator,
            architecture => architecture.Attributes, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                attribute => attribute.IsAbstract);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                attribute => !attribute.IsAbstract);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}