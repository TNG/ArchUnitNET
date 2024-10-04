//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class Architecture
    {
        private readonly List<Assembly> _assemblies;
        private readonly List<Namespace> _namespaces;
        private readonly List<IType> _types;
        private readonly List<GenericParameter> _genericParameters;
        private readonly List<IType> _referencedTypes;
        private readonly List<IMember> _members;
        private readonly ObjectProviderCache _objectProviderCache;

        public Architecture(
            List<Assembly> assemblies,
            List<Namespace> namespaces,
            List<IType> types,
            List<GenericParameter> genericParameters,
            List<IType> referencedTypes
        )
        {
            _assemblies = assemblies;
            _namespaces = namespaces;
            _types = types;
            _genericParameters = genericParameters;
            _referencedTypes = referencedTypes;
            _members = types.SelectMany(type => type.Members).ToList();
            _objectProviderCache = new ObjectProviderCache(this);
        }

        public IEnumerable<Assembly> Assemblies =>
            _assemblies.Where(assembly => !assembly.IsOnlyReferenced);
        public IEnumerable<Namespace> Namespaces => _namespaces;
        public IEnumerable<IType> Types => _types;
        public IEnumerable<GenericParameter> GenericParameters => _genericParameters;
        public IEnumerable<IType> ReferencedTypes => _referencedTypes;
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
        public IEnumerable<IMember> Members => _members;

        public IEnumerable<T> GetOrCreateObjects<T>(
            IObjectProvider<T> objectProvider,
            Func<Architecture, IEnumerable<T>> providingFunction
        )
            where T : ICanBeAnalyzed
        {
            return _objectProviderCache.GetOrCreateObjects(objectProvider, providingFunction);
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
