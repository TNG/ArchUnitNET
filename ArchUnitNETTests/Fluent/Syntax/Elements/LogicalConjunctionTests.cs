//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class LogicalConjunctionTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private static readonly Class ThisClass = Architecture.GetClassOfType(typeof(LogicalConjunctionTests));

        private static readonly Class OtherClass =
            Architecture.GetClassOfType(typeof(OtherClassForLogicalConjunctionTest));

        private static readonly string ThisClassName = ThisClass.Name;
        private static readonly string OtherClassName = OtherClass.Name;

        private static readonly IArchRule ThisClassExists =
            Classes().That().Are(ThisClass).Should().Exist();

        private static readonly IArchRule ThisClassDoesNotExist =
            Classes().That().Are(ThisClass).Should().NotExist();

        private static readonly IArchRule ThisCondition1 =
            Classes().That().Are(ThisClass).Should().Be(ThisClass);

        private static readonly IArchRule ThisCondition2 =
            Classes().That().HaveName(ThisClassName).Should().Be(ThisClass);

        private static readonly IArchRule OtherCondition1 =
            Classes().That().Are(OtherClass).Should().Be(OtherClass);

        private static readonly IArchRule OtherCondition2 =
            Classes().That().HaveName(OtherClassName).Should().Be(OtherClass);

        private static readonly IArchRule FalseThisCondition1 =
            Classes().That().AreNot(ThisClass).Should().Be(ThisClass);

        private static readonly IArchRule FalseThisCondition2 =
            Classes().That().DoNotHaveName(ThisClassName).Should().Be(ThisClass);

        private static readonly IArchRule ThisShouldCondition1 =
            Classes().That().Are(ThisClass).Should().Be(ThisClass);

        private static readonly IArchRule ThisShouldCondition2 =
            Classes().That().Are(ThisClass).Should().HaveName(ThisClassName);

        private static readonly IArchRule OtherShouldCondition1 =
            Classes().That().Are(OtherClass).Should().Be(OtherClass);

        private static readonly IArchRule OtherShouldCondition2 =
            Classes().That().Are(OtherClass).Should().HaveName(OtherClassName);

        private static readonly IArchRule FalseThisShouldCondition1 =
            Classes().That().Are(ThisClass).Should().NotBe(ThisClass);

        private static readonly IArchRule FalseThisShouldCondition2 =
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

            Assert.True(thisShouldCondition1AndThisShouldCondition2.HasNoViolations(Architecture));
            Assert.False(thisShouldCondition1AndOtherShouldCondition1.HasNoViolations(Architecture));
            Assert.False(otherCondition2AndThisCondition2.HasNoViolations(Architecture));
            Assert.False(falseThisCondition1AndFalseThisCondition2.HasNoViolations(Architecture));
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

            Assert.True(thisCondition1AndThisCondition2.HasNoViolations(Architecture));
            Assert.True(thisCondition1AndOtherCondition1.HasNoViolations(Architecture));
            Assert.True(otherCondition2AndThisCondition2.HasNoViolations(Architecture));
            Assert.True(falseThisCondition1AndFalseThisCondition2.HasNoViolations(Architecture));
        }

        [Fact]
        public void BasicRulesBehaveAsExpected()
        {
            Assert.True(ThisClassExists.HasNoViolations(Architecture));
            Assert.False(ThisClassDoesNotExist.HasNoViolations(Architecture));
            Assert.True(ThisCondition1.HasNoViolations(Architecture));
            Assert.True(ThisCondition2.HasNoViolations(Architecture));
            Assert.True(OtherCondition1.HasNoViolations(Architecture));
            Assert.True(OtherCondition2.HasNoViolations(Architecture));
            Assert.False(FalseThisCondition1.HasNoViolations(Architecture));
            Assert.False(FalseThisCondition2.HasNoViolations(Architecture));

            Assert.True(ThisShouldCondition1.HasNoViolations(Architecture));
            Assert.True(ThisShouldCondition2.HasNoViolations(Architecture));
            Assert.True(OtherShouldCondition1.HasNoViolations(Architecture));
            Assert.True(OtherShouldCondition2.HasNoViolations(Architecture));
            Assert.False(FalseThisShouldCondition1.HasNoViolations(Architecture));
            Assert.False(FalseThisShouldCondition2.HasNoViolations(Architecture));
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

            Assert.True(thisCondition1OrThisCondition2AndOtherCondition1.HasNoViolations(Architecture));
            Assert.True(thisCondition1AndOtherCondition1OrThisCondition2.HasNoViolations(Architecture));
            Assert.True(thisCondition1OrOtherCondition1AndOtherCondition2.HasNoViolations(Architecture));
            Assert.False(thisCondition1AndThisCondition2OrFalseThisCondition1.HasNoViolations(Architecture));
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

            Assert.True(
                thisShouldCondition1AndOtherShouldCondition1OrThisShouldCondition2.HasNoViolations(Architecture));
            Assert.True(
                otherShouldCondition1OrThisShouldCondition1AndThisShouldCondition2.HasNoViolations(Architecture));
            Assert.False(
                thisShouldCondition1OrThisShouldCondition2AndOtherShouldCondition1.HasNoViolations(Architecture));
            Assert.False(thisShouldExistAndShouldNotExistOrShouldNotExist.HasNoViolations(Architecture));
            Assert.False(thisShouldExistOrShouldNotExistAndShouldNotExist.HasNoViolations(Architecture));
            Assert.True(thisShouldExistAndShouldNotExistOrShouldExist.HasNoViolations(Architecture));
            Assert.True(
                thisShouldExistAndThisShouldCondition1OrOtherShouldCondition1OrShouldNotExist.HasNoViolations(
                    Architecture));
        }

        [Fact]
        public void CombinedArchRuleAndTest()
        {
            var thisClassExistsAndThisClassExists1 =
                ThisClassExists.And().Classes().That().Are(ThisClass).Should().Exist();
            var thisClassExistsAndThisClassDoesNotExist1 =
                ThisClassExists.And().Classes().That().Are(ThisClass).Should().NotExist();
            var thisClassDoesNotExistAndThisClassExists1 =
                ThisClassDoesNotExist.And().Classes().That().Are(ThisClass).Should().Exist();
            var thisClassDoesNotExistAndThisClassDoesNotExist1 =
                ThisClassDoesNotExist.And().Classes().That().Are(ThisClass).Should().NotExist();


            var thisClassExistsAndThisClassExists2 = ThisClassExists.And(ThisClassExists);
            var thisClassExistsAndThisClassDoesNotExist2 = ThisClassExists.And(ThisClassDoesNotExist);
            var thisClassDoesNotExistAndThisClassExists2 = ThisClassDoesNotExist.And(ThisClassExists);
            var thisClassDoesNotExistAndThisClassDoesNotExist2 = ThisClassDoesNotExist.And(ThisClassDoesNotExist);

            var thisClassExistsAndThisClassExistsAndThisClassExists =
                ThisClassExists.And(thisClassExistsAndThisClassExists2);
            var thisClassExistsAndThisClassExistsAndThisClassExistsAndThisClassExists =
                thisClassExistsAndThisClassExists2.And(thisClassExistsAndThisClassExists2);
            var thisClassExistsAndThisClassExistsAndThisClassDoesNotExist =
                ThisClassExists.And(thisClassExistsAndThisClassDoesNotExist2);
            var thisClassExistsAndThisClassDoesNotExistAndThisClassExists =
                thisClassExistsAndThisClassDoesNotExist2.And(ThisClassExists);
            var thisClassDoesNotExistAndThisClassExistsAndThisClassDoesNotExist =
                ThisClassDoesNotExist.And(thisClassExistsAndThisClassDoesNotExist2);

            Assert.True(thisClassExistsAndThisClassExists1.HasNoViolations(Architecture));
            Assert.False(thisClassExistsAndThisClassDoesNotExist1.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassExists1.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassDoesNotExist1.HasNoViolations(Architecture));

            Assert.True(thisClassExistsAndThisClassExists2.HasNoViolations(Architecture));
            Assert.False(thisClassExistsAndThisClassDoesNotExist2.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassExists2.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassDoesNotExist2.HasNoViolations(Architecture));

            Assert.True(thisClassExistsAndThisClassExistsAndThisClassExists.HasNoViolations(Architecture));
            Assert.True(
                thisClassExistsAndThisClassExistsAndThisClassExistsAndThisClassExists.HasNoViolations(Architecture));
            Assert.False(thisClassExistsAndThisClassExistsAndThisClassDoesNotExist.HasNoViolations(Architecture));
            Assert.False(thisClassExistsAndThisClassDoesNotExistAndThisClassExists.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassExistsAndThisClassDoesNotExist.HasNoViolations(Architecture));
        }

        [Fact]
        public void CombinedArchRuleCombinedAndOrTest()
        {
            var thisClassExistsAndThisClassExistsOrThisClassExists =
                ThisClassExists.And(Classes().That().Are(ThisClass).Should().Exist()).Or().Classes().That()
                    .Are(ThisClass).Should().Exist();
            var thisClassExistsOrThisClassDoesNotExistOrThisClassExistsAndThisClassDoesNotExist =
                ThisClassExists.Or(ThisClassDoesNotExist).Or(ThisClassExists).And(ThisClassDoesNotExist);
            var thisClassExistsOrThisClassDoesNotExistAndThisClassExists =
                ThisClassExists.Or(ThisClassDoesNotExist).And(ThisClassExists);
            var thisClassDoesNotExistAndThisClassExistsOrThisClassDoesNotExist =
                ThisClassDoesNotExist.And(ThisClassExists).Or(ThisClassDoesNotExist);

            var thisClassExistsOrThisClassDoesNotExistOrJoinedThisClassExistsAndThisClassDoesNotExist =
                ThisClassExists.Or(ThisClassDoesNotExist).Or(ThisClassExists.And(ThisClassDoesNotExist));
            var thisClassExistsOrJoinedThisClassDoesNotExistAndThisClassExists =
                ThisClassExists.Or(ThisClassDoesNotExist.And(ThisClassExists));

            Assert.True(thisClassExistsAndThisClassExistsOrThisClassExists.HasNoViolations(Architecture));
            Assert.False(
                thisClassExistsOrThisClassDoesNotExistOrThisClassExistsAndThisClassDoesNotExist.HasNoViolations(
                    Architecture));
            Assert.True(thisClassExistsOrThisClassDoesNotExistAndThisClassExists.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistAndThisClassExistsOrThisClassDoesNotExist.HasNoViolations(Architecture));

            Assert.True(
                thisClassExistsOrThisClassDoesNotExistOrJoinedThisClassExistsAndThisClassDoesNotExist.HasNoViolations(
                    Architecture));
            Assert.True(thisClassExistsOrJoinedThisClassDoesNotExistAndThisClassExists.HasNoViolations(Architecture));
        }

        [Fact]
        public void CombinedArchRuleOrTest()
        {
            var thisClassExistsOrThisClassExists1 =
                ThisClassExists.Or().Classes().That().Are(ThisClass).Should().Exist();
            var thisClassExistsOrThisClassDoesNotExist1 =
                ThisClassExists.Or().Classes().That().Are(ThisClass).Should().NotExist();
            var thisClassDoesNotExistOrThisClassExists1 =
                ThisClassDoesNotExist.Or().Classes().That().Are(ThisClass).Should().Exist();
            var thisClassDoesNotExistOrThisClassDoesNotExist1 =
                ThisClassDoesNotExist.Or().Classes().That().Are(ThisClass).Should().NotExist();


            var thisClassExistsOrThisClassExists2 = ThisClassExists.Or(ThisClassExists);
            var thisClassExistsOrThisClassDoesNotExist2 = ThisClassExists.Or(ThisClassDoesNotExist);
            var thisClassDoesNotExistOrThisClassExists2 = ThisClassDoesNotExist.Or(ThisClassExists);
            var thisClassDoesNotExistOrThisClassDoesNotExist2 = ThisClassDoesNotExist.Or(ThisClassDoesNotExist);

            var thisClassExistsOrThisClassExistsOrThisClassExists =
                ThisClassExists.Or(thisClassExistsOrThisClassExists2);
            var thisClassExistsOrThisClassExistsOrThisClassExistsOrThisClassExists =
                thisClassExistsOrThisClassExists2.Or(thisClassExistsOrThisClassExists2);
            var thisClassExistsOrThisClassExistsOrThisClassDoesNotExist =
                ThisClassExists.Or(thisClassExistsOrThisClassDoesNotExist2);
            var thisClassExistsOrThisClassDoesNotExistOrThisClassExists =
                thisClassExistsOrThisClassDoesNotExist2.Or(ThisClassExists);
            var thisClassDoesNotExistOrThisClassDoesNotExistOrThisClassDoesNotExist =
                ThisClassDoesNotExist.Or(thisClassDoesNotExistOrThisClassDoesNotExist2);

            Assert.True(thisClassExistsOrThisClassExists1.HasNoViolations(Architecture));
            Assert.True(thisClassExistsOrThisClassDoesNotExist1.HasNoViolations(Architecture));
            Assert.True(thisClassDoesNotExistOrThisClassExists1.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistOrThisClassDoesNotExist1.HasNoViolations(Architecture));

            Assert.True(thisClassExistsOrThisClassExists2.HasNoViolations(Architecture));
            Assert.True(thisClassExistsOrThisClassDoesNotExist2.HasNoViolations(Architecture));
            Assert.True(thisClassDoesNotExistOrThisClassExists2.HasNoViolations(Architecture));
            Assert.False(thisClassDoesNotExistOrThisClassDoesNotExist2.HasNoViolations(Architecture));

            Assert.True(thisClassExistsOrThisClassExistsOrThisClassExists.HasNoViolations(Architecture));
            Assert.True(
                thisClassExistsOrThisClassExistsOrThisClassExistsOrThisClassExists.HasNoViolations(Architecture));
            Assert.True(thisClassExistsOrThisClassExistsOrThisClassDoesNotExist.HasNoViolations(Architecture));
            Assert.True(thisClassExistsOrThisClassDoesNotExistOrThisClassExists.HasNoViolations(Architecture));
            Assert.False(
                thisClassDoesNotExistOrThisClassDoesNotExistOrThisClassDoesNotExist.HasNoViolations(Architecture));
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

            Assert.True(thisShouldCondition1OrThisShouldCondition2.HasNoViolations(Architecture));
            Assert.True(thisShouldCondition1OrOtherShouldCondition1.HasNoViolations(Architecture));
            Assert.True(otherCondition2OrThisCondition2.HasNoViolations(Architecture));
            Assert.False(falseThisCondition1OrFalseThisCondition2.HasNoViolations(Architecture));
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

            Assert.True(thisCondition1OrThisCondition2.HasNoViolations(Architecture));
            Assert.True(thisCondition1OrOtherCondition1.HasNoViolations(Architecture));
            Assert.True(otherCondition2OrThisCondition2.HasNoViolations(Architecture));
            Assert.True(falseThisCondition1OrFalseThisCondition2.HasNoViolations(Architecture));
        }
    }

    internal class OtherClassForLogicalConjunctionTest
    {
    }
}