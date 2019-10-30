//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.NUnit;
using NUnit.Framework;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNET.NUnitTests
{
    public class RuleEvaluationTests
    {
        private Architecture _architecture;
        private string _expectedErrorMessage;
        private IArchRule _falseRule;
        private IArchRule _trueRule;

        [SetUp]
        public void Setup()
        {
            _architecture = new ArchLoader().LoadAssemblies(typeof(RuleEvaluationTests).Assembly).Build();
            _trueRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().Exist();
            _falseRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().NotExist();
            _expectedErrorMessage = _falseRule.Evaluate(_architecture).ToErrorMessage();
        }

        [Test]
        public void ArchRuleAssertTest()
        {
            ArchRuleAssert.FulfilsRule(_architecture, _trueRule);
            Assert.Throws<AssertionException>(() => ArchRuleAssert.FulfilsRule(_architecture, _falseRule));
            Assert.AreEqual(_expectedErrorMessage,
                Assert.Catch<AssertionException>(() => ArchRuleAssert.FulfilsRule(_architecture, _falseRule)).Message);
        }

        [Test]
        public void ArchRuleExtensionsTest()
        {
            _architecture.CheckRule(_trueRule);
            _trueRule.Check(_architecture);
            Assert.Throws<AssertionException>(() => _architecture.CheckRule(_falseRule));
            Assert.Throws<AssertionException>(() => _falseRule.Check(_architecture));
            Assert.AreEqual(_expectedErrorMessage,
                Assert.Catch<AssertionException>(() => _architecture.CheckRule(_falseRule)).Message);
            Assert.AreEqual(_expectedErrorMessage,
                Assert.Catch<AssertionException>(() => _falseRule.Check(_architecture)).Message);
        }
    }
}