using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Freeze;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Freeze.FreezingArchRule;

namespace ArchUnitNETTests.Fluent
{
    public class FreezeTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(FreezeTests).Assembly)
            .Build();

        private readonly IArchRule _frozenRule = Types()
            .That()
            .Are(typeof(Violation), typeof(Violation2))
            .Should()
            .NotBePrivate();

        private readonly IArchRule _frozenRule2 = Types()
            .That()
            .Are(typeof(Violation))
            .Should()
            .BeProtected();

        private readonly IArchRule _failingFrozenRule = Types()
            .That()
            .Are(typeof(Violation), typeof(Violation2))
            .Should()
            .BePublic();

        private readonly IArchRule _frozenSliceRule = SliceRuleDefinition
            .Slices()
            .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
            .Should()
            .NotDependOnEachOther();

        private readonly IArchRule _failingFrozenSliceRule = SliceRuleDefinition
            .Slices()
            .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*)..")
            .Should()
            .NotDependOnEachOther();

        [Fact]
        public void PassFrozenRules()
        {
            Freeze(_frozenRule).Check(Architecture);
            Freeze(_frozenRule2).Check(Architecture);
            Freeze(_frozenSliceRule).Check(StaticTestArchitectures.SlicesTestArchitecture);
        }

        [Fact]
        public void PassFrozenRulesUsingXmlViolationStore()
        {
            Freeze(_frozenRule, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenRule2, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenSliceRule, new XmlViolationStore())
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
        }

        [Fact]
        public void FailFrozenRule()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenRule).Check(Architecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenSliceRule)
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        [Fact]
        public void FailFrozenRuleUsingXmlViolationStore()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenRule, new XmlViolationStore()).Check(Architecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenSliceRule, new XmlViolationStore())
                    .Check(StaticTestArchitectures.SlicesTestArchitecture)
            );
        }

        [Fact]
        public void PassFrozenRulesWithCustomViolationStorePath()
        {
            Freeze(_frozenRule, "../../../ArchUnitNET/Storage/CustomPathFrozenRules.json")
                .Check(Architecture);
            Freeze(_frozenRule2, "../../../ArchUnitNET/Storage/CustomPathFrozenRules.json")
                .Check(Architecture);
            Freeze(_frozenSliceRule, "../../../ArchUnitNET/Storage/CustomPathFrozenRules.json")
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
        }

        private class Violation { }

        private class Violation2 { }
    }
}
