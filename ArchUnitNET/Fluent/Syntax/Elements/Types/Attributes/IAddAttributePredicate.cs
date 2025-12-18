using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAddAttributePredicate<out TNextElement, TRuleType>
        : IAddTypePredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement AreAbstract();
        TNextElement AreSealed();

        //Negations

        TNextElement AreNotAbstract();
        TNextElement AreNotSealed();
    }
}
