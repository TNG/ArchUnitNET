/*
 * Copyright 2019 TNG Technology Consulting GmbH
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