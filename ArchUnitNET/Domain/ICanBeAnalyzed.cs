namespace ArchUnitNET.Domain
{
    public interface ICanBeAnalyzed
        : IHasAssemblyQualifiedName,
            IResidesInNamespace,
            IHasDependencies,
            IHasAttributes,
            IHasVisibility,
            ICanBeGeneric,
            ICanBeCompilerGenerated { }
}
