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

namespace ArchUnitNET.Domain
{
    public class Architecture
    {
        public Architecture(IEnumerable<Assembly> assemblies, IEnumerable<Namespace> namespaces,
            IEnumerable<IType> types)
        {
            Assemblies = assemblies;
            Namespaces = namespaces;
            Types = types;
        }

        public IEnumerable<Assembly> Assemblies { get; }

        public IEnumerable<Namespace> Namespaces { get; }

        public IEnumerable<IType> Types { get; }

        public IEnumerable<Class> Classes => Types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();

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
            var hashCode = 397 ^ (Assemblies != null ? Assemblies.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Namespaces != null ? Namespaces.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Types != null ? Types.GetHashCode() : 0);
            return hashCode;
        }
    }
}