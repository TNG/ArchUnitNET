//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class CustomSyntaxElementsTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNETTests"))
            .Build();

        private readonly Class _testClass;

        public CustomSyntaxElementsTests()
        {
            _testClass = Architecture.GetClassOfType(typeof(CustomRuleTestClass));
        }

        [Fact]
        public void CustomConditionTest()
        {
            var passingRule = Classes()
                .That()
                .Are(typeof(CustomRuleTestClass))
                .Should()
                .FollowCustomCondition(
                    cls => cls.FullName.Contains(nameof(CustomRuleTestClass)),
                    "passing custom condition",
                    "failed custom condition which should have passed"
                );
            var failingRule = Classes()
                .That()
                .Are(typeof(CustomRuleTestClass))
                .Should()
                .FollowCustomCondition(
                    cls => !cls.FullName.Contains(nameof(CustomRuleTestClass)),
                    "failing custom condition",
                    "failed custom condition which should have failed"
                );

            passingRule.Check(Architecture);
            Assert.False(failingRule.HasNoViolations(Architecture));
        }

        [Fact]
        public void CustomPredicateTest()
        {
            var predicateTestObjects = Classes()
                .That()
                .FollowCustomPredicate(
                    cls => cls.Name.Equals(nameof(CustomRuleTestClass)),
                    "custom predicate"
                )
                .GetObjects(Architecture);

            Assert.Single(predicateTestObjects);
            Assert.Equal(_testClass, predicateTestObjects.First());
        }
    }

    internal class CustomRuleTestClass { }
}
