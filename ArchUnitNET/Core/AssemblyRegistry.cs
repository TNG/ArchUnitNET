/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Core
{
    internal class AssemblyRegistry
    {
        private readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public IEnumerable<Assembly> Assemblies => _assemblies.Values;

        public Assembly GetOrCreateAssembly(string assemblyName, string assemblyFullName)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(assemblyName, _assemblies,
                s => new Assembly(assemblyName, assemblyFullName));
        }
    }
}