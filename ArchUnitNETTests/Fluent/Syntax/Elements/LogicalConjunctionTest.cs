using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class LogicalConjunctionTest
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private static readonly Class ThisClass = Architecture.GetClassOfType(typeof(LogicalConjunctionTest));

        private static readonly Class OtherClass =
            Architecture.GetClassOfType(typeof(OtherClassForLogicalConjunctionTest));

        private static readonly string ThisClassName = ThisClass.Name;
        private static readonly string OtherClassName = OtherClass.Name;

        private static readonly ArchRule<Class> ThisClassExists =
            Classes().That().Are(ThisClass).Should().Exist();

        private static readonly ArchRule<Class> ThisClassDoesNotExist =
            Classes().That().Are(ThisClass).Should().NotExist();

        private static readonly ArchRule<Class> ThisCondition1 =
            Classes().That().Are(ThisClass).Should().Be(ThisClass);

        private static readonly ArchRule<Class> ThisCondition2 =
            Classes().That().HaveName(ThisClassName).Should().Be(ThisClass);

        private static readonly ArchRule<Class> OtherCondition1 =
            Classes().That().Are(OtherClass).Should().Be(OtherClass);

        private static readonly ArchRule<Class> OtherCondition2 =
            Classes().That().HaveName(OtherClassName).Should().Be(OtherClass);

        private static readonly ArchRule<Class> FalseThisCondition1 =
            Classes().That().AreNot(ThisClass).Should().Be(ThisClass);

        private static readonly ArchRule<Class> FalseThisCondition2 =
            Classes().That().DoNotHaveName(ThisClassName).Should().Be(ThisClass);

        private static readonly ArchRule<Class> ThisShouldCondition1 =
            Classes().That().Are(ThisClass).Should().Be(ThisClass);

        private static readonly ArchRule<Class> ThisShouldCondition2 =
            Classes().That().Are(ThisClass).Should().HaveName(ThisClassName);

        private static readonly ArchRule<Class> OtherShouldCondition1 =
            Classes().That().Are(OtherClass).Should().Be(OtherClass);

        private static readonly ArchRule<Class> OtherShouldCondition2 =
            Classes().That().Are(OtherClass).Should().HaveName(OtherClassName);

        private static readonly ArchRule<Class> FalseThisShouldCondition1 =
            Classes().That().Are(ThisClass).Should().NotBe(ThisClass);

        private static readonly ArchRule<Class> FalseThisShouldCondition2 =
            Classes().That().Are(ThisClass).Should().NotHaveName(ThisClassName);

        [Fact]
        public void AndShouldTest()
        {
            var thisShouldCondition1AndThisShouldCondition2 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).AndShould().HaveName(ThisClassName);
            var thisShouldCondition1AndOtherShouldCondition1 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).AndShould().Be(OtherClass);
            var otherCondition2AndThisCondition2 =
                Classes().That().Are(ThisClass).Should().HaveName(OtherClassName).AndShould().HaveName(ThisClassName);
            var falseThisCondition1AndFalseThisCondition2 =
                Classes().That().Are(ThisClass).Should().NotBe(ThisClass).AndShould().NotHaveName(ThisClassName);

            Assert.True(thisShouldCondition1AndThisShouldCondition2.Check(Architecture));
            Assert.False(thisShouldCondition1AndOtherShouldCondition1.Check(Architecture));
            Assert.False(otherCondition2AndThisCondition2.Check(Architecture));
            Assert.False(falseThisCondition1AndFalseThisCondition2.Check(Architecture));
        }

        [Fact]
        public void AndTest()
        {
            var thisCondition1AndThisCondition2 =
                Classes().That().Are(ThisClass).And().HaveName(ThisClassName).Should().Be(ThisClass);
            var thisCondition1AndOtherCondition1 =
                Classes().That().Are(ThisClass).And().Are(OtherClass).Should().NotExist();
            var otherCondition2AndThisCondition2 =
                Classes().That().HaveName(OtherClassName).And().HaveName(ThisClassName).Should().NotExist();
            var falseThisCondition1AndFalseThisCondition2 =
                Classes().That().AreNot(ThisClass).And().DoNotHaveName(ThisClassName).Should().NotBe(ThisClass);

            Assert.True(thisCondition1AndThisCondition2.Check(Architecture));
            Assert.True(thisCondition1AndOtherCondition1.Check(Architecture));
            Assert.True(otherCondition2AndThisCondition2.Check(Architecture));
            Assert.True(falseThisCondition1AndFalseThisCondition2.Check(Architecture));
        }

        [Fact]
        public void BasicRulesBehaveAsExpected()
        {
            Assert.True(ThisClassExists.Check(Architecture));
            Assert.False(ThisClassDoesNotExist.Check(Architecture));
            Assert.True(ThisCondition1.Check(Architecture));
            Assert.True(ThisCondition2.Check(Architecture));
            Assert.True(OtherCondition1.Check(Architecture));
            Assert.True(OtherCondition2.Check(Architecture));
            Assert.False(FalseThisCondition1.Check(Architecture));
            Assert.False(FalseThisCondition2.Check(Architecture));

            Assert.True(ThisShouldCondition1.Check(Architecture));
            Assert.True(ThisShouldCondition2.Check(Architecture));
            Assert.True(OtherShouldCondition1.Check(Architecture));
            Assert.True(OtherShouldCondition2.Check(Architecture));
            Assert.False(FalseThisShouldCondition1.Check(Architecture));
            Assert.False(FalseThisShouldCondition2.Check(Architecture));
        }

        [Fact]
        public void CombinedAndOrTest()
        {
            var thisCondition1OrThisCondition2AndOtherCondition1 =
                Classes().That().Are(ThisClass).Or().HaveName(ThisClassName).And().Are(OtherClass).Should().NotExist();
            var thisCondition1AndOtherCondition1OrThisCondition2 =
                Classes().That().Are(ThisClass).And().Are(OtherClass).Or().HaveName(ThisClassName).Should().Exist();
            var thisCondition1OrOtherCondition1AndOtherCondition2 =
                Classes().That().Are(ThisClass).Or().Are(OtherClass).And().HaveName(OtherClassName).Should()
                    .Be(OtherClass);
            var thisCondition1AndThisCondition2OrFalseThisCondition1 =
                Classes().That().Are(ThisClass).And().HaveName(ThisClassName).Or().AreNot(ThisClass).Should()
                    .Be(ThisClass);

            Assert.True(thisCondition1OrThisCondition2AndOtherCondition1.Check(Architecture));
            Assert.True(thisCondition1AndOtherCondition1OrThisCondition2.Check(Architecture));
            Assert.True(thisCondition1OrOtherCondition1AndOtherCondition2.Check(Architecture));
            Assert.False(thisCondition1AndThisCondition2OrFalseThisCondition1.Check(Architecture));
        }

        [Fact]
        public void CombinedAndShouldOrShouldTest()
        {
            var thisShouldCondition1AndOtherShouldCondition1OrThisShouldCondition2 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).AndShould().Be(OtherClass).OrShould()
                    .HaveName(ThisClassName);
            var otherShouldCondition1OrThisShouldCondition1AndThisShouldCondition2 =
                Classes().That().Are(ThisClass).Should().Be(OtherClass).OrShould().Be(ThisClass).AndShould()
                    .HaveName(ThisClassName);
            var thisShouldCondition1OrThisShouldCondition2AndOtherShouldCondition1 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).OrShould().HaveName(ThisClassName).AndShould()
                    .Be(OtherClass);
            var thisShouldExistAndShouldNotExistOrShouldNotExist =
                Classes().That().Are(ThisClass).Should().Exist().AndShould().NotExist().OrShould().NotExist();
            var thisShouldExistOrShouldNotExistAndShouldNotExist =
                Classes().That().Are(ThisClass).Should().Exist().OrShould().NotExist().AndShould().NotExist();
            var thisShouldExistAndShouldNotExistOrShouldExist =
                Classes().That().Are(ThisClass).Should().Exist().AndShould().NotExist().OrShould().Exist();
            var thisShouldExistAndThisShouldCondition1OrOtherShouldCondition1OrShouldNotExist =
                Classes().That().Are(ThisClass).Should().Exist().AndShould().Be(ThisClass).OrShould().Be(OtherClass)
                    .OrShould().NotExist();

            Assert.True(thisShouldCondition1AndOtherShouldCondition1OrThisShouldCondition2.Check(Architecture));
            Assert.True(otherShouldCondition1OrThisShouldCondition1AndThisShouldCondition2.Check(Architecture));
            Assert.False(thisShouldCondition1OrThisShouldCondition2AndOtherShouldCondition1.Check(Architecture));
            Assert.False(thisShouldExistAndShouldNotExistOrShouldNotExist.Check(Architecture));
            Assert.False(thisShouldExistOrShouldNotExistAndShouldNotExist.Check(Architecture));
            Assert.True(thisShouldExistAndShouldNotExistOrShouldExist.Check(Architecture));
            Assert.True(
                thisShouldExistAndThisShouldCondition1OrOtherShouldCondition1OrShouldNotExist.Check(Architecture));
        }

        [Fact]
        public void OrShouldTest()
        {
            var thisShouldCondition1OrThisShouldCondition2 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).OrShould().HaveName(ThisClassName);
            var thisShouldCondition1OrOtherShouldCondition1 =
                Classes().That().Are(ThisClass).Should().Be(ThisClass).OrShould().Be(OtherClass);
            var otherCondition2OrThisCondition2 =
                Classes().That().Are(ThisClass).Should().HaveName(OtherClassName).OrShould().HaveName(ThisClassName);
            var falseThisCondition1OrFalseThisCondition2 =
                Classes().That().Are(ThisClass).Should().NotBe(ThisClass).OrShould().NotHaveName(ThisClassName);

            Assert.True(thisShouldCondition1OrThisShouldCondition2.Check(Architecture));
            Assert.True(thisShouldCondition1OrOtherShouldCondition1.Check(Architecture));
            Assert.True(otherCondition2OrThisCondition2.Check(Architecture));
            Assert.False(falseThisCondition1OrFalseThisCondition2.Check(Architecture));
        }

        [Fact]
        public void OrTest()
        {
            var thisCondition1OrThisCondition2 =
                Classes().That().Are(ThisClass).Or().HaveName(ThisClassName).Should().Be(ThisClass);
            var thisCondition1OrOtherCondition1 =
                Classes().That().Are(ThisClass).Or().Are(OtherClass).Should().Exist();
            var otherCondition2OrThisCondition2 =
                Classes().That().HaveName(OtherClassName).Or().HaveName(ThisClassName).Should().Exist();
            var falseThisCondition1OrFalseThisCondition2 =
                Classes().That().AreNot(ThisClass).Or().DoNotHaveName(ThisClassName).Should().NotBe(ThisClass);

            Assert.True(thisCondition1OrThisCondition2.Check(Architecture));
            Assert.True(thisCondition1OrOtherCondition1.Check(Architecture));
            Assert.True(otherCondition2OrThisCondition2.Check(Architecture));
            Assert.True(falseThisCondition1OrFalseThisCondition2.Check(Architecture));
        }
    }

    internal class OtherClassForLogicalConjunctionTest
    {
    }
}