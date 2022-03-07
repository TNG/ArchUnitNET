//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class SliceIdentifierComparer : IEqualityComparer<SliceIdentifier>
    {
        public bool Equals(SliceIdentifier x, SliceIdentifier y)
        {
            return x != null && x.CompareTo(y);
        }

        public int GetHashCode(SliceIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
}