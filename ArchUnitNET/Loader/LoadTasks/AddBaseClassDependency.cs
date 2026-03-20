using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// If the type has a base class, creates an <see cref="InheritsBaseClassDependency"/>
    /// and adds it to the type's dependency list. Skipped for interfaces and types
    /// whose base is not a <see cref="Class"/>.
    /// </summary>
    internal static class AddBaseClassDependency
    {
        internal static void Execute(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            var baseTypeRef = typeDefinition.BaseType;
            if (baseTypeRef == null)
            {
                return;
            }

            var baseType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(baseTypeRef);
            if (!(baseType.Type is Class baseClass))
            {
                return;
            }

            var dependency = new InheritsBaseClassDependency(
                type,
                new TypeInstance<Class>(
                    baseClass,
                    baseType.GenericArguments,
                    baseType.ArrayDimensions
                )
            );
            type.Dependencies.Add(dependency);
        }
    }
}
