using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain.Extensions
{
    public static class ArchitectureExtensions
    {
        public static IEnumerable<IType> AllTypes(Architecture architecture)
        {
            return architecture.Types.Concat(architecture.ReferencedTypes);
        }

        public static IEnumerable<Class> AllClasses(Architecture architecture)
        {
            return architecture.Classes.Concat(architecture.ReferencedClasses);
        }

        public static IEnumerable<Interface> AllInterfaces(Architecture architecture)
        {
            return architecture.Interfaces.Concat(architecture.ReferencedInterfaces);
        }

        public static IEnumerable<Attribute> AllAttributes(Architecture architecture)
        {
            return architecture.Attributes.Concat(architecture.ReferencedAttributes);
        }

        [NotNull]
        public static IType GetITypeOfType(
            [NotNull] this Architecture architecture,
            [NotNull] Type type
        )
        {
            if (type.IsClass)
            {
                return architecture.GetClassOfType(type);
            }

            if (type.IsInterface)
            {
                return architecture.GetInterfaceOfType(type);
            }

            var foundType = AllTypes(architecture)
                .WhereAssemblyQualifiedNameIs(type.AssemblyQualifiedName);
            if (foundType != null)
            {
                return foundType;
            }

            throw new TypeDoesNotExistInArchitecture(
                $"Type {type.FullName} does not exist in provided architecture."
            );
        }

        [NotNull]
        public static Class GetClassOfType(
            [NotNull] this Architecture architecture,
            [NotNull] Type type
        )
        {
            var cls = AllClasses(architecture)
                .WhereAssemblyQualifiedNameIs(type.AssemblyQualifiedName);
            if (cls != null)
            {
                return cls;
            }

            throw new TypeDoesNotExistInArchitecture(
                $"Type {type.FullName} does not exist in provided architecture or is no class."
            );
        }

        [NotNull]
        public static Interface GetInterfaceOfType(
            [NotNull] this Architecture architecture,
            [NotNull] Type type
        )
        {
            var intf = AllInterfaces(architecture)
                .WhereAssemblyQualifiedNameIs(type.AssemblyQualifiedName);
            if (intf != null)
            {
                return intf;
            }

            throw new TypeDoesNotExistInArchitecture(
                $"Type {type.FullName} does not exist in provided architecture or is no interface."
            );
        }

        [NotNull]
        public static Attribute GetAttributeOfType(
            [NotNull] this Architecture architecture,
            [NotNull] Type type
        )
        {
            var attribute = AllAttributes(architecture)
                .WhereAssemblyQualifiedNameIs(type.AssemblyQualifiedName);
            if (attribute != null)
            {
                return attribute;
            }

            throw new TypeDoesNotExistInArchitecture(
                $"Type {type.FullName} does not exist in provided architecture or is no attribute."
            );
        }

        [NotNull]
        public static Assembly GetAssemblyOfAssembly(
            [NotNull] this Architecture architecture,
            [NotNull] System.Reflection.Assembly assembly
        )
        {
            try
            {
                var foundAssembly = architecture.Assemblies.WhereFullNameIs(assembly.FullName);
                if (foundAssembly != null)
                {
                    return foundAssembly;
                }

                throw new AssemblyDoesNotExistInArchitecture(
                    $"Assembly {assembly.FullName} does not exist in provided architecture."
                );
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Assembly {assembly.FullName} found multiple times in provided architecture."
                );
            }
        }
    }
}
