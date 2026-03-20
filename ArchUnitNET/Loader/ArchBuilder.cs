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
    /// <summary>
    /// Internal builder that constructs an <see cref="Architecture"/> from loaded modules.
    /// Coordinates type discovery (via <see cref="DomainResolver"/>), type processing
    /// (via the <c>LoadTasks</c> static classes), and architecture assembly. Manages the
    /// <see cref="ArchitectureCache"/> lookup and supports disabling rule-evaluation caching.
    /// </summary>
    internal class ArchBuilder
    {
        private readonly ArchitectureCache _architectureCache;
        private readonly ArchitectureCacheKey _architectureCacheKey;
        private readonly DomainResolver _domainResolver;

        /// <summary>
        /// Non-compiler-generated types paired with their Cecil <see cref="TypeDefinition"/>s,
        /// collected during <see cref="LoadTypesForModule"/> and consumed by
        /// <see cref="ProcessTypes"/> to populate members, dependencies, and attributes.
        /// Keyed by assembly-qualified name for deduplication.
        /// </summary>
        private readonly Dictionary<
            string,
            (ITypeInstance<IType> TypeInstance, TypeDefinition Definition)
        > _typesToProcess =
            new Dictionary<
                string,
                (ITypeInstance<IType> TypeInstance, TypeDefinition Definition)
            >();

        /// <summary>
        /// Assemblies paired with their Cecil <see cref="AssemblyDefinition"/>s,
        /// registered via <see cref="AddAssembly"/>. Keyed by assembly full name
        /// for deduplication. Consumed by Phase 5 (assembly attribute collection).
        /// </summary>
        private readonly Dictionary<
            string,
            (Assembly Assembly, AssemblyDefinition Definition)
        > _assemblyData =
            new Dictionary<string, (Assembly Assembly, AssemblyDefinition Definition)>();

        public ArchBuilder()
        {
            _domainResolver = new DomainResolver();
            _architectureCacheKey = new ArchitectureCacheKey();
            _architectureCache = ArchitectureCache.Instance;
        }

        /// <summary>
        /// Registers an assembly for attribute collection during <see cref="ProcessTypes"/>.
        /// Skips assemblies that have already been registered.
        /// </summary>
        public void AddAssembly([NotNull] AssemblyDefinition moduleAssembly, bool isOnlyReferenced)
        {
            if (_assemblyData.ContainsKey(moduleAssembly.FullName))
            {
                return;
            }

            var references = moduleAssembly
                .MainModule.AssemblyReferences.Select(reference => reference.Name)
                .ToList();

            var assembly = _domainResolver.GetOrCreateAssembly(
                moduleAssembly.Name.Name,
                moduleAssembly.FullName,
                isOnlyReferenced,
                references
            );
            _assemblyData.Add(moduleAssembly.FullName, (assembly, moduleAssembly));
        }

        /// <summary>
        /// Discovers types in the given <paramref name="module"/>, creates domain type instances
        /// via <see cref="DomainResolver"/>, and records them for later processing.
        /// Filters out compiler-generated types, code-coverage instrumentation types,
        /// nullable context attributes, and types outside the optional
        /// <paramref name="namespaceFilter"/>.
        /// </summary>
        public void LoadTypesForModule(ModuleDefinition module, string namespaceFilter)
        {
            _architectureCacheKey.Add(module.Name, namespaceFilter);

            var types = module.Types.First().FullName.Contains("<Module>")
                ? module.Types.Skip(1).ToList()
                : module.Types.ToList();

            types = types
                .Where(t =>
                    t.FullName != "Microsoft.CodeAnalysis.EmbeddedAttribute"
                    && t.FullName != "System.Runtime.CompilerServices.NullableAttribute"
                    && t.FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                    && !t.FullName.StartsWith("Coverlet")
                )
                .ToList();

            var nestedTypes = types;
            while (nestedTypes.Any())
            {
                nestedTypes = nestedTypes
                    .SelectMany(typeDefinition =>
                        typeDefinition.NestedTypes.Where(type => !type.IsCompilerGenerated())
                    )
                    .ToList();
                types.AddRange(nestedTypes);
            }

            types
                .Where(typeDefinition =>
                    (
                        namespaceFilter == null
                        || typeDefinition.Namespace.StartsWith(namespaceFilter)
                    )
                    && typeDefinition.CustomAttributes.All(att =>
                        att.AttributeType.FullName
                        != "Microsoft.VisualStudio.TestPlatform.TestSDKAutoGeneratedCode"
                    )
                )
                .ForEach(typeDefinition =>
                {
                    var typeInstance = _domainResolver.GetOrCreateTypeInstanceFromTypeReference(
                        typeDefinition
                    );
                    var type = typeInstance.Type;
                    var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                        module.Assembly.Name.Name,
                        typeDefinition.FullName
                    );
                    if (
                        !_typesToProcess.ContainsKey(assemblyQualifiedName)
                        && !type.IsCompilerGenerated
                    )
                    {
                        _typesToProcess.Add(assemblyQualifiedName, (typeInstance, typeDefinition));
                    }
                });
        }

        /// <summary>
        /// Builds the <see cref="Architecture"/> from all loaded modules. Returns a cached
        /// instance when available (unless <paramref name="skipArchitectureCache"/> is set).
        /// </summary>
        public Architecture Build(bool skipRuleEvaluationCache, bool skipArchitectureCache)
        {
            if (skipRuleEvaluationCache)
            {
                _architectureCacheKey.SetRuleEvaluationCacheDisabled();
            }

            if (!skipArchitectureCache)
            {
                var architecture = _architectureCache.TryGetArchitecture(_architectureCacheKey);
                if (architecture != null)
                {
                    return architecture;
                }
            }

            ProcessTypes();

            var allTypes = _domainResolver
                .Types.Select(instance => instance.Type)
                .Where(type => !type.IsCompilerGenerated)
                .Distinct()
                .ToList();
            var types = allTypes
                .Where(type => !type.IsStub && !(type is GenericParameter))
                .ToList();
            var genericParameters = allTypes.OfType<GenericParameter>().ToList();
            var referencedTypes = allTypes
                .Where(type => type.IsStub && !(type is GenericParameter))
                .ToList();
            var namespaces = _domainResolver.Namespaces.Where(ns => ns.Types.Any());
            var newArchitecture = new Architecture(
                _domainResolver.Assemblies,
                namespaces,
                types,
                genericParameters,
                referencedTypes,
                !skipRuleEvaluationCache
            );

            if (!skipArchitectureCache)
            {
                _architectureCache.Add(_architectureCacheKey, newArchitecture);
            }

            return newArchitecture;
        }

        /// <summary>
        /// Runs all type-processing phases in the required order across every discovered type.
        /// Each phase must complete for all types before the next phase begins, because later
        /// phases depend on data populated by earlier ones (e.g. members must exist before
        /// method dependencies can be resolved).
        /// </summary>
        private void ProcessTypes()
        {
            var typesToProcess = _typesToProcess.Values;

            // Phase 1: Base class dependency (non-interface types only)
            foreach (var entry in typesToProcess.Where(entry => !entry.Definition.IsInterface))
            {
                AddBaseClassDependency.Execute(
                    entry.TypeInstance.Type,
                    entry.Definition,
                    _domainResolver
                );
            }

            // Phase 2: Members (fields, properties, methods)
            // Collect (TypeInstance, Definition, MemberData) for use in Phases 4 and 7.
            var typesWithMemberData = new List<(
                ITypeInstance<IType> TypeInstance,
                TypeDefinition TypeDef,
                MemberData MemberData
            )>(_typesToProcess.Count);
            typesWithMemberData.AddRange(
                from entry in typesToProcess
                let memberData = AddMembers.Execute(
                    entry.TypeInstance,
                    entry.Definition,
                    _domainResolver
                )
                select (entry.TypeInstance, entry.Definition, memberData)
            );

            // Phase 3: Generic parameter dependencies
            foreach (var entry in typesToProcess)
            {
                AddGenericParameterDependencies.Execute(entry.TypeInstance.Type);
            }

            // Phase 4: Attributes and attribute dependencies
            foreach (var entry in typesWithMemberData)
            {
                AddAttributesAndAttributeDependencies.Execute(
                    entry.TypeInstance.Type,
                    entry.TypeDef,
                    _domainResolver,
                    entry.MemberData.MethodPairs
                );
            }

            // Phase 5: Assembly-level attributes
            // Materialized to a list because CollectAssemblyAttributes can trigger
            // GetOrCreateAssembly in DomainResolver, which would invalidate a lazy query.
            var assemblyData = _assemblyData.Values.ToList();
            foreach (var entry in assemblyData)
            {
                CollectAssemblyAttributes.Execute(
                    entry.Assembly,
                    entry.Definition,
                    _domainResolver
                );
            }

            // Phase 6: Field and property type dependencies
            foreach (var entry in typesToProcess)
            {
                AddFieldAndPropertyDependencies.Execute(entry.TypeInstance.Type);
            }

            // Phase 7: Method signature and body dependencies
            foreach (var entry in typesWithMemberData)
            {
                AddMethodDependencies.Execute(
                    entry.TypeInstance.Type,
                    _domainResolver,
                    entry.MemberData.MethodPairs,
                    entry.MemberData.PropertyByAccessor
                );
            }

            // Phase 8: Generic argument dependencies
            foreach (var entry in typesToProcess)
            {
                AddGenericArgumentDependencies.Execute(entry.TypeInstance.Type);
            }

            // Phase 9: Interface and member-to-type dependencies
            foreach (var entry in typesToProcess)
            {
                AddClassDependencies.Execute(
                    entry.TypeInstance.Type,
                    entry.Definition,
                    _domainResolver
                );
            }

            // Phase 10: Backwards dependencies
            foreach (var entry in typesToProcess)
            {
                AddBackwardsDependencies.Execute(entry.TypeInstance.Type);
            }

            // Phase 11: Register types with their namespaces
            AddTypesToNamespaces.Execute(
                _typesToProcess.Values.Select(entry => entry.TypeInstance.Type)
            );
        }
    }
}
