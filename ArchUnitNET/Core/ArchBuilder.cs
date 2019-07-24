/*
 * Copyright 2019 TNG Technology Consulting GmbH
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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Core.LoadTasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    internal class ArchBuilder
    {
        private readonly List<IType> _architectureTypes = new List<IType>();
        private readonly AssemblyRegistry _assemblyRegistry;
        private readonly LoadTaskRegistry _loadTaskRegistry;
        private readonly NamespaceRegistry _namespaceRegistry;
        private readonly TypeFactory _typeFactory;
        private readonly ArchitectureCacheKey _architectureCacheKey;
        private readonly ArchitectureCache _architectureCache;

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

        public void LoadTypesForModule(ModuleDefinition module, string namespaceFilter)
        {
            _architectureCacheKey.Add(module.Name, namespaceFilter);

            var allTypes = module.Types.Concat(module.Types.SelectMany(typeDefinition => typeDefinition.NestedTypes));
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
                    _loadTaskRegistry.Add(typeof(AddTypesToNamespace), new AddTypesToNamespace(ns, _architectureTypes));
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
            var newArchitecture = new Architecture(Assemblies, Namespaces, Types);
            _architectureCache.Add(_architectureCacheKey, newArchitecture);
            return newArchitecture;
        }
    }
}