using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace XUnitExtension
{
    public class ArchRuleTest
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(Architecture).Assembly).Build();

        [Fact]
        public void CheckArchRuleTest()
        {
            IArchRule trueArchRule = Classes().That().AreNotNested().Should().BePublic().OrShould().BeInternal()
                .Because("we said so.");
            IArchRule wrongArchRule1 = Classes().That().AreNotNested().Should().BePrivate().OrShould().BeInternal()
                .Because("we said so.");
            IArchRule wrongArchRule2 = Interfaces().Should().BeNested();
            IArchRule wrongExistArchRule = Classes().That().HaveName("asdasda").Should().Exist();
            IArchRule interfaceRule = Interfaces().Should().HaveNameStartingWith("I");

            Assert.CheckArchRule(Architecture, trueArchRule);
            Assert.CheckArchRule(Architecture, interfaceRule);
            Assert.CheckArchRule(Architecture, wrongExistArchRule);
            Assert.CheckArchRule(Architecture, wrongArchRule1.Or(wrongArchRule2));
        }
    }
}