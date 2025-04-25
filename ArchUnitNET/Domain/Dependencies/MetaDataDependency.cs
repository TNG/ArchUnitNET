namespace ArchUnitNET.Domain.Dependencies
{
    public class MetaDataDependency : MemberTypeInstanceDependency
    {
        public MetaDataDependency(IMember originMember, ITypeInstance<IType> metaDataTypeInstance)
            : base(originMember, metaDataTypeInstance) { }
    }
}
