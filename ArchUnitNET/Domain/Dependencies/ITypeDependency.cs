using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public interface ITypeDependency
    {
        IType Origin { get; }
        IType Target { get; }

        IEnumerable<GenericArgument> TargetGenericArguments { get; }
        bool TargetIsArray { get; }
        IEnumerable<int> TargetArrayDimensions { get; }
    }
}
