using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public interface IFieldMemberConditions<out TReturnType, out TRuleType>
        : IMemberConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
