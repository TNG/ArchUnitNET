//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;

namespace ArchUnitNET.Core
{
    internal static class RegistryUtils
    {
        public static T GetFromDictOrCreateAndAdd<T, TK>(TK key, Dictionary<TK, T> dict, Func<TK, T> createFunc)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }

            value = createFunc(key);
            dict.Add(key, value);

            return value;
        }
    }
}