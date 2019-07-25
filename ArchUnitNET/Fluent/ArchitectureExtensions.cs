/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public static class ArchitectureExtensions
    {
        [NotNull]
        public static IType GetTypeOfType([NotNull] this Architecture architecture, [NotNull] Type type)
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
                var foundType = architecture.Types.WhereFullNameIs(type.FullName);
                if (foundType == null)
                {
                    throw new TypeDoesNotExistInArchitecture(
                        $"Type {type.FullName} does not exist in provided architecture.");
                }

                return foundType;
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
            return architecture.Classes.WhereFullNameIs(type.FullName);
        }

        [NotNull]
        public static Interface GetInterfaceOfType([NotNull] this Architecture architecture, [NotNull] Type type)
        {
            return architecture.Interfaces.WhereFullNameIs(type.FullName);
        }
    }
}