using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface
        IMembersShould<TRuleTypeShouldConjunction, TRuleType> : IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern);
        TRuleTypeShouldConjunction HaveMethodCallDependencies();
        TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern);
        TRuleTypeShouldConjunction HaveFieldTypeDependencies();
        TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern);


        //Negations


        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies(string pattern);
        TRuleTypeShouldConjunction NotHaveMethodCallDependencies();
        TRuleTypeShouldConjunction NotHaveMethodCallDependencies(string pattern);
        TRuleTypeShouldConjunction NotHaveFieldTypeDependencies();
        TRuleTypeShouldConjunction NotHaveFieldTypeDependencies(string pattern);
    }
}