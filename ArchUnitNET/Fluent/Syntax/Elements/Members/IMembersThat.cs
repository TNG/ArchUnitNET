namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMembersThat<TRuleTypeConjunction> : IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction HaveMethodCallDependencies();
        TRuleTypeConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction HaveFieldTypeDependencies();
        TRuleTypeConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern);


        //Negations


        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies();
        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveMethodCallDependencies();
        TRuleTypeConjunction DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveFieldTypeDependencies();
        TRuleTypeConjunction DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern);
    }
}