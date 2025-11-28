using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.MSTestV2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNET.MSTestV3Tests
{
    [TestClass]
    public class RuleEvaluationTests
    {
        private static Architecture _architecture;
        private static string _expectedErrorMessage;
        private static IArchRule _falseRule;
        private static IArchRule _trueRule;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _architecture = new ArchLoader()
                .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNET.MSTestV3Tests"))
                .Build();
            _trueRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().Exist();
            _falseRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().NotExist();
            _expectedErrorMessage = _falseRule.Evaluate(_architecture).ToErrorMessage();
        }

        [TestMethod]
        public void ArchRuleAssertTest()
        {
            ArchRuleAssert.FulfilsRule(_architecture, _trueRule);
            Assert.ThrowsExactly<AssertFailedException>(() =>
                ArchRuleAssert.FulfilsRule(_architecture, _falseRule)
            );
            Assert.AreEqual(
                _expectedErrorMessage,
                RemoveAssertionText(
                    Assert
                        .ThrowsExactly<AssertFailedException>(() =>
                            ArchRuleAssert.FulfilsRule(_architecture, _falseRule)
                        )
                        .Message
                )
            );
        }

        [TestMethod]
        public void ArchRuleExtensionsTest()
        {
            _architecture.CheckRule(_trueRule);
            _trueRule.Check(_architecture);
            Assert.ThrowsExactly<AssertFailedException>(() => _architecture.CheckRule(_falseRule));
            Assert.ThrowsExactly<AssertFailedException>(() => _falseRule.Check(_architecture));
            Assert.AreEqual(
                _expectedErrorMessage,
                RemoveAssertionText(
                    Assert
                        .ThrowsExactly<AssertFailedException>(() =>
                            _architecture.CheckRule(_falseRule)
                        )
                        .Message
                )
            );
            Assert.AreEqual(
                _expectedErrorMessage,
                RemoveAssertionText(
                    Assert
                        .ThrowsExactly<AssertFailedException>(() => _falseRule.Check(_architecture))
                        .Message
                )
            );
        }

        private static string RemoveAssertionText(string exceptionMessage)
        {
            return exceptionMessage.Replace("Assert.Fail failed. ", string.Empty);
        }
    }
}
