/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Core.LoadTasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    internal class ArchBuilder
    {
        private readonly ArchitectureCache _architectureCache;
        private readonly ArchitectureCacheKey _architectureCacheKey;
        private readonly List<IType> _architectureTypes = new List<IType>();
        private readonly AssemblyRegistry _assemblyRegistry;
        private readonly LoadTaskRegistry _loadTaskRegistry;
        private readonly NamespaceRegistry _namespaceRegistry;
        private readonly TypeFactory _typeFactory;

        public ArchBuilder()
        {
            _assemblyRegistry = new AssemblyRegistry();
            _namespaceRegistry = new NamespaceRegistry();
            _loadTaskRegistry = new LoadTaskRegistry();
            var typeRegistry = new TypeRegistry();
            _typeFactory = new TypeFactory(typeRegistry, _loadTaskRegistry, _assemblyRegistry, _namespaceRegistry);
            _architectureCacheKey = new ArchitectureCacheKey();

            _architectureCache = ArchitectureCache.Instance;
        }

        public IEnumerable<IType> Types => _architectureTypes;
        public IEnumerable<Assembly> Assemblies => _assemblyRegistry.Assemblies;
        public IEnumerable<Namespace> Namespaces => _namespaceRegistry.Namespaces;

        public void AddAssembly([NotNull] AssemblyDefinition moduleAssembly, bool isOnlyReferenced)
        {
            _assemblyRegistry.GetOrCreateAssembly(moduleAssembly.Name.FullName, moduleAssembly.FullName,
                isOnlyReferenced);
        }

        public void LoadTypesForModule(ModuleDefinition module, string namespaceFilter)
        {
            _architectureCacheKey.Add(module.Name, namespaceFilter);

            var types = module.Types;

            var allTypes = types.Concat(types.SelectMany(typeDefinition => typeDefinition.NestedTypes));
            allTypes
                .Where(typeDefinition => RegexUtils.MatchNamespaces(namespaceFilter,
                    typeDefinition.Namespace))
                .ForEach(typeDefinition =>
                {
                    var type = _typeFactory.GetOrCreateTypeFromTypeReference(typeDefinition);
                    if (!_architectureTypes.Contains(type))
                    {
                        _architectureTypes.Add(type);
                    }
                });

            _namespaceRegistry.Namespaces
                .Where(ns => RegexUtils.MatchNamespaces(namespaceFilter, ns.FullName))
                .ForEach(ns =>
                {
                    _loadTaskRegistry.Add(typeof(AddTypesToNamespace),
                        new AddTypesToNamespace(ns, _architectureTypes));
                });
        }

        private void UpdateTypeDefinitions()
        {
            _loadTaskRegistry.ExecuteTasks(new List<System.Type>
            {
                typeof(AddMembers),
                typeof(AddAttributesAndAttributeDependencies),
                typeof(AddFieldAndPropertyDependencies),
                typeof(AddMethodDependencies),
                typeof(AddClassDependencies),
                typeof(AddBackwardsDependencies),
                typeof(AddTypesToNamespace)
            });
        }

        public Architecture Build()
        {
            var architecture = _architectureCache.TryGetArchitecture(_architectureCacheKey);
            if (architecture != null)
            {
                return architecture;
            }

            UpdateTypeDefinitions();
            var newArchitecture =
                new Architecture(Assemblies, Namespaces, Types.Skip(1)); //Skip first Type to ignore <Module>
            _architectureCache.Add(_architectureCacheKey, newArchitecture);
            return newArchitecture;
        }
    }
}