namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberPredicates<out TRuleTypeConjunction> : IMemberPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreConstructors();
        TRuleTypeConjunction AreVirtual();
        TRuleTypeConjunction AreCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction HaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);


        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
        TRuleTypeConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false);
        TRuleTypeConjunction DoNotHaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
    }
}