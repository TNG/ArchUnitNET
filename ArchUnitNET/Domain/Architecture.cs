//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Domain
{
    public class Architecture
    {
        private readonly IEnumerable<Assembly> _allAssemblies;
        private readonly ObjectProviderCache _objectProviderCache;

        public Architecture(IEnumerable<Assembly> allAssemblies, IEnumerable<Namespace> namespaces,
            IEnumerable<IType> types)
        {
            _allAssemblies = allAssemblies;
            Namespaces = namespaces;
            Types = types;
            _objectProviderCache = new ObjectProviderCache(this);
        }

        public IEnumerable<Assembly> Assemblies => _allAssemblies.Where(assembly => !assembly.IsOnlyReferenced);

        public IEnumerable<Namespace> Namespaces { get; }

        public IEnumerable<IType> Types { get; }

        public IEnumerable<Class> Classes => Types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();
        public IEnumerable<Attribute> Attributes => Types.OfType<Attribute>();
        public IEnumerable<PropertyMember> PropertyMembers => Members.OfType<PropertyMember>();
        public IEnumerable<FieldMember> FieldMembers => Members.OfType<FieldMember>();
        public IEnumerable<MethodMember> MethodMembers => Members.OfType<MethodMember>();
        public IEnumerable<IMember> Members => Types.SelectMany(type => type.Members);

        public bool FulfilsRule(IArchRule archRule)
        {
            return archRule.HasNoViolations(this);
        }

        public IEnumerable<T> GetOrCreateObjects<T>(IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction) where T : ICanBeAnalyzed
        {
            return _objectProviderCache.GetOrCreateObjects(objectProvider, providingFunction);
        }

        public IEnumerable<EvaluationResult> EvaluateRule(IArchRule archRule)
        {
            return archRule.Evaluate(this);
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

            return obj.GetType() == GetType() && Equals((Architecture) obj);
        }

        private bool Equals(Architecture other)
        {
            return Assemblies.Equals(other.Assemblies) && Namespaces.Equals(other.Namespaces) &&
                   Types.Equals(other.Types);
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