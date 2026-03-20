using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Adds each type to its namespace's type list. Must run after all types have been
    /// fully populated so that namespace queries return complete results.
    /// </summary>
    internal static class AddTypesToNamespaces
    {
        internal static void Execute(IEnumerable<IType> types)
        {
            foreach (var type in types)
            {
                ((List<IType>)type.Namespace.Types).Add(type);
            }
        }
    }
}
