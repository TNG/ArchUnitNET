namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberPredicates<TRuleTypeConjunction> : IMemberPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreConstructors();
        TRuleTypeConjunction AreVirtual();


        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
    }
}