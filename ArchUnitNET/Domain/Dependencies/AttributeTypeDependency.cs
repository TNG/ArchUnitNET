namespace ArchUnitNET.Domain.Dependencies
{
    public class AttributeTypeDependency : TypeInstanceDependency
    {
        public AttributeTypeDependency(IType origin, AttributeInstance attributeInstance)
            : base(origin, attributeInstance) { }
    }
}
