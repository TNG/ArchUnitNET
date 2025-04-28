using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// An interface for object providers that can provide a size independent of the used architecture.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISizedObjectProvider<out T> : IObjectProvider<T>
    {
        int Count { get; }
    }
}
