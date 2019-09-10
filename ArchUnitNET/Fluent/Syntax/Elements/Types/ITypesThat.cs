namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypesThat<TRuleTypeConjunction> : IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction ImplementInterface(string pattern);
        TRuleTypeConjunction ResideInNamespace(string pattern);
        TRuleTypeConjunction HavePropertyMemberWithName(string name);
        TRuleTypeConjunction HaveFieldMemberWithName(string name);
        TRuleTypeConjunction HaveMethodMemberWithName(string name);
        TRuleTypeConjunction HaveMemberWithName(string name);
        TRuleTypeConjunction AreNested();


        //Negations


        TRuleTypeConjunction DoNotImplementInterface(string pattern);
        TRuleTypeConjunction DoNotResideInNamespace(string pattern);
        TRuleTypeConjunction DoNotHavePropertyMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveFieldMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMethodMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMemberWithName(string name);
        TRuleTypeConjunction AreNotNested();
    }
}