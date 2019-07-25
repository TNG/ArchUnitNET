/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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