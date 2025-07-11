extern alias LoaderTestAssemblyAlias;
extern alias OtherLoaderTestAssemblyAlias;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Freeze;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Freeze.FreezingArchRule;

using DuplicateClass = LoaderTestAssemblyAlias::DuplicateClassAcrossAssemblies.DuplicateClass;
using OtherDuplicateClass = OtherLoaderTestAssemblyAlias::DuplicateClassAcrossAssemblies.DuplicateClass;

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
            .Matching("TestAssembly.Slices.(**)")
            .Should()
            .NotDependOnEachOther();

        private readonly IArchRule _failingFrozenSliceRule = SliceRuleDefinition
            .Slices()
            .Matching("TestAssembly.Slices.(*)..")
            .Should()
            .NotDependOnEachOther();

        [Fact]
        public void PassFrozenRules()
        {
            Freeze(_frozenRule).Check(Architecture);
            Freeze(_frozenRule2).Check(Architecture);
            Freeze(_frozenSliceRule)
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
        }

        [Fact]
        public void PassFrozenRulesUsingXmlViolationStore()
        {
            Freeze(_frozenRule, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenRule2, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenSliceRule, new XmlViolationStore())
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
        }

        [Fact]
        public void FailFrozenRule()
        {
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenRule).Check(Architecture)
            );
            Assert.Throws<FailedArchRuleException>(() =>
                Freeze(_failingFrozenSliceRule)
                    .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
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
                    .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture)
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
                .Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
        }

        [Fact]
        public void FrozenRuleWithTwoTypesWithSameFullName()
        {
            var rule =Types().That().Are(typeof(DuplicateClass), typeof(OtherDuplicateClass)).Should()
                .NotHaveName("DuplicateClass");
            Freeze(rule, new JsonViolationStore()).Check(StaticTestArchitectures.LoaderTestArchitecture);
            Freeze(rule, new XmlViolationStore()).Check(StaticTestArchitectures.LoaderTestArchitecture);
        }

        private class Violation { }

        private class Violation2 { }
    }
}
