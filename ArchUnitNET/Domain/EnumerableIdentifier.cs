//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Text;

namespace ArchUnitNET.Domain
{
    public class EnumerableIdentifier : StringIdentifier
    {
        public EnumerableIdentifier(IEnumerable<StringIdentifier> enumerable) : base(CreateIdentifierString(enumerable))
        {
        }

        private static string CreateIdentifierString(IEnumerable<StringIdentifier> enumerable)
        {
            var sb = new StringBuilder("<>_first:");
            foreach (var identifier in enumerable)
            {
                sb.Append(identifier.Identifier);
                sb.Append("+<>_next:");
            }

            sb.Append("+<>_end");
            return sb.ToString();
        }
    }
}