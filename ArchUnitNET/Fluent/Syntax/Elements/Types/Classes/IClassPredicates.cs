using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassPredicates<out TReturnType, TRuleType>
        : ITypePredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();
        TReturnType AreImmutable();

        //Negations


        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
        TReturnType AreNotImmutable();
    }
}
