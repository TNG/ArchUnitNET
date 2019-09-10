namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMembersThat<TRuleTypeConjunction> : IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeConjunction HaveBodyTypeMemberDependencies(string pattern);
        TRuleTypeConjunction HaveMethodCallDependencies();
        TRuleTypeConjunction HaveMethodCallDependencies(string pattern);
        TRuleTypeConjunction HaveFieldTypeDependencies();
        TRuleTypeConjunction HaveFieldTypeDependencies(string pattern);


        //Negations


        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies();
        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies(string pattern);
        TRuleTypeConjunction DoNotHaveMethodCallDependencies();
        TRuleTypeConjunction DoNotHaveMethodCallDependencies(string pattern);
        TRuleTypeConjunction DoNotHaveFieldTypeDependencies();
        TRuleTypeConjunction DoNotHaveFieldTypeDependencies(string pattern);
    }
}