namespace ArchUnitNET.Domain.Dependencies
{
    public class GenericArgumentMemberDependency : MemberTypeInstanceDependency
    {
        public GenericArgumentMemberDependency(
            IMember originMember,
            GenericArgument genericArgument
        )
            : base(originMember, genericArgument) { }
    }
}
