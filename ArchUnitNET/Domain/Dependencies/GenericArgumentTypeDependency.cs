namespace ArchUnitNET.Domain.Dependencies
{
    public class GenericArgumentTypeDependency : TypeInstanceDependency
    {
        public GenericArgumentTypeDependency(IType origin, GenericArgument genericArgument)
            : base(origin, genericArgument) { }
    }
}
