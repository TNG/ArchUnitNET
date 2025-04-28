using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributePredicates<out TReturnType, TRuleType>
        : ITypePredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();

        //Negations

        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
    }
}
