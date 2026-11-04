using System;
using System.IO;
using System.Xml.Linq;
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

        // Several capture groups, so the store keys its violations by slice names whose parts
        // are joined with " - ".
        private readonly IArchRule _frozenMultiGroupSliceRule = SliceRuleDefinition
            .Slices()
            .Matching("SlicesTestAssembly.MultipleSubnamespaces.(*).(*)")
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
            Freeze(_frozenMultiGroupSliceRule)
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
        }

        [Fact]
        public void PassFrozenRulesUsingXmlViolationStore()
        {
            Freeze(_frozenRule, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenRule2, new XmlViolationStore()).Check(Architecture);
            Freeze(_frozenSliceRule, new XmlViolationStore())
                .Check(StaticTestArchitectures.SlicesTestArchitecture);
            Freeze(_frozenMultiGroupSliceRule, new XmlViolationStore())
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

        [Fact]
        public void XmlViolationStoreWritesToCustomPath()
        {
            using (var dir = new TempStoreDirectory())
            {
                var path = dir.PathFor("FrozenRules.xml");

                Freeze(_frozenRule, new XmlViolationStore(path)).Check(Architecture);

                Assert.True(File.Exists(path));
                Assert.Contains(
                    XDocument.Load(path).Root.Elements(),
                    element => element.Attribute("ArchRule")?.Value == _frozenRule.Description
                );
            }
        }

        [Fact]
        public void FreezeThrowsOnMalformedJsonStoreAndLeavesFileUntouched()
        {
            using (var dir = new TempStoreDirectory())
            {
                const string content = "{ this is not valid json";
                var path = dir.CreateFile("FrozenRules.json", content);

                Assert.ThrowsAny<Exception>(() =>
                    Freeze(_frozenRule, new JsonViolationStore(path)).Check(Architecture)
                );

                Assert.Equal(content, File.ReadAllText(path));
            }
        }

        [Fact]
        public void FreezeThrowsOnMalformedXmlStoreAndLeavesFileUntouched()
        {
            using (var dir = new TempStoreDirectory())
            {
                const string content = "<FrozenRules><FrozenRule";
                var path = dir.CreateFile("FrozenRules.xml", content);

                Assert.ThrowsAny<Exception>(() =>
                    Freeze(_frozenRule, new XmlViolationStore(path)).Check(Architecture)
                );

                Assert.Equal(content, File.ReadAllText(path));
            }
        }

        [Fact]
        public void FreezeCreatesStoreInMissingDirectory()
        {
            using (var dir = new TempStoreDirectory())
            {
                var jsonPath = dir.PathFor(Path.Combine("json", "FrozenRules.json"));
                var xmlPath = dir.PathFor(Path.Combine("xml", "FrozenRules.xml"));

                Freeze(_frozenRule, new JsonViolationStore(jsonPath)).Check(Architecture);
                Freeze(_frozenRule, new XmlViolationStore(xmlPath)).Check(Architecture);

                Assert.True(File.Exists(jsonPath));
                Assert.True(File.Exists(xmlPath));
            }
        }

        private class Violation { }

        private class Violation2 { }
    }
}
