using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public interface IAddInterfaceCondition<TNextElement, TRuleType>
        : IAddTypeCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
