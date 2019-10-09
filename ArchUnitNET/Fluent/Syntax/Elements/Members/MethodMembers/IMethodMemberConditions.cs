namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        TReturnType BeConstructor();
        TReturnType BeVirtual();


        //Negations


        TReturnType BeNoConstructor();
        TReturnType NotBeVirtual();
    }
}