using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddTypesToNamespace : ILoadTask
    {
        private readonly Namespace _ns;
        private readonly List<IType> _types;

        public AddTypesToNamespace(Namespace ns, List<IType> types)
        {
            _ns = ns;
            _types = types;
        }

        public void Execute()
        {
            ((List<IType>)_ns.Types).AddRange(_types.Where(type => type.Namespace.Equals(_ns)));
        }
    }
}
