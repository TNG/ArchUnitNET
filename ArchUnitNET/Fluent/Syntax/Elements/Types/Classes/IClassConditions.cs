namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public interface IClassConditions<out TReturnType> : ITypeConditions<TReturnType>
    {
        TReturnType BeAbstract();
        TReturnType BeSealed();
        TReturnType BeValueTypes();
        TReturnType BeEnums();
        TReturnType BeStructs();


        //Negations


        TReturnType NotBeAbstract();
        TReturnType NotBeSealed();
        TReturnType NotBeValueTypes();
        TReturnType NotBeEnums();
        TReturnType NotBeStructs();
    }
}