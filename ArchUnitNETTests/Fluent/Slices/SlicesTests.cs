//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.xUnit;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    public class SlicesTests
    {
        [Fact]
        public void CycleDetectionTest()
        {
            Assert.Throws<FailedArchRuleException>(
                () =>
                    SliceRuleDefinition
                        .Slices()
                        .Matching("TestAssembly.Slices.(**)")
                        .Should()
                        .BeFreeOfCycles()
                        .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .Should()
                    .BeFreeOfCycles()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
        }

        [Fact]
        public void MatchingTest()
        {
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                7,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)..")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                3,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.Equal(
                2,
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Slice3.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count()
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Service.(*)")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Any()
            );
        }

        [Fact]
        public void MatchingRegexTest()
        {
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .MatchingRegex(@"(?:[^\.]*\.){2}([^\.]+).*", true)
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .ToList().Count > 5
            );

            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .MatchingRegex(@"TestAssembly\.Slices\.([^\.]+).*", true)
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .ToList()
                    .Select(slice => slice.Identifier.Identifier)
                    .Select(identifier => identifier.Contains("."))
                    .Aggregate(false, (a, b) => a||b)
            );

            Assert.DoesNotContain("TestAssembly.Slices.Slice1", 
                SliceRuleDefinition
                    .Slices()
                    .MatchingRegex(@"TestAssembly\.Slices\.(?!Slice1)([^\.]+).*")
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .ToList()
                    .Select(slice => slice.Identifier.Identifier)
            );
        }

        [Fact]
        public void WhereTest()
        {
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Slice3.(*)")
                    .Where(slice => slice.Identifier.Identifier.Contains("Group1"))
                    .Where(slice => true)
                    .GetObjects(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
                    .Count() == 1
            );

            SliceRuleDefinition
                .Slices()
                .Matching("TestAssembly.Slices.(**)")
                .Where(slice => !slice.Identifier.Identifier.Contains("Slice3"))
                .Where(slice => !slice.Identifier.Identifier.Contains("Slice2"))
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);

            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Where(slice => !slice.Identifier.Identifier.Contains("Slice3"))
                    .Where(slice => !slice.Identifier.Identifier.Contains("Slice2"))
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
        }

        [Fact]
        public void NotDependOnEachOtherTest()
        {
            SliceRuleDefinition
                .Slices()
                .Matching("TestAssembly.Slices.Slice3.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
            SliceRuleDefinition
                .Slices()
                .Matching("TestAssembly.Slices.Slice1.(*)")
                .Should()
                .NotDependOnEachOther()
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
            Assert.True(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.Slice1.(*)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.Throws<FailedArchRuleException>(
                () =>
                    SliceRuleDefinition
                        .Slices()
                        .Matching("TestAssembly.Slices.(**)")
                        .Should()
                        .NotDependOnEachOther()
                        .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(*)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
            Assert.False(
                SliceRuleDefinition
                    .Slices()
                    .Matching("TestAssembly.Slices.(**)..")
                    .Should()
                    .NotDependOnEachOther()
                    .HasNoViolations(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
            );
        }
    }
}
