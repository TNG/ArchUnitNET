//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Fluent.Exceptions;

namespace ArchUnitNET.Fluent.Extensions
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