using System.Linq;
using ArchUnitNET.Domain;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Creates attribute instances from the assembly-level custom attributes of the
    /// given <see cref="AssemblyDefinition"/> and adds them to the domain
    /// <see cref="Assembly"/>.
    /// </summary>
    internal static class CollectAssemblyAttributes
    {
        internal static void Execute(
            Assembly assembly,
            AssemblyDefinition assemblyDefinition,
            DomainResolver domainResolver
        )
        {
            var attributeInstances = assemblyDefinition
                .CustomAttributes.Select(attr =>
                    attr.CreateAttributeFromCustomAttribute(domainResolver)
                )
                .ToList();
            assembly.AttributeInstances.AddRange(attributeInstances);
        }
    }
}
