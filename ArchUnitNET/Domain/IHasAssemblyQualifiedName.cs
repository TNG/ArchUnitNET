namespace ArchUnitNET.Domain
{
    public interface IHasAssemblyQualifiedName : IHasName, IResidesInAssembly
    {
        string AssemblyQualifiedName { get; }
    }
}
