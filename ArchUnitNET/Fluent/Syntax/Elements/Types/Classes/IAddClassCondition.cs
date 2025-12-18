using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IAddClassCondition<TNextElement, TRuleType>
        : IAddTypeCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement BeAbstract();
        TNextElement BeSealed();
        TNextElement BeImmutable();

        //Negations

        TNextElement NotBeAbstract();
        TNextElement NotBeSealed();
        TNextElement NotBeImmutable();
    }
}
