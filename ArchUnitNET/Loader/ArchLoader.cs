using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Loader
{
    public class ArchLoader
    {
        private readonly ArchBuilder _archBuilder = new ArchBuilder();
        private DotNetCoreAssemblyResolver _assemblyResolver = new DotNetCoreAssemblyResolver();
        private ArchLoaderCacheConfig _cacheConfig = new ArchLoaderCacheConfig();
        private readonly List<string> _loadedAssemblyPaths = new List<string>();
        public Architecture Build()
        {
            var architecture = _archBuilder.Build(_loadedAssemblyPaths, _cacheConfig);
            _assemblyResolver.Dispose();
            _assemblyResolver = new DotNetCoreAssemblyResolver();

            return architecture;
        }

        /// <summary>
        /// Configure caching behavior for this ArchLoader instance
        /// </summary>
        /// <param name="config">Cache configuration</param>
        /// <returns>This ArchLoader instance for fluent API</returns>
        public ArchLoader WithCacheConfig(ArchLoaderCacheConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            _cacheConfig = config.Clone();
            return this;
        }

        /// <summary>
        /// Disable caching for this ArchLoader instance
        /// </summary>
        /// <returns>This ArchLoader instance for fluent API</returns>
        public ArchLoader WithoutCaching()
        {
            _cacheConfig = new ArchLoaderCacheConfig { CachingEnabled = false };
            return this;
        }

        /// <summary>
        /// Set a user-defined cache key for fine-grained cache control
        /// </summary>
        /// <param name="userCacheKey">Custom cache key</param>
        /// <returns>This ArchLoader instance for fluent API</returns>
        public ArchLoader WithUserCacheKey(string userCacheKey)
        {
            _cacheConfig.UserCacheKey = userCacheKey;
            return this;
        }

        public ArchLoader LoadAssemblies(params Assembly[] assemblies)
        {
            var assemblySet = new HashSet<Assembly>(assemblies);
            assemblySet.ForEach(assembly => LoadAssembly(assembly));
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
            var assemblySet = new HashSet<Assembly>(assemblies);
            assemblySet.ForEach(assembly => LoadAssemblyIncludingDependencies(assembly, recursive));
            return this;
        }

        public ArchLoader LoadFilteredDirectory(
            string directory,
            string filter,
            SearchOption searchOption = TopDirectoryOnly
        )
        {
            var path = Path.GetFullPath(directory);
            _assemblyResolver.AssemblyPath = path;
            var assemblies = Directory.GetFiles(path, filter, searchOption);

            var result = this;
            return assemblies.Aggregate(
                result,
                (current, assembly) => current.LoadAssembly(assembly, false, false)
            );
        }

        public ArchLoader LoadFilteredDirectoryIncludingDependencies(
            string directory,
            string filter,
            bool recursive = false,
            SearchOption searchOption = TopDirectoryOnly
        )
        {
            var path = Path.GetFullPath(directory);
            _assemblyResolver.AssemblyPath = path;
            var assemblies = Directory.GetFiles(path, filter, searchOption);

            var result = this;
            return assemblies.Aggregate(
                result,
                (current, assembly) => current.LoadAssembly(assembly, true, recursive)
            );
        }

        public ArchLoader LoadNamespacesWithinAssembly(Assembly assembly, params string[] namespc)
        {
            var nameSpaces = new HashSet<string>(namespc);
            nameSpaces.ForEach(nameSpace =>
            {
                LoadModule(assembly.Location, nameSpace, false, false);
            });
            return this;
        }

        public ArchLoader LoadAssembly(Assembly assembly)
        {
            return LoadAssembly(assembly.Location, false, false);
        }

        public ArchLoader LoadAssemblyIncludingDependencies(
            Assembly assembly,
            bool recursive = false
        )
        {
            return LoadAssembly(assembly.Location, true, recursive);
        }

        private ArchLoader LoadAssembly(string fileName, bool includeDependencies, bool recursive)
        {
            LoadModule(fileName, null, includeDependencies, recursive);

            return this;
        }

        private void LoadModule(
            string fileName,
            string nameSpace,
            bool includeDependencies,
            bool recursive,
            FilterFunc filterFunc = null
        )
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    var fullPath = Path.GetFullPath(fileName);
                    if (!_loadedAssemblyPaths.Contains(fullPath))
                    {
                        _loadedAssemblyPaths.Add(fullPath);
                    }
                }

                var module = ModuleDefinition.ReadModule(
                    fileName,
                    new ReaderParameters { AssemblyResolver = _assemblyResolver }
                );
                var processedAssemblies = new List<AssemblyNameReference> { module.Assembly.Name };
                var resolvedModules = new List<ModuleDefinition>();
                _assemblyResolver.AddLib(module.Assembly);
                _archBuilder.AddAssembly(module.Assembly, false);
                foreach (var assemblyReference in module.AssemblyReferences)
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
                        try
                        {
                            processedAssemblies.Add(assemblyReference);
                            _assemblyResolver.AddLib(assemblyReference);
                            if (includeDependencies)
                            {
                                var assemblyDefinition =
                                    _assemblyResolver.Resolve(assemblyReference)
                                    ?? throw new AssemblyResolutionException(assemblyReference);
                                _archBuilder.AddAssembly(assemblyDefinition, false);
                                resolvedModules.AddRange(assemblyDefinition.Modules);
                            }
                        }
                        catch (AssemblyResolutionException)
                        {
                            //Failed to resolve assembly, skip it
                        }
                    }
                }

                _archBuilder.LoadTypesForModule(module, nameSpace);
                foreach (var moduleDefinition in resolvedModules)
                {
                    _archBuilder.LoadTypesForModule(moduleDefinition, null);
                }
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }
        }

        private void AddReferencedAssembliesRecursively(
            AssemblyNameReference currentAssemblyReference,
            ICollection<AssemblyNameReference> processedAssemblies,
            List<ModuleDefinition> resolvedModules,
            FilterFunc filterFunc
        )
        {
            if (processedAssemblies.Contains(currentAssemblyReference))
            {
                return;
            }

            processedAssemblies.Add(currentAssemblyReference);
            try
            {
                _assemblyResolver.AddLib(currentAssemblyReference);
                var assemblyDefinition =
                    _assemblyResolver.Resolve(currentAssemblyReference)
                    ?? throw new AssemblyResolutionException(currentAssemblyReference);

                var filterResult = filterFunc?.Invoke(assemblyDefinition);
                if (filterResult?.LoadThisAssembly != false)
                {
                    _archBuilder.AddAssembly(assemblyDefinition, false);
                    resolvedModules.AddRange(assemblyDefinition.Modules);
                }

                foreach (
                    var reference in assemblyDefinition.Modules.SelectMany(m =>
                        m.AssemblyReferences
                    )
                )
                {
                    if (filterResult?.TraverseDependencies != false)
                        AddReferencedAssembliesRecursively(
                            reference,
                            processedAssemblies,
                            resolvedModules,
                            filterFunc
                        );
                }
            }
            catch (AssemblyResolutionException)
            {
                //Failed to resolve assembly, skip it
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
                LoadModule(assembly.Location, null, true, true, filterFunc);
            }
            return this;
        }
    }
}
