//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class StringIdentifierComparer : EqualityComparer<StringIdentifier>
    {
        public override bool Equals(StringIdentifier x, StringIdentifier y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Identifier == y.Identifier;
        }

        public override int GetHashCode(StringIdentifier obj)
        {
            return obj.Identifier.GetHashCode();
        }
    }
}