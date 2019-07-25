/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;

namespace ArchUnitNET.ArchitectureExceptions
{
    public class TypeDoesNotExistInArchitecture : Exception
    {
        public TypeDoesNotExistInArchitecture(string message) : base(message)
        {
        }

        public TypeDoesNotExistInArchitecture(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}