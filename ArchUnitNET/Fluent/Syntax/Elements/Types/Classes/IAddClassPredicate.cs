using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IAddClassPredicate<out TNextElement, TRuleType>
        : IAddTypePredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement AreAbstract();
        TNextElement AreSealed();
        TNextElement AreRecord();
        TNextElement AreImmutable();

        //Negations

        TNextElement AreNotAbstract();
        TNextElement AreNotSealed();
        TNextElement AreNotRecord();
        TNextElement AreNotImmutable();
    }
}
