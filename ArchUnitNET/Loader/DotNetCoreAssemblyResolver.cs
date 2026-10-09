using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    internal class DotNetCoreAssemblyResolver : IAssemblyResolver
    {
        private readonly DefaultAssemblyResolver _defaultAssemblyResolver;
        private readonly Dictionary<string, AssemblyDefinition> _libraries;
        public string AssemblyPath { get; set; } = "";

        public DotNetCoreAssemblyResolver()
        {
            _libraries = new Dictionary<string, AssemblyDefinition>();
            _defaultAssemblyResolver = new DefaultAssemblyResolver();
        }

        [CanBeNull]
        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return Resolve(name, new ReaderParameters { AssemblyResolver = this });
        }

        [CanBeNull]
        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_libraries.TryGetValue(name.FullName, out var assemblyDefinition))
            {
                return assemblyDefinition;
            }

            if (!string.IsNullOrEmpty(AssemblyPath))
            {
                var file = Directory
                    .EnumerateFiles(AssemblyPath, $"{name.Name}.dll", SearchOption.AllDirectories)
                    .FirstOrDefault();

                if (file != null)
                {
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(file, parameters);
                    _libraries.Add(name.FullName, assemblyDefinition);
                    return assemblyDefinition;
                }
            }

            // Fall back to DefaultAssemblyResolver for framework assemblies not found in
            // AssemblyPath. Pass the original parameters so the loaded assembly keeps using
            // this resolver (which returns null for unresolvable references) instead of the
            // DefaultAssemblyResolver (which throws).
            try
            {
                assemblyDefinition = _defaultAssemblyResolver.Resolve(name, parameters);
            }
            catch (AssemblyResolutionException)
            {
                return null;
            }

            if (assemblyDefinition != null)
            {
                _libraries.Add(name.FullName, assemblyDefinition);
            }

            return assemblyDefinition;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddLib([NotNull] AssemblyDefinition moduleAssembly)
        {
            if (!_libraries.ContainsKey(moduleAssembly.FullName))
            {
                _libraries.Add(moduleAssembly.FullName, moduleAssembly);
            }
        }

        private void AddLib(
            [NotNull] AssemblyNameReference name,
            [NotNull] AssemblyDefinition moduleAssembly
        )
        {
            if (!_libraries.ContainsKey(name.FullName))
            {
                _libraries.Add(name.FullName, moduleAssembly);
            }
        }

        public void AddLib(AssemblyNameReference name)
        {
            var assembly = Resolve(name);
            AddLib(name, assembly ?? throw new AssemblyResolutionException(name));
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
