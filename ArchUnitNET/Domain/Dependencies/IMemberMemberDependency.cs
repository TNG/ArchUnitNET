using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public interface IMemberMemberDependency : IMemberTypeDependency
    {
        IMember TargetMember { get; }
        IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }
    }
}
