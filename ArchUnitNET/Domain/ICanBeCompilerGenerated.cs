namespace ArchUnitNET.Domain
{
    public interface ICanBeCompilerGenerated
    {
        bool IsCompilerGenerated { get; }
    }
}
