//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;

namespace ArchUnitNET.Domain.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute(this IHasAttributes a, string fullName)
        {
            return a.Attributes.Any(attribute => attribute.FullNameEquals(fullName));
        }

        public static bool HasAttributeMatching(this IHasAttributes a, string pattern)
        {
            return a.Attributes.Any(attribute => attribute.FullNameMatches(pattern));
        }

        public static bool OnlyHasAttributes(this IHasAttributes a, string name)
        {
            return a.Attributes.IsNullOrEmpty()
                || a.Attributes.All(attribute => attribute.FullNameEquals(name));
        }

        public static bool OnlyHasAttributesMatching(this IHasAttributes a, string pattern)
        {
            return a.Attributes.IsNullOrEmpty()
                || a.Attributes.All(attribute => attribute.FullNameMatches(pattern));
        }
    }
}
