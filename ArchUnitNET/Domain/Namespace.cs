//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class Namespace : Slice, IHasName
    {
        public Namespace(string name, IEnumerable<IType> types)
            : base(SliceIdentifier.Of(name), types) { }

        public string Name => Description;
        public string FullName => Description;
    }
}
