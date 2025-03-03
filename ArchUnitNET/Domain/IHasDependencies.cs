using System.Collections.Generic;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public interface IHasDependencies
    {
        List<ITypeDependency> Dependencies { get; }
        List<ITypeDependency> BackwardsDependencies { get; }
    }
}
