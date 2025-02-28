namespace ArchUnitNET.Domain.Dependencies
{
    public class PropertyTypeDependency : MemberTypeInstanceDependency
    {
        public PropertyTypeDependency(PropertyMember property)
            : base(property, property) { }
    }
}
