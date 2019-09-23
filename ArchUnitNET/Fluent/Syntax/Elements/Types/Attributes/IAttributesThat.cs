namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributesThat<TRuleTypeConjunction> : ITypesThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreAbstract();


        //Negations


        TRuleTypeConjunction AreNotAbstract();
    }
}