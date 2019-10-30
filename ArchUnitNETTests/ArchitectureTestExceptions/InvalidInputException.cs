//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;

namespace ArchUnitNETTests.ArchitectureTestExceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }

        public InvalidInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}