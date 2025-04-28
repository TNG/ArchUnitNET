using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributeConditions<out TReturnType, out TRuleType>
        : ITypeConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType BeAbstract();
        TReturnType BeSealed();

        //Negations

        TReturnType NotBeAbstract();
        TReturnType NotBeSealed();
    }
}
