using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Creates <see cref="ImplementsInterfaceDependency"/> for each interface the type
    /// implements (including inherited interfaces), then rolls up all member-level
    /// dependencies to the type's dependency list.
    /// </summary>
    internal static class AddClassDependencies
    {
        internal static void Execute(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            // Interface dependencies
            GetInterfacesImplementedByClass(typeDefinition)
                .ForEach(target =>
                {
                    var targetType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                        target
                    );
                    type.Dependencies.Add(new ImplementsInterfaceDependency(type, targetType));
                });

            // Member dependencies rolled up to type level
            type.Members.ForEach(member =>
            {
                type.Dependencies.AddRange(member.Dependencies);
            });
        }

        private static IEnumerable<TypeReference> GetInterfacesImplementedByClass(
            TypeDefinition typeDefinition
        )
        {
            var baseType = typeDefinition.BaseType?.Resolve();
            var baseInterfaces =
                baseType != null
                    ? GetInterfacesImplementedByClass(baseType)
                    : new List<TypeReference>();

            return typeDefinition
                .Interfaces.Select(implementation => implementation.InterfaceType)
                .Concat(baseInterfaces);
        }
    }
}
