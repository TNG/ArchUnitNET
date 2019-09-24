namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributesThat<TRuleTypeConjunction> : ITypesThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreAbstract();
        TRuleTypeConjunction AreSealed();


        //Negations


        TRuleTypeConjunction AreNotAbstract();
        TRuleTypeConjunction AreNotSealed();
    }
}