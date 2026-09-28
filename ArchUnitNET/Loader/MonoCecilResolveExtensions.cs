using System;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    /// <summary>
    /// Resolve extensions that return <c>null</c> instead of throwing
    /// <see cref="AssemblyResolutionException" /> when an assembly cannot be located.
    /// </summary>
    internal static class MonoCecilResolveExtensions
    {
        [CanBeNull]
        public static TypeDefinition TryResolve([CanBeNull] this TypeReference typeReference)
        {
            if (typeReference == null)
            {
                return null;
            }

            try
            {
                return typeReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                return null;
            }
        }

        [CanBeNull]
        public static MethodDefinition TryResolve([CanBeNull] this MethodReference methodReference)
        {
            if (methodReference == null)
            {
                return null;
            }

            try
            {
                return methodReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                return null;
            }
        }

        [CanBeNull]
        public static FieldDefinition TryResolve([CanBeNull] this FieldReference fieldReference)
        {
            if (fieldReference == null)
            {
                return null;
            }

            try
            {
                return fieldReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                return null;
            }
        }

        [CanBeNull]
        public static IMemberDefinition TryResolve([CanBeNull] this MemberReference memberReference)
        {
            if (memberReference == null)
            {
                return null;
            }

            try
            {
                return memberReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                return null;
            }
        }
    }
}
