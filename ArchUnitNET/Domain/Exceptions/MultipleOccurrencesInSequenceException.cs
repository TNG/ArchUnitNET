//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;

namespace ArchUnitNET.Domain.Exceptions
{
    public class MultipleOccurrencesInSequenceException : Exception
    {
        public MultipleOccurrencesInSequenceException(string message) : base(message)
        {
        }
    }
}