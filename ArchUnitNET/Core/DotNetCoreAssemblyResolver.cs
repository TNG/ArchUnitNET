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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Fluent;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    internal class DotNetCoreAssemblyResolver : IAssemblyResolver
    {
        private readonly Dictionary<string, AssemblyDefinition> _libraries;
        public string AssemblyPath = "";

        public DotNetCoreAssemblyResolver()
        {
            _libraries = new Dictionary<string, AssemblyDefinition>();
        }

        public void AddLib(AssemblyDefinition moduleAssembly)
        {
            if (!_libraries.ContainsKey(moduleAssembly.FullName))
            {
                _libraries.Add(moduleAssembly.FullName, moduleAssembly);
            }
        }

        [CanBeNull]
        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return Resolve(name, new ReaderParameters {AssemblyResolver = this});
        }

        [CanBeNull]
        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_libraries.TryGetValue(name.Name, out var assemblyDefinition) || string.IsNullOrEmpty(AssemblyPath))
            {
                return assemblyDefinition;
            }

            var file = Directory.EnumerateFiles(AssemblyPath, $"{name.Name}.dll", SearchOption.AllDirectories)
                .FirstOrDefault();

            if (file == null)
            {
                return null;
            }

            assemblyDefinition = AssemblyDefinition.ReadAssembly(file, parameters);
            _libraries.Add(name.Name, assemblyDefinition);

            return assemblyDefinition;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _libraries.Values.ForEach(def => def.Dispose());
        }
    }
}