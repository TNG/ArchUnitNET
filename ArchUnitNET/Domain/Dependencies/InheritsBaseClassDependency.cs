namespace ArchUnitNET.Domain.Dependencies
{
    public class InheritsBaseClassDependency : TypeInstanceDependency
    {
        public InheritsBaseClassDependency(IType origin, ITypeInstance<IType> targetInstance)
            : base(origin, targetInstance) { }
    }
}
