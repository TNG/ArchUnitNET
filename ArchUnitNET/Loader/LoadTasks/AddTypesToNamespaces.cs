using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddTypesToNamespaces : ILoadTask
    {
        private readonly List<IType> _types;

        public AddTypesToNamespaces(List<IType> types)
        {
            _types = types;
        }

        public void Execute()
        {
            foreach (var type in _types)
            {
                ((List<IType>)type.Namespace.Types).Add(type);
            }
        }
    }
}
