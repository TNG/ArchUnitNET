using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassConditions<out TReturnType, out TRuleType>
        : ITypeConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType BeAbstract();
        TReturnType BeSealed();
        TReturnType BeImmutable();

        //Negations


        TReturnType NotBeAbstract();
        TReturnType NotBeSealed();
        TReturnType NotBeImmutable();
    }
}
