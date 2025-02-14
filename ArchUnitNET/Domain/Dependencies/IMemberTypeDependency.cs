namespace ArchUnitNET.Domain.Dependencies
{
    public interface IMemberTypeDependency : ITypeDependency
    {
        IMember OriginMember { get; }
    }
}
