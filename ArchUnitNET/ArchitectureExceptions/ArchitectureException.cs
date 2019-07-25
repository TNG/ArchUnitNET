/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;

namespace ArchUnitNET.ArchitectureExceptions
{
    public class ArchitectureException : Exception
    {
        public ArchitectureException(string firstFullName) : base(firstFullName)
        {
        }
    }
}