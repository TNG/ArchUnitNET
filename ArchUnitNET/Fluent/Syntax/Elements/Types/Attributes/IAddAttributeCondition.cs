using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAddAttributeCondition<TNextElement, TRuleType>
        : IAddTypeCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement BeAbstract();
        TNextElement BeSealed();

        //Negations

        TNextElement NotBeAbstract();
        TNextElement NotBeSealed();
    }
}
