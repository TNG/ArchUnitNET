/*
 * Copyright 2019 TNG Technology Consulting GmbH
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