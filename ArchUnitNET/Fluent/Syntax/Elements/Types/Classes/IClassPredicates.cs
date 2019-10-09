namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassPredicates<out TReturnType> : ITypePredicates<TReturnType>
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();
        TReturnType AreValueTypes();
        TReturnType AreEnums();
        TReturnType AreStructs();


        //Negations


        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
        TReturnType AreNotValueTypes();
        TReturnType AreNotEnums();
        TReturnType AreNotStructs();
    }
}