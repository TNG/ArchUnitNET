namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassPredicates<TRuleTypeConjunction> : ITypePredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreAbstract();
        TRuleTypeConjunction AreSealed();
        TRuleTypeConjunction AreValueTypes();
        TRuleTypeConjunction AreEnums();
        TRuleTypeConjunction AreStructs();


        //Negations


        TRuleTypeConjunction AreNotAbstract();
        TRuleTypeConjunction AreNotSealed();
        TRuleTypeConjunction AreNotValueTypes();
        TRuleTypeConjunction AreNotEnums();
        TRuleTypeConjunction AreNotStructs();
    }
}