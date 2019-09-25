/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using Mono.Cecil;
using Assembly = System.Reflection.Assembly;

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

        public ArchLoader LoadAssemblies(params Assembly[] assemblies)
        {
            var assemblySet = new HashSet<Assembly>(assemblies);
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

        public ArchLoader LoadNamespacesWithinAssembly(Assembly assembly, params string[] namespc)
        {
            var nameSpaces = new HashSet<string>(namespc);
            nameSpaces.ForEach(nameSpace => { LoadModule(assembly.Location, nameSpace); });
            return this;
        }

        public ArchLoader LoadAssembly(Assembly assembly)
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
                _archBuilder.AddAssembly(module.Assembly, false);
                foreach (var reference in module.AssemblyReferences)
                {
                    _assemblyResolver.AddLib(reference);
                }

                _archBuilder.LoadTypesForModule(module, nameSpace);
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }
        }
    }
}