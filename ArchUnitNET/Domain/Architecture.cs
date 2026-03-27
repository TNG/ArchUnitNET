using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Represents a loaded and analyzed software architecture, containing all assemblies,
    /// namespaces, types, and their relationships.
    /// </summary>
    /// <remarks>
    /// An architecture is typically created via <see cref="Loader.ArchLoader"/>. Rule evaluation
    /// results are cached by default to avoid recomputing the same provider multiple times.
    /// Use <see cref="Loader.ArchLoader.WithoutRuleEvaluationCache"/> to disable this caching.
    /// </remarks>
    public class Architecture
    {
        private readonly IEnumerable<Assembly> _allAssemblies;
        private readonly bool _useRuleEvaluationCache;
        private readonly ConcurrentDictionary<int, object> _ruleEvaluationCache =
            new ConcurrentDictionary<int, object>();

        /// <summary>
        /// Creates a new architecture from the given domain elements.
        /// </summary>
        /// <param name="allAssemblies">All assemblies in the architecture, including referenced-only assemblies.</param>
        /// <param name="namespaces">The namespaces containing the loaded types.</param>
        /// <param name="types">The types loaded from the assemblies.</param>
        /// <param name="genericParameters">Generic parameters found in the loaded types.</param>
        /// <param name="referencedTypes">Types referenced but not directly loaded.</param>
        /// <param name="useRuleEvaluationCache">
        /// If <c>true</c> (the default), rule evaluation results are cached by object provider
        /// hash code and type. Pass <c>false</c> to disable caching.
        /// </param>
        public Architecture(
            IEnumerable<Assembly> allAssemblies,
            IEnumerable<Namespace> namespaces,
            IEnumerable<IType> types,
            IEnumerable<GenericParameter> genericParameters,
            IEnumerable<IType> referencedTypes,
            bool useRuleEvaluationCache = true
        )
        {
            _allAssemblies = allAssemblies;
            Namespaces = namespaces;
            Types = types;
            GenericParameters = genericParameters;
            ReferencedTypes = referencedTypes;
            _useRuleEvaluationCache = useRuleEvaluationCache;
        }

        public IEnumerable<Assembly> Assemblies =>
            _allAssemblies.Where(assembly => !assembly.IsOnlyReferenced);
        public IEnumerable<Namespace> Namespaces { get; }
        public IEnumerable<IType> Types { get; }
        public IEnumerable<GenericParameter> GenericParameters { get; }
        public IEnumerable<IType> ReferencedTypes { get; }
        public IEnumerable<Class> Classes => Types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();
        public IEnumerable<Attribute> Attributes => Types.OfType<Attribute>();
        public IEnumerable<Struct> Structs => Types.OfType<Struct>();
        public IEnumerable<Enum> Enums => Types.OfType<Enum>();
        public IEnumerable<Class> ReferencedClasses => ReferencedTypes.OfType<Class>();
        public IEnumerable<Interface> ReferencedInterfaces => ReferencedTypes.OfType<Interface>();
        public IEnumerable<Attribute> ReferencedAttributes => ReferencedTypes.OfType<Attribute>();
        public IEnumerable<PropertyMember> PropertyMembers => Members.OfType<PropertyMember>();
        public IEnumerable<FieldMember> FieldMembers => Members.OfType<FieldMember>();
        public IEnumerable<MethodMember> MethodMembers => Members.OfType<MethodMember>();
        public IEnumerable<IMember> Members => Types.SelectMany(type => type.Members);

        /// <summary>
        /// Returns the cached result for the given object provider, or invokes the providing
        /// function and caches the result. Caching can be disabled via
        /// <see cref="Loader.ArchLoader.WithoutRuleEvaluationCache"/>.
        /// </summary>
        public IEnumerable<T> GetOrCreateObjects<T>(
            IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction
        )
            where T : ICanBeAnalyzed
        {
            if (!_useRuleEvaluationCache)
            {
                return providingFunction(this);
            }
            unchecked
            {
                var key =
                    (objectProvider.GetHashCode() * 397) ^ objectProvider.GetType().GetHashCode();
                return (IEnumerable<T>)
                    _ruleEvaluationCache.GetOrAdd(key, _ => providingFunction(this));
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Architecture)obj);
        }

        private bool Equals(Architecture other)
        {
            return Assemblies.Equals(other.Assemblies)
                && Namespaces.Equals(other.Namespaces)
                && Types.Equals(other.Types);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (Assemblies != null ? Assemblies.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Namespaces != null ? Namespaces.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Types != null ? Types.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
