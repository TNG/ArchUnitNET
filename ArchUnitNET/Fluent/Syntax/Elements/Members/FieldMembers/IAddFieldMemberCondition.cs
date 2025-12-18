using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public interface IAddFieldMemberCondition<TNextElement, TRuleType>
        : IAddMemberCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
