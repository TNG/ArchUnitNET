using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Interface, TRuleType>,
        IInterfacesThat<TRuleTypeShouldConjunction> where TRuleType : ICanBeAnalyzed
    {
        public InterfacesShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            architecture => architecture.Interfaces)
        {
        }
    }
}