using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader
{
    internal class NamespaceRegistry
    {
        private readonly Dictionary<string, Namespace> _namespaces =
            new Dictionary<string, Namespace>();

        public IEnumerable<Namespace> Namespaces => _namespaces.Values;

        public Namespace GetOrCreateNamespace(string typeNamespaceName)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(
                typeNamespaceName,
                _namespaces,
                s => new Namespace(typeNamespaceName, new List<IType>())
            );
        }
    }
}
