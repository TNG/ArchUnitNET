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
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Attribute = System.Attribute;

namespace ArchUnitNETTests.Fluent
{
    public class MultipleConditionRulesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNETTests"))
            .Build();

        private readonly Class _failingClass;

        private readonly Class _passingClass1;
        private readonly Class _passingClass2;

        public MultipleConditionRulesTests()
        {
            _passingClass1 = Architecture.GetClassOfType(typeof(PassingClass1));
            _passingClass2 = Architecture.GetClassOfType(typeof(PassingClass2));
            _failingClass = Architecture.GetClassOfType(typeof(FailingClass));
        }

        [Fact]
        public void AssignCorrectTypeToEvaluationResults()
        {
            var evaluationResults = Classes()
                .That()
                .Are(typeof(PassingClass1), typeof(FailingClass), typeof(PassingClass2))
                .And()
                .AreNotPrivate()
                .Should()
                .HaveAnyAttributes(typeof(Test1))
                .AndShould()
                .HaveAnyAttributes(typeof(Test2))
                .Evaluate(Architecture)
                .ToList();

            var passedObjects = evaluationResults
                .Where(result => result.Passed)
                .Select(result => result.EvaluatedObject)
                .ToList();
            var failedObjects = evaluationResults
                .Where(result => !result.Passed)
                .Select(result => result.EvaluatedObject)
                .ToList();

            Assert.Equal(2, passedObjects.Count);
            Assert.Single(failedObjects);

            Assert.Contains(_passingClass1, passedObjects);
            Assert.Contains(_passingClass2, passedObjects);
            Assert.Contains(_failingClass, failedObjects);
        }
    }

    [Test1]
    [Test2]
    internal class PassingClass1 { }

    [Test1]
    internal class FailingClass { }

    [Test1]
    [Test2]
    internal class PassingClass2 { }

    internal class Test1 : Attribute { }

    internal class Test2 : Attribute { }
}
