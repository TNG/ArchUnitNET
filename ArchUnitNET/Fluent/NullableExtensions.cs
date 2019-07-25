/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.ArchitectureExceptions;

namespace ArchUnitNET.Fluent
{
    public static class NullableExtensions
    {
        public static T RequiredNotNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new InvalidStateException("Expecting value to be not null");
            }

            return obj;
        }
    }
}