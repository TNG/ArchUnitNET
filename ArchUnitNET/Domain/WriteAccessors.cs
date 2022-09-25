using System;

namespace ArchUnitNET.Domain
{
    [Flags]
    public enum WriteAccessors
    {
        None = 0,
        ReadOnly = 1,
        Init = 1 << 1,
        Set = 1 << 2,
        Immutable = None | ReadOnly | Init
    }
}
