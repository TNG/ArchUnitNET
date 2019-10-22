/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

namespace ArchUnitNET.Domain
{
    public enum Visibility
    {
        Public = 0,
        Private = 1,
        Protected = 2,
        Internal = 3,
        ProtectedInternal = 4,
        PrivateProtected = 5,
        NotAccessible = 6 //should only be used for Getters/Setters or as default instead of null
    }

    public static class VisibilityStrings
    {
        private static readonly string[] CapitalLetters =
            {"Public", "Private", "Protected", "Internal", "Protected Internal", "Private Protected", "Not Accessible"};

        private static readonly string[] LowerCase =
            {"public", "private", "protected", "internal", "protected internal", "private protected", "not accessible"};

        public static string ToString(this Visibility visibility, bool useCapitalLetters = false)
        {
            return useCapitalLetters ? CapitalLetters[(int) visibility] : LowerCase[(int) visibility];
        }
    }
}