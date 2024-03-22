//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Extensions
{
    public static class SyntaxElementExtensions
    {
        public static string GetShortDescription(this IHasDescription obj, int maxLength = 150)
        {
            return obj.Description.Length > maxLength
                ? obj.Description.Substring(0, maxLength - 3) + "..."
                : obj.Description;
        }
    }
}
