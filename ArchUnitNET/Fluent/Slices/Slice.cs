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

namespace ArchUnitNET.Fluent.Slices
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

        public List<ITypeDependency> Dependencies => Types.SelectMany(type => type.Dependencies).ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            Types.SelectMany(type => type.BackwardsDependencies).ToList();

        public string Description => Identifier.Description;
    }
}