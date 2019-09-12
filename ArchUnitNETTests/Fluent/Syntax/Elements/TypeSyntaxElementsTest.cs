using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class TypeSyntaxElementsTest
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        [Fact]
        public void ImplementInterfacesStartingWithSameNameTest()
        {
            //TODO fix this
            var interfaceImplementsWrongInterface =
                Interfaces().That().Are(InheritedFromTestInterface12).Should().ImplementInterface(TestInterface1);

            Assert.False(interfaceImplementsWrongInterface.Check(Architecture));
            //this fails because ITestInterface1's full name starts identical as ITestInterface12's full name and ImplementInterface() only compares the patterns of the full names
            //Equals() method of Generic Types has to be reworked such that implemented instances of generics are equal to the generic version (maybe not possible, then try to find other solution)
            //the implemented version is missing the generic argument at the end of the name and therefore is considered not equal
        }


        [Fact]
        public void ImplementInterfaceTest()
        {
            foreach (var intf in Architecture.Interfaces)
            {
                var typesThatImplementInterfaceImplementInterface = Types().That().ImplementInterface(intf)
                    .Should().ImplementInterface(intf.FullName);
                var typesThatImplementInterfaceDoNotImplementInterface = Types().That()
                    .ImplementInterface(intf.FullName).Should().NotImplementInterface(intf).AndShould().Exist();
                var typesThatDoNotImplementInterfaceImplementInterface = Types().That().DoNotImplementInterface(intf)
                    .Should().ImplementInterface(intf.FullName).AndShould().Exist();
                var typesThatDoNotImplementInterfaceDoNotImplementInterface = Types().That()
                    .DoNotImplementInterface(intf.FullName).Should().NotImplementInterface(intf);

                Assert.True(typesThatImplementInterfaceImplementInterface.Check(Architecture));
                Assert.False(typesThatImplementInterfaceDoNotImplementInterface.Check(Architecture));
                Assert.False(typesThatDoNotImplementInterfaceImplementInterface.Check(Architecture));
                Assert.True(typesThatDoNotImplementInterfaceDoNotImplementInterface.Check(Architecture));
            }

            var testClassThatImplementsInterfaceImplementsInterface = Classes().That()
                .Are(InheritedType).Should().ImplementInterface(InheritedTestInterface);
            var testClassThatImplementsOtherInterfaceImplementsInterfaces = Types().That()
                .Are(InheritedType).Should().ImplementInterface(InheritedTestInterface).AndShould()
                .ImplementInterface(InheritingInterface);
            var testInterfaceThatImplementsInterfaceImplementsInterface = Interfaces().That()
                .Are(InheritingInterface).Should().ImplementInterface(InheritedTestInterface);
            var testClassThatImplementsNoInterfaceDoesNotImplementInterface = Interfaces().That()
                .Are(PublicTestClass).Should().NotImplementInterface(InheritedTestInterface);
            var testClassThatImplementsNoInterfaceImplementsInterface = Interfaces().That()
                .Are(PublicTestClass).Should().ImplementInterface(InheritedTestInterface).AndShould().Exist();

            Assert.True(testClassThatImplementsInterfaceImplementsInterface.Check(Architecture));
            Assert.True(testClassThatImplementsOtherInterfaceImplementsInterfaces.Check(Architecture));
            Assert.True(testInterfaceThatImplementsInterfaceImplementsInterface.Check(Architecture));
            Assert.True(testClassThatImplementsNoInterfaceDoesNotImplementInterface.Check(Architecture));
            Assert.False(testClassThatImplementsNoInterfaceImplementsInterface.Check(Architecture));
        }


        [Fact]
        public void TypesThatAreNotNestedMustBeVisible()
        {
            var typesThatAreNotNestedMustBeVisible =
                Types().That().AreNotNested().Should().BePublic().OrShould().BeInternal();
            Assert.True(typesThatAreNotNestedMustBeVisible.Check(Architecture));
        }

        [Fact]
        public void TypesWithRestrictedVisibilityMustBeNested()
        {
            var typesWithRestrictedVisibilityMustBeNested = Types().That().ArePrivate().Or()
                .AreProtected().Or().ArePrivateProtected().Or().AreProtectedInternal().Should().BeNested();
            Assert.True(typesWithRestrictedVisibilityMustBeNested.Check(Architecture));
        }
    }
}