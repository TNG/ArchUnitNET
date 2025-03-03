using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public interface IMember : ICanBeAnalyzed
    {
        IType DeclaringType { get; }
        List<IMemberTypeDependency> MemberDependencies { get; }
        List<IMemberTypeDependency> MemberBackwardsDependencies { get; }
        bool? IsStatic { get; }
        Writability? Writability { get; }
    }
}
