namespace ArchUnitNET.Domain.Dependencies
{
    public class CastTypeDependency : MemberTypeInstanceDependency
    {
        public CastTypeDependency(IMember originMember, ITypeInstance<IType> castTypeInstance)
            : base(originMember, castTypeInstance) { }
    }
}
