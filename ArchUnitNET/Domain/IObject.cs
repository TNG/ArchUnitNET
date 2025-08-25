namespace ArchUnitNET.Domain
{
    internal interface IObject<out T> : IHasDescription
    {
        T Get(Architecture architecture);
    }
}
