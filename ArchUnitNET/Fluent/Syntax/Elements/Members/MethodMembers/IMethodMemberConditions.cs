using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions : IMemberConditions<MethodMembersShouldConjunction, MethodMember>
    {
        MethodMembersShouldConjunction BeConstructor();
        MethodMembersShouldConjunction BeVirtual();


        //Negations


        MethodMembersShouldConjunction BeNoConstructor();
        MethodMembersShouldConjunction NotBeVirtual();
    }
}