/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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