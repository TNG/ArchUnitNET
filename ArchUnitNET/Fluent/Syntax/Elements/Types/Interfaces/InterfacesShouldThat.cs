using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class InterfacesShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        TypesShouldThat<TRuleTypeShouldConjunction, Interface, TRuleType>,
        IInterfacesThat<TRuleTypeShouldConjunction> where TRuleType : ICanBeAnalyzed
    {
        protected InterfacesShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<TRuleType, Interface, bool> relationCondition) : base(ruleCreator,
            architecture => architecture.Interfaces, relationCondition)
        {
        }
    }
}