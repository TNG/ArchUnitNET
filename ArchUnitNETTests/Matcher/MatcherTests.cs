//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Matcher;
using Xunit;

namespace ArchUnitNETTests.Matcher
{
    public class MatcherTests
    {
        private readonly List<string> _misMatches = new List<string>
        {
            "ApplePie", "AppleSauce", "AppleStrudel",
            "BananaBread"
        };

        private readonly List<string> _matches = new List<string> {"ApplePie", "AppleSauce", "AppleStrudel"};

        private static bool ContainsApple(string food)
        {
            return food.Contains("Apple");
        }

        [Fact]
        public void ThrowsProperExceptionWithNoMatch()
        {
            _matches.ShouldAll(ContainsApple);
            Assert.Throws<ArchitectureException>(() => _misMatches.ShouldAll(ContainsApple));
        }
    }
}