using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface ITypeInstance<out T>
        where T : IType
    {
        T Type { get; }
        IEnumerable<GenericArgument> GenericArguments { get; }
        bool IsArray { get; }
        IEnumerable<int> ArrayDimensions { get; }
    }
}
