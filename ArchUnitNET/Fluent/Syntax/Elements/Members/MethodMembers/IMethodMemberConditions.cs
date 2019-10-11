namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        TReturnType BeConstructor();
        TReturnType BeVirtual();
        TReturnType BeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType HaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);


        //Negations


        TReturnType BeNoConstructor();
        TReturnType NotBeVirtual();
        TReturnType NotBeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveDependencyInMethodBodyTo(string pattern, bool useRegularExpressions = false);
    }
}