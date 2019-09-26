using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IObjectProvider<T> : IHasDescription where T : ICanBeAnalyzed
    {
        IEnumerable<T> GetObjects(Architecture architecture);
    }
}