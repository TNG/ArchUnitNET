using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public interface IInterfaceConditions<out TReturnType, out TRuleType>
        : ITypeConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
