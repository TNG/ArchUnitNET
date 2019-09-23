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
        public AttributesShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.Attributes)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, AttributesFilterDefinition.AreAbstract());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                AttributesFilterDefinition.AreNotAbstract());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}