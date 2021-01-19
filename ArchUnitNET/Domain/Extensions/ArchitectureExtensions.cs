//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

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
        public static IType GetITypeOfType([NotNull] this Architecture architecture, [NotNull] Type type)
        {
            if (type.IsClass)
            {
                return architecture.GetClassOfType(type);
            }

            if (type.IsInterface)
            {
                return architecture.GetInterfaceOfType(type);
            }

            try
            {
                var foundType = AllTypes(architecture).WhereFullNameIs(type.FullName);
                if (foundType != null)
                {
                    return foundType;
                }

                throw new TypeDoesNotExistInArchitecture(
                    $"Type {type.FullName} does not exist in provided architecture.");
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Type {type.FullName} found multiple times in provided architecture. Please use extern "
                    + "alias to reference assemblies that have the same fully-qualified type names.");
            }
        }

        [NotNull]
        public static Class GetClassOfType([NotNull] this Architecture architecture, [NotNull] Type type)
        {
            try
            {
                var cls = AllClasses(architecture).WhereFullNameIs(type.FullName);
                if (cls != null)
                {
                    return cls;
                }

                throw new TypeDoesNotExistInArchitecture(
                    $"Type {type.FullName} does not exist in provided architecture or is no class.");
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Type {type.FullName} found multiple times in provided architecture. Please use extern "
                    + "alias to reference assemblies that have the same fully-qualified type names.");
            }
        }

        [NotNull]
        public static Interface GetInterfaceOfType([NotNull] this Architecture architecture, [NotNull] Type type)
        {
            try
            {
                var intf = AllInterfaces(architecture).WhereFullNameIs(type.FullName);
                if (intf != null)
                {
                    return intf;
                }

                throw new TypeDoesNotExistInArchitecture(
                    $"Type {type.FullName} does not exist in provided architecture or is no interface.");
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Type {type.FullName} found multiple times in provided architecture. Please use extern "
                    + "alias to reference assemblies that have the same fully-qualified type names.");
            }
        }

        [NotNull]
        public static Attribute GetAttributeOfType([NotNull] this Architecture architecture, [NotNull] Type type)
        {
            try
            {
                var attribute = AllAttributes(architecture).WhereFullNameIs(type.FullName);
                if (attribute != null)
                {
                    return attribute;
                }

                throw new TypeDoesNotExistInArchitecture(
                    $"Type {type.FullName} does not exist in provided architecture or is no attribute.");
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Type {type.FullName} found multiple times in provided architecture. Please use extern "
                    + "alias to reference assemblies that have the same fully-qualified type names.");
            }
        }

        [NotNull]
        public static Assembly GetAssemblyOfAssembly([NotNull] this Architecture architecture,
            [NotNull] System.Reflection.Assembly assembly)
        {
            try
            {
                var foundAssembly = architecture.Assemblies.WhereFullNameIs(assembly.FullName);
                if (foundAssembly != null)
                {
                    return foundAssembly;
                }

                throw new AssemblyDoesNotExistInArchitecture(
                    $"Assembly {assembly.FullName} does not exist in provided architecture.");
            }
            catch (MultipleOccurrencesInSequenceException)
            {
                throw new NotSupportedException(
                    $"Assembly {assembly.FullName} found multiple times in provided architecture.");
            }
        }
    }
}