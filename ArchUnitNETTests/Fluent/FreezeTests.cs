//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Freeze.FreezingArchRule;

namespace ArchUnitNETTests.Fluent
{
    public class FreezeTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(FreezeTests).Assembly).Build();

        private readonly IArchRule _failingRule = Types().That().Are(typeof(Violation)).Should().NotBePrivate();

        [Fact]
        public void PassFrozenRule()
        {
            Freeze(_failingRule).Check(Architecture);
        }

        private class Violation
        {
        }
    }
}