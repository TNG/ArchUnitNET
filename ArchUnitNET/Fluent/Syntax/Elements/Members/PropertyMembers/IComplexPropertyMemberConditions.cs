using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IComplexPropertyMemberConditions :
        IComplexMemberConditions<PropertyMembersShouldConjunction, PropertyMember>,
        IPropertyMemberConditions<PropertyMembersShouldConjunction>
    {
    }
}