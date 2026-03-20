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
        private readonly List<LoadInstruction> _loadInstructions = new List<LoadInstruction>();

        // ---------------------------------------------------------------------------
        // LoadInstruction hierarchy
        // ---------------------------------------------------------------------------

        private abstract class LoadInstruction
        {
            public bool IncludeDependencies { get; }
            public bool Recursive { get; }

            protected LoadInstruction(bool includeDependencies, bool recursive)
            {
                IncludeDependencies = includeDependencies;
                Recursive = recursive;
            }
        }

        private sealed class FileLoadInstruction : LoadInstruction
        {
            public string FileName { get; }
            public string NamespaceFilter { get; }
            public FilterFunc FilterFunc { get; }

            public FileLoadInstruction(
                string fileName,
                string namespaceFilter,
                bool includeDependencies,
                bool recursive,
                FilterFunc filterFunc = null
            )
                : base(includeDependencies, recursive)
            {
                FileName = fileName;
                NamespaceFilter = namespaceFilter;
                FilterFunc = filterFunc;
            }
        }

        private sealed class DirectoryLoadInstruction : LoadInstruction
        {
            public string Directory { get; }
            public string DirectoryFilter { get; }
            public SearchOption SearchOption { get; }

            public DirectoryLoadInstruction(
                string directory,
                string directoryFilter,
                SearchOption searchOption,
                bool includeDependencies,
                bool recursive
            )
                : base(includeDependencies, recursive)
            {
                Directory = directory;
                DirectoryFilter = directoryFilter;
                SearchOption = searchOption;
            }
        }

        // ---------------------------------------------------------------------------
        // Build
        // ---------------------------------------------------------------------------

        public Architecture Build()
        {
            var assemblyResolver = new DotNetCoreAssemblyResolver();
            try
            {
                var archBuilder = new ArchBuilder();
                foreach (var instruction in _loadInstructions)
                {
                    if (instruction is DirectoryLoadInstruction dir)
                    {
                        ProcessInstruction(assemblyResolver, archBuilder, dir);
                    }
                    else
                    {
                        ProcessInstruction(
                            assemblyResolver,
                            archBuilder,
                            (FileLoadInstruction)instruction
                        );
                    }
                }

                return archBuilder.Build();
            }
            finally
            {
                assemblyResolver.Dispose();
            }
        }

        // ---------------------------------------------------------------------------
        // Instruction processing
        // ---------------------------------------------------------------------------

        private static void ProcessInstruction(
            DotNetCoreAssemblyResolver assemblyResolver,
            ArchBuilder archBuilder,
            DirectoryLoadInstruction instruction
        )
        {
            var path = Path.GetFullPath(instruction.Directory);
            assemblyResolver.AssemblyPath = path;
            var files = System.IO.Directory.GetFiles(
                path,
                instruction.DirectoryFilter,
                instruction.SearchOption
            );
            foreach (var file in files)
            {
                LoadModule(
                    assemblyResolver,
                    archBuilder,
                    file,
                    null,
                    instruction.IncludeDependencies,
                    instruction.Recursive
                );
            }
        }

        private static void ProcessInstruction(
            DotNetCoreAssemblyResolver assemblyResolver,
            ArchBuilder archBuilder,
            FileLoadInstruction instruction
        )
        {
            LoadModule(
                assemblyResolver,
                archBuilder,
                instruction.FileName,
                instruction.NamespaceFilter,
                instruction.IncludeDependencies,
                instruction.Recursive,
                instruction.FilterFunc
            );
        }

        // ---------------------------------------------------------------------------
        // Public loading API
        // ---------------------------------------------------------------------------

        public ArchLoader LoadAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in new HashSet<Assembly>(assemblies))
            {
                _loadInstructions.Add(
                    new FileLoadInstruction(assembly.Location, null, false, false)
                );
            }
            return this;
        }

        public ArchLoader LoadAssembly(Assembly assembly) => LoadAssemblies(assembly);

        public ArchLoader LoadAssembliesIncludingDependencies(params Assembly[] assemblies) =>
            LoadAssembliesIncludingDependencies(assemblies, false);

        public ArchLoader LoadAssembliesIncludingDependencies(
            IEnumerable<Assembly> assemblies,
            bool recursive
        )
        {
            foreach (var assembly in new HashSet<Assembly>(assemblies))
            {
                _loadInstructions.Add(
                    new FileLoadInstruction(assembly.Location, null, true, recursive)
                );
            }
            return this;
        }

        public ArchLoader LoadAssemblyIncludingDependencies(
            Assembly assembly,
            bool recursive = false
        ) => LoadAssembliesIncludingDependencies(new[] { assembly }, recursive);

        public ArchLoader LoadNamespacesWithinAssembly(Assembly assembly, params string[] namespc)
        {
            foreach (var nameSpace in new HashSet<string>(namespc))
            {
                _loadInstructions.Add(
                    new FileLoadInstruction(assembly.Location, nameSpace, false, false)
                );
            }
            return this;
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
                _loadInstructions.Add(
                    new FileLoadInstruction(assembly.Location, null, true, true, filterFunc)
                );
            }
            return this;
        }

        public ArchLoader LoadFilteredDirectory(
            string directory,
            string filter,
            SearchOption searchOption = TopDirectoryOnly
        )
        {
            _loadInstructions.Add(
                new DirectoryLoadInstruction(directory, filter, searchOption, false, false)
            );
            return this;
        }

        public ArchLoader LoadFilteredDirectoryIncludingDependencies(
            string directory,
            string filter,
            bool recursive = false,
            SearchOption searchOption = TopDirectoryOnly
        )
        {
            _loadInstructions.Add(
                new DirectoryLoadInstruction(directory, filter, searchOption, true, recursive)
            );
            return this;
        }

        // ---------------------------------------------------------------------------
        // Core module loading
        // ---------------------------------------------------------------------------

        private static void LoadModule(
            DotNetCoreAssemblyResolver assemblyResolver,
            ArchBuilder archBuilder,
            string fileName,
            string nameSpace,
            bool includeDependencies,
            bool recursive,
            FilterFunc filterFunc = null
        )
        {
            try
            {
                var module = ModuleDefinition.ReadModule(
                    fileName,
                    new ReaderParameters { AssemblyResolver = assemblyResolver }
                );
                var processedAssemblies = new HashSet<string> { module.Assembly.Name.FullName };
                var resolvedModules = new List<ModuleDefinition>();
                assemblyResolver.AddLib(module.Assembly);
                archBuilder.AddAssembly(module.Assembly, false);
                foreach (var assemblyReference in module.AssemblyReferences)
                {
                    if (includeDependencies && recursive)
                    {
                        AddReferencedAssembliesRecursively(
                            assemblyResolver,
                            archBuilder,
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
                            processedAssemblies.Add(assemblyReference.FullName);
                            assemblyResolver.AddLib(assemblyReference);
                            if (includeDependencies)
                            {
                                var assemblyDefinition =
                                    assemblyResolver.Resolve(assemblyReference)
                                    ?? throw new AssemblyResolutionException(assemblyReference);
                                archBuilder.AddAssembly(assemblyDefinition, false);
                                resolvedModules.AddRange(assemblyDefinition.Modules);
                            }
                        }
                        catch (AssemblyResolutionException)
                        {
                            //Failed to resolve assembly, skip it
                        }
                    }
                }

                archBuilder.LoadTypesForModule(module, nameSpace);
                foreach (var moduleDefinition in resolvedModules)
                {
                    archBuilder.LoadTypesForModule(moduleDefinition, null);
                }
            }
            catch (BadImageFormatException)
            {
                // invalid file format of DLL or executable, therefore ignored
            }
        }

        private static void AddReferencedAssembliesRecursively(
            DotNetCoreAssemblyResolver assemblyResolver,
            ArchBuilder archBuilder,
            AssemblyNameReference currentAssemblyReference,
            ICollection<string> processedAssemblies,
            List<ModuleDefinition> resolvedModules,
            FilterFunc filterFunc
        )
        {
            if (processedAssemblies.Contains(currentAssemblyReference.FullName))
            {
                return;
            }

            processedAssemblies.Add(currentAssemblyReference.FullName);
            try
            {
                assemblyResolver.AddLib(currentAssemblyReference);
                var assemblyDefinition =
                    assemblyResolver.Resolve(currentAssemblyReference)
                    ?? throw new AssemblyResolutionException(currentAssemblyReference);

                var filterResult = filterFunc?.Invoke(assemblyDefinition);
                if (filterResult?.LoadThisAssembly != false)
                {
                    archBuilder.AddAssembly(assemblyDefinition, false);
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
                            assemblyResolver,
                            archBuilder,
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
    }
}
