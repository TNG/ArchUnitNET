using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers
{
    public interface IComplexFieldMemberConditions
        : IComplexMemberConditions<FieldMembersShouldConjunction, FieldMember>,
            IFieldMemberConditions<FieldMembersShouldConjunction, FieldMember> { }
}
