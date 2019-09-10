namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassesThat<TRuleTypeConjunction> : ITypesThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreAbstract();


        //Negations


        TRuleTypeConjunction AreNotAbstract();
    }
}