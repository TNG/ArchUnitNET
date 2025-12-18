using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public interface IAddInterfacePredicate<out TNextElement, TRuleType>
        : IAddTypePredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
