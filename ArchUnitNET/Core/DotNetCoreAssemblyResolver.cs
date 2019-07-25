/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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