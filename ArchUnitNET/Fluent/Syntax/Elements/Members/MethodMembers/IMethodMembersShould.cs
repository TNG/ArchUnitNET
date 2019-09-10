using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMembersShould : IMembersShould<MethodMembersShouldConjunction, MethodMember>
    {
        MethodMembersShouldConjunction BeConstructor();
        MethodMembersShouldConjunction BeVirtual();


        //Negations


        MethodMembersShouldConjunction BeNoConstructor();
        MethodMembersShouldConjunction NotBeVirtual();
    }
}