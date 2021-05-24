﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceAssignment : IHasDescription
    {
        private readonly Func<IType, SliceIdentifier> _assignIdentifierFunc;


        public SliceAssignment(Func<IType, SliceIdentifier> assignIdentifierFunc, string description)
        {
            _assignIdentifierFunc = assignIdentifierFunc;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<Slice> Apply(IEnumerable<IType> types)
        {
            return types.GroupBy(_assignIdentifierFunc, (identifier, enumerable) => new Slice(identifier, enumerable),
                SliceIdentifier.Comparer);
        }
    }
}