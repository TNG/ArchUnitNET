using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public interface IAddFieldMemberPredicate<out TNextElement, TRuleType>
        : IAddMemberPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
