//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using Xunit.Sdk;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void CycleDetectionTest()
        {
            SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(*)").Should().BeFreeOfCycles().Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
            Assert.True(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(*)").Should().BeFreeOfCycles().HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture));
            Assert.Throws<FailedArchRuleException>(() => SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(**)").Should().BeFreeOfCycles().Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture));
            Assert.False(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(**)").Should().BeFreeOfCycles().HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture));
            Assert.False(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(*)..").Should().BeFreeOfCycles().HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture));
            Assert.False(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(**)..").Should().BeFreeOfCycles().HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture));
        }

        [Fact]
        public void MatchingTest()
        {
            Assert.True(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(*)").GetSlices(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture).Count() == 3);
            Assert.True(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(**)").GetSlices(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture).Count() == 5);
            Assert.True(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(*)..").GetSlices(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture).Count() == 3);
            Assert.True(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.(**)..").GetSlices(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture).Count() == 3);
            Assert.False(SliceRuleDefinition.Slices().Matching("TestAssembly.Slices.Service.(*)").GetSlices(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture).Any());
        }
    }
}