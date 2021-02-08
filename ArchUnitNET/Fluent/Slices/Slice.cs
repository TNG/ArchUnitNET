//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Identifiers;

namespace ArchUnitNET.Fluent.Slices
{
    public class Slice : IHasDescription
    {
        public readonly SliceIdentifier Identifier;
        public readonly IEnumerable<IType> Types;

        public Slice(SliceIdentifier identifier, IEnumerable<IType> types)
        {
            Identifier = identifier;
            Types = types;
        }

        public IEnumerable<ITypeDependency> Dependencies => Types.SelectMany(type => type.Dependencies);
        public string Description => Identifier.Description;
    }
}