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
using ArchUnitNET.Fluent.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    internal class DotNetCoreAssemblyResolver : IAssemblyResolver
    {
        private readonly DefaultAssemblyResolver _defaultAssemblyResolver;
        private readonly Dictionary<string, AssemblyDefinition> _libraries;
        public string AssemblyPath = "";

        public DotNetCoreAssemblyResolver()
        {
            _libraries = new Dictionary<string, AssemblyDefinition>();
            _defaultAssemblyResolver = new DefaultAssemblyResolver();
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

            if (_libraries.TryGetValue(name.FullName, out var assemblyDefinition) || string.IsNullOrEmpty(AssemblyPath))
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
            _libraries.Add(name.FullName, assemblyDefinition);

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

        private void AddLib([NotNull] AssemblyNameReference name, [NotNull] AssemblyDefinition moduleAssembly)
        {
            if (!_libraries.ContainsKey(name.FullName))
            {
                _libraries.Add(name.FullName, moduleAssembly);
            }
        }

        public void AddLib(AssemblyNameReference name)
        {
            var assembly = _defaultAssemblyResolver.Resolve(name);
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