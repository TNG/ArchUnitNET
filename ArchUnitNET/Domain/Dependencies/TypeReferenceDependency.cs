namespace ArchUnitNET.Domain.Dependencies
{
    public class TypeReferenceDependency : TypeInstanceDependency
    {
        public TypeReferenceDependency(IType origin, ITypeInstance<IType> targetInstance)
            : base(origin, targetInstance) { }
    }
}
