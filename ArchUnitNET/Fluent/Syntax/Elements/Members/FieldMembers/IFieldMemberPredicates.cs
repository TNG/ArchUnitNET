using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public interface IFieldMemberPredicates<out TRuleTypeConjunction, TRuleType>
        : IMemberPredicates<TRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed { }
}
