/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.ArchitectureExceptions;

namespace ArchUnitNET.Matcher
{
    public static class Matcher
    {
        public static void ShouldAll<T>(this IEnumerable<T> enumerable, Func<T, bool> matcher)
        {
            var first = enumerable.FirstOrDefault(arg => !matcher(arg));
            if (first != null)
            {
                throw new ArchitectureException(first.ToString());
            }
        }
    }
}