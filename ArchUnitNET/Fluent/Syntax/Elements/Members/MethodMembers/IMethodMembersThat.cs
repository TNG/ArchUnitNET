namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMembersThat<TRuleTypeConjunction> : IMembersThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreConstructors();
        TRuleTypeConjunction AreVirtual();


        //Negations


        TRuleTypeConjunction AreNoConstructors();
        TRuleTypeConjunction AreNotVirtual();
    }
}