using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public interface IMember : ICanBeAnalyzed
    {
        IType DeclaringType { get; }
        bool? IsStatic { get; }
        Writability? Writability { get; }
    }
}
