using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface ICanBeGeneric
    {
        bool IsGeneric { get; }
        List<GenericParameter> GenericParameters { get; }
    }
}
