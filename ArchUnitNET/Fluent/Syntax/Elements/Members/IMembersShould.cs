using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface
        IMembersShould<TRuleTypeShouldConjunction, TRuleType> : IObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HaveMethodCallDependencies();
        TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HaveFieldTypeDependencies();
        TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern);


        //Negations


        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHaveMethodCallDependencies();
        TRuleTypeShouldConjunction NotHaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHaveFieldTypeDependencies();
        TRuleTypeShouldConjunction NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern);
    }
}