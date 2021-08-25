//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader.LoadTasks;
using JetBrains.Annotations;
using Mono.Cecil;
using GenericParameter = ArchUnitNET.Domain.GenericParameter;

namespace ArchUnitNET.Loader
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
            var methodMemberRegistry = new MethodMemberRegistry();
            _typeFactory = new TypeFactory(typeRegistry, methodMemberRegistry, _loadTaskRegistry, _assemblyRegistry,
                _namespaceRegistry);
            _architectureCacheKey = new ArchitectureCacheKey();
            _architectureCache = ArchitectureCache.Instance;
        }

        public IEnumerable<IType> Types => _architectureTypes;
        public IEnumerable<Assembly> Assemblies => _assemblyRegistry.Assemblies;
        public IEnumerable<Namespace> Namespaces => _namespaceRegistry.Namespaces;

        public void AddAssembly([NotNull] AssemblyDefinition moduleAssembly, bool isOnlyReferenced)
        {
            if (!_assemblyRegistry.ContainsAssembly(moduleAssembly.Name.FullName))
            {
                var assembly = _assemblyRegistry.GetOrCreateAssembly(moduleAssembly.Name.FullName,
                    moduleAssembly.FullName, isOnlyReferenced);
                _loadTaskRegistry.Add(typeof(CollectAssemblyAttributes),
                    new CollectAssemblyAttributes(assembly, moduleAssembly, _typeFactory));
            }
        }

        public void LoadTypesForModule(ModuleDefinition module, string namespaceFilter)
        {
            _architectureCacheKey.Add(module.Name, namespaceFilter);

            ICollection<TypeDefinition> types;

            if (module.Types.First().FullName.Contains("<Module>"))
            {
                types = module.Types.Skip(1).ToList();
            }
            else
            {
                types = module.Types;
            }


            var allTypes = types.Concat(types.SelectMany(typeDefinition =>
                typeDefinition.NestedTypes.Where(type => !type.IsCompilerGenerated())));
            allTypes
                .Where(typeDefinition => RegexUtils.MatchNamespaces(namespaceFilter,
                    typeDefinition.Namespace))
                .ForEach(typeDefinition =>
                {
                    var type = _typeFactory.GetOrCreateTypeFromTypeReference(typeDefinition);
                    if (!_architectureTypes.Contains(type) && !type.IsCompilerGenerated)
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
                typeof(AddGenericParameterDependencies),
                typeof(AddAttributesAndAttributeDependencies),
                typeof(CollectAssemblyAttributes),
                typeof(AddFieldAndPropertyDependencies),
                typeof(AddMethodDependencies),
                typeof(AddGenericArgumentDependencies),
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
            var allTypes = _typeFactory.GetAllNonCompilerGeneratedTypes().ToList();
            var genericParameters = allTypes.OfType<GenericParameter>().ToList();
            var referencedTypes = allTypes.Except(Types).Except(genericParameters);
            var newArchitecture = new Architecture(Assemblies, Namespaces, Types, genericParameters, referencedTypes);
            _architectureCache.Add(_architectureCacheKey, newArchitecture);
            return newArchitecture;
        }
    }
}