//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public static class PlantUmlNameChecker
    {
        private static readonly string[] ForbiddenCharacters =
        {
            "[",
            "]",
            "\r",
            "\n",
            "\f",
            "\a",
            "\b",
            "\v"
        };

        public static bool ContainsForbiddenCharacters([CanBeNull] string name)
        {
            return name != null && ForbiddenCharacters.Any(name.Contains);
        }

        public static void AssertNoForbiddenCharacters(params string[] names)
        {
            if (names.Any(ContainsForbiddenCharacters))
            {
                throw new IllegalComponentNameException(
                    "PlantUml component names must not contain \"[\" or \"]\" or any of the escape characters \"\\r\", \"\\n\", \"\\f\", \"\\a\", \"\\b\", \"\\v\"."
                );
            }
        }

        public static void AssertNotNullOrEmpty(params string[] names)
        {
            if (names.Any(name => name.IsNullOrEmpty()))
            {
                throw new IllegalComponentNameException(
                    "PlantUml component names can't be null or empty."
                );
            }
        }
    }
}
