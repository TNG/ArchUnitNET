﻿/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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