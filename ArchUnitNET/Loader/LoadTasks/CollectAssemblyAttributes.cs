using System.Linq;
using ArchUnitNET.Domain;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class CollectAssemblyAttributes : ILoadTask
    {
        private readonly Assembly _assembly;
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly DomainResolver _domainResolver;

        public CollectAssemblyAttributes(
            Assembly assembly,
            AssemblyDefinition assemblyDefinition,
            DomainResolver domainResolver
        )
        {
            _assembly = assembly;
            _assemblyDefinition = assemblyDefinition;
            _domainResolver = domainResolver;
        }

        public void Execute()
        {
            var attributeInstances = _assemblyDefinition
                .CustomAttributes.Select(attr =>
                    attr.CreateAttributeFromCustomAttribute(_domainResolver)
                )
                .ToList();
            _assembly.AttributeInstances.AddRange(attributeInstances);
        }
    }
}
