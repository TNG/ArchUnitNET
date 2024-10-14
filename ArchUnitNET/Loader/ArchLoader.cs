//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using static System.IO.SearchOption;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Loader
{
    public class ArchLoader
    {
        private readonly ArchBuilder _archBuilder = new ArchBuilder();

        public Architecture Build()
        {
            var architecture = _archBuilder.Build();
            return architecture;
        }

        public ArchLoader LoadAssemblies(params Assembly[] assemblies)
        {
            assemblies.ForEach(assembly => LoadAssembly(assembly));
            return this;
        }

        public ArchLoader LoadAssembliesIncludingDependencies(params Assembly[] assemblies)
        {
            return LoadAssembliesIncludingDependencies(assemblies, false);
        }

        public ArchLoader LoadAssembliesIncludingDependencies(
            IEnumerable<Assembly> assemblies,
            bool recursive
        )
        {
            assemblies.ForEach(assembly => LoadAssemblyIncludingDependencies(assembly, recursive));
            return this;
        }

        // public ArchLoader LoadFilteredDirectory(
        //     string directory,
        //     string filter,
        //     SearchOption searchOption = TopDirectoryOnly
        // )
        // {
        //     var path = Path.GetFullPath(directory);
        //     _assemblyResolver.AssemblyPath = path;
        //     var assemblies = Directory.GetFiles(path, filter, searchOption);
        //
        //     var result = this;
        //     return assemblies.Aggregate(
        //         result,
        //         (current, assembly) => current.LoadAssembly(assembly, false, false)
        //     );
        // }
        //
        // public ArchLoader LoadFilteredDirectoryIncludingDependencies(
        //     string directory,
        //     string filter,
        //     bool recursive = false,
        //     SearchOption searchOption = TopDirectoryOnly
        // )
        // {
        //     var path = Path.GetFullPath(directory);
        //     _assemblyResolver.AssemblyPath = path;
        //     var assemblies = Directory.GetFiles(path, filter, searchOption);
        //
        //     var result = this;
        //     return assemblies.Aggregate(
        //         result,
        //         (current, assembly) => current.LoadAssembly(assembly, true, recursive)
        //     );
        // }

        public ArchLoader LoadNamespacesWithinAssembly(Assembly assembly, params string[] namespc)
        {
            namespc.ForEach(nameSpace =>
            {
                LoadAssembly(assembly, nameSpace, false, false);
            });
            return this;
        }

        public ArchLoader LoadAssembly(Assembly assembly)
        {
            LoadAssembly(assembly, null, false, false);
            return this;
        }

        public ArchLoader LoadAssemblyIncludingDependencies(
            Assembly assembly,
            bool recursive = false
        )
        {
            LoadAssembly(assembly, null, true, recursive);
            return this;
        }

        private ArchLoader LoadAssembly(string fileName, bool includeDependencies, bool recursive)
        {
            LoadAssembly(Assembly.LoadFile(fileName), null, includeDependencies, recursive);
            return this;
        }

        private void LoadAssembly(
            Assembly assembly,
            string nameSpace,
            bool includeDependencies,
            bool recursive,
            FilterFunc filterFunc = null
        )
        {
            var processedAssemblies = new List<AssemblyName> { assembly.GetName() };
            var resolvedModules = new List<Module>(assembly.Modules);
            var references = assembly.GetReferencedAssemblies();
            _archBuilder.AddAssembly(assembly, false, references);
            foreach (var assemblyReference in references)
            {
                if (includeDependencies && recursive)
                {
                    AddReferencedAssembliesRecursively(
                        assemblyReference,
                        processedAssemblies,
                        resolvedModules,
                        filterFunc
                    );
                }
                else
                {
                    processedAssemblies.Add(assemblyReference);
                    if (includeDependencies)
                    {
                        Assembly loadedAssembly = null;
                        try
                        {
                            loadedAssembly = Assembly.Load(assemblyReference);
                        }
                        catch (FileNotFoundException)
                        {
                            throw new ArchLoaderException(
                                $"Assembly {assemblyReference.Name} not found"
                            );
                        }
                        _archBuilder.AddAssembly(loadedAssembly, false, null);
                        resolvedModules.AddRange(loadedAssembly.Modules);
                    }
                }
            }

            foreach (var module in resolvedModules)
            {
                _archBuilder.LoadTypesForModule(module, null);
            }
        }

        private void AddReferencedAssembliesRecursively(
            AssemblyName currentAssemblyReference,
            ICollection<AssemblyName> processedAssemblies,
            List<Module> resolvedModules,
            FilterFunc filterFunc
        )
        {
            if (processedAssemblies.Contains(currentAssemblyReference))
            {
                return;
            }

            processedAssemblies.Add(currentAssemblyReference);

            var currentAssembly = Assembly.Load(currentAssemblyReference);

            var filterResult = filterFunc?.Invoke(currentAssembly);
            if (filterResult?.LoadThisAssembly != false)
            {
                _archBuilder.AddAssembly(currentAssembly, false, null);
                resolvedModules.AddRange(currentAssembly.Modules);
            }

            foreach (var reference in currentAssembly.GetReferencedAssemblies())
            {
                if (filterResult?.TraverseDependencies != false)
                {
                    AddReferencedAssembliesRecursively(
                        reference,
                        processedAssemblies,
                        resolvedModules,
                        filterFunc
                    );
                }
            }
        }

        /// <summary>
        /// Loads assemblies from dependency tree with user-defined filtration logic
        /// </summary>
        /// <param name="assemblies">Assemblies to start traversal from</param>
        /// <param name="filterFunc">Delegate to control loading and traversal logic</param>
        /// <returns></returns>
        public ArchLoader LoadAssembliesRecursively(
            IEnumerable<Assembly> assemblies,
            FilterFunc filterFunc
        )
        {
            foreach (var assembly in assemblies)
            {
                LoadAssembly(assembly, null, true, true, filterFunc);
            }
            return this;
        }
    }
}
