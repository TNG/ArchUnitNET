using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IObjectProvider<out T> : IHasDescription
    {
        IEnumerable<T> GetObjects(Architecture architecture);

        string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        );
    }
}
