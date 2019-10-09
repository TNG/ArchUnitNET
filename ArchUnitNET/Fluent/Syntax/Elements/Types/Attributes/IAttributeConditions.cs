namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributeConditions<out TReturnType> : ITypeConditions<TReturnType>
    {
        TReturnType BeAbstract();
        TReturnType BeSealed();


        //Negations


        TReturnType NotBeAbstract();
        TReturnType NotBeSealed();
    }
}