using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IComplexMemberConditions<TRuleTypeShouldConjunction, TRuleType>
        : IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>,
            IMemberConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > BeDeclaredInTypesThat();

        //Negations

        ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotBeDeclaredInTypesThat();
    }
}
