using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IComplexMethodMemberConditions
        : IComplexMemberConditions<MethodMembersShouldConjunction, MethodMember>,
            IMethodMemberConditions<MethodMembersShouldConjunction, MethodMember> { }
}
