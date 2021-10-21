//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class Slice : IHasDescription, IHasDependencies
    {
        public readonly SliceIdentifier Identifier;
        public readonly IEnumerable<IType> Types;

        public Slice(SliceIdentifier identifier, IEnumerable<IType> types)
        {
            Identifier = identifier;
            Types = types;
        }

        public IEnumerable<Class> Classes => Types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();

        public List<ITypeDependency> Dependencies => Types.SelectMany(type => type.Dependencies).ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            Types.SelectMany(type => type.BackwardsDependencies).ToList();

        public string Description => Identifier.Description;

        protected bool Equals(Slice other)
        {
            return Equals(Identifier, other.Identifier) && Equals(Types, other.Types);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Slice)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0) * 397) ^
                       (Types != null ? Types.GetHashCode() : 0);
            }
        }
    }
}