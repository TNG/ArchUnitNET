namespace ArchUnitNET.Domain.Dependencies
{
    public class ImplementsInterfaceDependency : TypeInstanceDependency
    {
        public ImplementsInterfaceDependency(
            IType origin,
            ITypeInstance<IType> implementedInterface
        )
            : base(origin, implementedInterface) { }
    }
}
