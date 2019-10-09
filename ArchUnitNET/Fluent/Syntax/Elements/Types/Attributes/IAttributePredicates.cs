namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public interface IAttributePredicates<out TReturnType> : ITypePredicates<TReturnType>
    {
        TReturnType AreAbstract();
        TReturnType AreSealed();


        //Negations


        TReturnType AreNotAbstract();
        TReturnType AreNotSealed();
    }
}