namespace ArchUnitNET.Domain.Dependencies
{
    public class FieldTypeDependency : MemberTypeInstanceDependency
    {
        public FieldTypeDependency(FieldMember field)
            : base(field, field) { }
    }
}
