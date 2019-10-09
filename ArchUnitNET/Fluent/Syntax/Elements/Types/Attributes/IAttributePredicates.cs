namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributePredicates<TRuleTypeConjunction> : ITypePredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreAbstract();
        TRuleTypeConjunction AreSealed();


        //Negations


        TRuleTypeConjunction AreNotAbstract();
        TRuleTypeConjunction AreNotSealed();
    }
}