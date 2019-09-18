namespace ArchUnitNET.Fluent
{
    public interface IHasFailDescription : IHasDescription
    {
        string FailDescription { get; }
    }
}