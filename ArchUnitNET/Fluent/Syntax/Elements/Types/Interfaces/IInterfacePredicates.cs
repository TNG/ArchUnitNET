using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public interface IInterfacePredicates<out TReturnType, TRuleType>
        : ITypePredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
