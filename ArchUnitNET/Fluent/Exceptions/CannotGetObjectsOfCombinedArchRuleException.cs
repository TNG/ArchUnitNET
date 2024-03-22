//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;

namespace ArchUnitNET.Fluent.Exceptions
{
    public class CannotGetObjectsOfCombinedArchRuleException : Exception
    {
        public CannotGetObjectsOfCombinedArchRuleException(string message)
            : base(message) { }

        public CannotGetObjectsOfCombinedArchRuleException(string message, Exception inner)
            : base(message, inner) { }
    }
}
