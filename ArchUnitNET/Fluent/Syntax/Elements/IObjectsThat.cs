namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction DependOn(string pattern);
        TRuleTypeConjunction HaveName(string name);
        TRuleTypeConjunction HaveFullName(string fullname);
        TRuleTypeConjunction HaveNameStartingWith(string pattern);
        TRuleTypeConjunction HaveNameEndingWith(string pattern);
        TRuleTypeConjunction HaveNameContaining(string pattern);
        TRuleTypeConjunction ArePrivate();
        TRuleTypeConjunction ArePublic();
        TRuleTypeConjunction AreProtected();
        TRuleTypeConjunction AreInternal();
        TRuleTypeConjunction AreProtectedInternal();
        TRuleTypeConjunction ArePrivateProtected();


        //Negations


        TRuleTypeConjunction DoNotDependOn(string pattern);
        TRuleTypeConjunction DoNotHaveName(string name);
        TRuleTypeConjunction DoNotHaveFullName(string fullname);
        TRuleTypeConjunction DoNotHaveNameStartingWith(string pattern);
        TRuleTypeConjunction DoNotHaveNameEndingWith(string pattern);
        TRuleTypeConjunction DoNotHaveNameContaining(string pattern);
        TRuleTypeConjunction AreNotPrivate();
        TRuleTypeConjunction AreNotPublic();
        TRuleTypeConjunction AreNotProtected();
        TRuleTypeConjunction AreNotInternal();
        TRuleTypeConjunction AreNotProtectedInternal();
        TRuleTypeConjunction AreNotPrivateProtected();
    }
}