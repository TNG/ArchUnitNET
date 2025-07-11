using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader
{
    internal class AssemblyRegistry
    {
        private readonly Dictionary<string, Assembly> _assemblies =
            new Dictionary<string, Assembly>();

        public IEnumerable<Assembly> Assemblies => _assemblies.Values;

        public Assembly GetOrCreateAssembly(
            string assemblyName,
            string assemblyFullName,
            bool isOnlyReferenced,
            List<string> assemblyReferences
        )
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(
                assemblyFullName,
                _assemblies,
                s => new Assembly(
                    assemblyName,
                    assemblyFullName,
                    isOnlyReferenced,
                    assemblyReferences
                )
            );
        }

        public bool ContainsAssembly(string assemblyName)
        {
            return _assemblies.ContainsKey(assemblyName);
        }
    }
}
