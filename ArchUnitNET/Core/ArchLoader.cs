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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    public class ArchLoader
    {
        private readonly ArchBuilder _archBuilder = new ArchBuilder();
        private DotNetCoreAssemblyResolver _assemblyResolver = new DotNetCoreAssemblyResolver();

        public Architecture Build()
        {
            var architecture = _archBuilder.Build();
            _assemblyResolver.Dispose();
            _assemblyResolver = new DotNetCoreAssemblyResolver();

            return architecture;
        }

        public ArchLoader LoadAssemblies(params System.Reflection.Assembly[] assemblies)
        {
            var assemblySet = new HashSet<System.Reflection.Assembly>(assemblies);
            assemblySet.ForEach(assembly => LoadAssembly(assembly.Location));
            return this;
        }

        public ArchLoader LoadFilteredDirectory(string directory, string filter)
        {
            _assemblyResolver.AssemblyPath = directory;
            var assemblies = Directory.GetFiles(directory, filter);

            var result = this;
            return assemblies.Aggregate(result, (current, assembly) => current.LoadAssembly(assembly));
        }

        public ArchLoader LoadNamespacesWithinAssembly(System.Reflection.Assembly assembly, params string[] namespc)
        {
            var nameSpaces = new HashSet<string>(namespc);
            nameSpaces.ForEach(nameSpace => { LoadModule(assembly.Location, nameSpace); });
            return this;
        }

        public ArchLoader LoadAssembly(System.Reflection.Assembly assembly)
        {
            return LoadAssembly(assembly.Location);
        }

        private ArchLoader LoadAssembly(string fileName)
        {
            LoadModule(fileName, null);

            return this;
        }

        private void LoadModule(string fileName, string nameSpace)
        {
            try
            {
                var module = ModuleDefinition.ReadModule(fileName,
                    new ReaderParameters {AssemblyResolver = _assemblyResolver});
                _assemblyResolver.AddLib(module.Assembly);

                _archBuilder.LoadTypesForModule(module, nameSpace);
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }
        }
    }
}