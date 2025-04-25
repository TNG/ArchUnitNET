namespace ArchUnitNET.Domain.Dependencies
{
    public class TypeCheckDependency : MemberTypeInstanceDependency
    {
        public TypeCheckDependency(IMember originMember, ITypeInstance<IType> typeCheckTypeInstance)
            : base(originMember, typeCheckTypeInstance) { }
    }
}
