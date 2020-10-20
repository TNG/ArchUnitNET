//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Mono.Cecil;
using static System.IO.SearchOption;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Loader
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
            assemblySet.ForEach(assembly => LoadAssembly(assembly));
            return this;
        }

        public ArchLoader LoadAssembliesIncludingDependencies(params Assembly[] assemblies)
        {
            var assemblySet = new HashSet<Assembly>(assemblies);
            assemblySet.ForEach(assembly => LoadAssemblyIncludingDependencies(assembly));
            return this;
        }

        public ArchLoader LoadFilteredDirectory(string directory, string filter,
            SearchOption searchOption = TopDirectoryOnly)
        {
            var path = Path.GetFullPath(directory);
            _assemblyResolver.AssemblyPath = path;
            var assemblies = Directory.GetFiles(path, filter, searchOption);

            var result = this;
            return assemblies.Aggregate(result,
                (current, assembly) => current.LoadAssembly(assembly, false));
        }

        public ArchLoader LoadFilteredDirectoryIncludingDependencies(string directory, string filter,
            SearchOption searchOption = TopDirectoryOnly)
        {
            var path = Path.GetFullPath(directory);
            _assemblyResolver.AssemblyPath = path;
            var assemblies = Directory.GetFiles(path, filter, searchOption);

            var result = this;
            return assemblies.Aggregate(result,
                (current, assembly) => current.LoadAssembly(assembly, true));
        }

        public ArchLoader LoadNamespacesWithinAssembly(Assembly assembly, params string[] namespc)
        {
            var nameSpaces = new HashSet<string>(namespc);
            nameSpaces.ForEach(nameSpace => { LoadModule(assembly.Location, nameSpace, false); });
            return this;
        }

        public ArchLoader LoadAssembly(Assembly assembly)
        {
            return LoadAssembly(assembly.Location, false);
        }

        public ArchLoader LoadAssemblyIncludingDependencies(Assembly assembly)
        {
            return LoadAssembly(assembly.Location, true);
        }

        private ArchLoader LoadAssembly(string fileName, bool includeDependencies)
        {
            LoadModule(fileName, null, includeDependencies);

            return this;
        }

        private void LoadModule(string fileName, string nameSpace, bool includeDependencies)
        {
            try
            {
                var module = ModuleDefinition.ReadModule(fileName,
                    new ReaderParameters {AssemblyResolver = _assemblyResolver});
                _assemblyResolver.AddLib(module.Assembly);
                _archBuilder.AddAssembly(module.Assembly, false);
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    try
                    {
                        _assemblyResolver.AddLib(assemblyReference);
                        if (includeDependencies)
                        {
                            _archBuilder.AddAssembly(
                                _assemblyResolver.Resolve(assemblyReference) ??
                                throw new AssemblyResolutionException(assemblyReference), false);
                        }
                    }
                    catch (AssemblyResolutionException)
                    {
                        //Failed to resolve assembly, skip it
                    }
                }

                _archBuilder.LoadTypesForModule(module, nameSpace);
                if (includeDependencies)
                {
                    foreach (var moduleDefinition in module.AssemblyReferences.SelectMany(reference =>
                        _assemblyResolver.Resolve(reference)?.Modules))
                    {
                        _archBuilder.LoadTypesForModule(moduleDefinition, null);
                    }
                }
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }
        }
    }
}