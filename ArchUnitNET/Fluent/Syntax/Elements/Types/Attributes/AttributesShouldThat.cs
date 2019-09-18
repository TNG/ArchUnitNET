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
        public AttributesShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            architecture => architecture.Attributes)
        {
        }

        public TRuleTypeShouldConjunction AreAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                attribute => attribute.IsAbstract, "are abstract");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotAbstract()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                attribute => !attribute.IsAbstract, "are not abstract");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}