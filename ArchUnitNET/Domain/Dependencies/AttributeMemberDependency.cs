namespace ArchUnitNET.Domain.Dependencies
{
    public class AttributeMemberDependency : MemberTypeInstanceDependency
    {
        public AttributeMemberDependency(IMember member, AttributeInstance attributeInstance)
            : base(member, attributeInstance) { }
    }
}
