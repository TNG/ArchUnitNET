using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.TUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNET.TUnitTests
{
    public class RuleEvaluationTests
    {
        private readonly Architecture _architecture;
        private readonly string _expectedErrorMessage;
        private readonly IArchRule _falseRule;
        private readonly IArchRule _trueRule;

        public RuleEvaluationTests()
        {
            _architecture = new ArchLoader()
                .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNET.TUnitTests"))
                .Build();
            _trueRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().Exist();
            _falseRule = Classes().That().Are(typeof(RuleEvaluationTests)).Should().NotExist();
            _expectedErrorMessage = _falseRule.Evaluate(_architecture).ToErrorMessage();
        }

        [Test]
        public async Task ArchRuleAssertTest()
        {
            ArchRuleAssert.CheckRule(_architecture, _trueRule);
            var ex = Assert.Throws<FailedArchRuleException>(() =>
                ArchRuleAssert.CheckRule(_architecture, _falseRule)
            );
            await Assert.That(ex.Message).IsEqualTo(_expectedErrorMessage);
        }

        [Test]
        public async Task ArchRuleExtensionsTest()
        {
            _architecture.CheckRule(_trueRule);
            _trueRule.Check(_architecture);
            var ex1 = Assert.Throws<FailedArchRuleException>(() =>
                _architecture.CheckRule(_falseRule)
            );
            var ex2 = Assert.Throws<FailedArchRuleException>(() => _falseRule.Check(_architecture));
            await Assert.That(ex1.Message).IsEqualTo(_expectedErrorMessage);
            await Assert.That(ex2.Message).IsEqualTo(_expectedErrorMessage);
        }
    }
}
