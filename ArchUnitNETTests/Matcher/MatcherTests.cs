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

using System.Collections.Generic;
using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Matcher;
using Xunit;

namespace ArchUnitNETTests.Matcher
{
    public class MatcherTests
    {
        private readonly List<string> _misMatches = new List<string>{"ApplePie", "AppleSauce", "AppleStrudel",
            "BananaBread"};
        private readonly List<string> _matches = new List<string>{"ApplePie", "AppleSauce", "AppleStrudel"};

        [Fact]
        public void ThrowsProperExceptionWithNoMatch()
        {
            _matches.ShouldAll(ContainsApple);
            Assert.Throws<ArchitectureException>(() => _misMatches.ShouldAll(ContainsApple));
        }

        private static bool ContainsApple(string food)
        {
            return food.Contains("Apple");
        }
    }
}