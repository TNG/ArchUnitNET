//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain
{
    public enum Visibility
    {
        Public = 0,
        ProtectedInternal = 1,
        Internal = 2,
        Protected = 3,
        PrivateProtected = 4,
        Private = 5,
        NotAccessible =
            6 //should only be used for Getters/Setters or as default instead of null
        ,
    }

    public static class VisibilityStrings
    {
        private static readonly string[] CapitalLetters =
        {
            "Public",
            "Protected Internal",
            "Internal",
            "Protected",
            "Private Protected",
            "Private",
            "Not Accessible",
        };

        private static readonly string[] LowerCase =
        {
            "public",
            "protected internal",
            "internal",
            "protected",
            "private protected",
            "private",
            "not accessible",
        };

        public static string ToString(this Visibility visibility, bool useCapitalLetters = false)
        {
            return useCapitalLetters ? CapitalLetters[(int)visibility] : LowerCase[(int)visibility];
        }
    }
}
