// ReSharper disable InconsistentNaming
// ReSharper disable SuggestVarOrType_SimpleTypes

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ExampleTest
{
    public class ExampleArchUnitTest
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(System.Reflection.Assembly.Load("TestAssembly"))
            .Build();

        // replace <ExampleClass> and <ForbiddenClass> with classes from the assemblies you want to test

        //declare variables you'll use throughout your tests up here
        //use As() to give them a custom description
        private readonly IObjectProvider<IType> ExampleLayer = Types()
            .That()
            .ResideInAssembly("ExampleAssembly")
            .As("Example Layer");

        private readonly IObjectProvider<Class> ExampleClasses = Classes()
            .That()
            .ImplementInterface(typeof(IExampleInterface))
            .As("Example Classes");

        private readonly IObjectProvider<IType> ForbiddenLayer = Types()
            .That()
            .ResideInNamespace("ForbiddenNamespace")
            .As("Forbidden Layer");

        private readonly IObjectProvider<Interface> ForbiddenInterfaces = Interfaces()
            .That()
            .HaveFullNameContaining("forbidden")
            .As("Forbidden Interfaces");

        private readonly IObjectProvider<IType> TypoLayer = Types()
            .That()
            .ResideInNamespace("TypoNamespace")
            .As("Non-existing Typo Layer");

        //write some tests
        [Fact(Skip = "This is just a syntax example, it does not actually work")]
        public void TypesShouldBeInCorrectLayer()
        {
            //you can use the fluent API to write your own rules
            IArchRule exampleClassesShouldBeInExampleLayer = Classes()
                .That()
                .Are(ExampleClasses)
                .Should()
                .Be(ExampleLayer);
            IArchRule forbiddenInterfacesShouldBeInForbiddenLayer = Interfaces()
                .That()
                .Are(ForbiddenInterfaces)
                .Should()
                .Be(ForbiddenLayer);

            //check if your architecture fulfils your rules
            exampleClassesShouldBeInExampleLayer.Check(Architecture);
            forbiddenInterfacesShouldBeInForbiddenLayer.Check(Architecture);

            //you can also combine your rules
            IArchRule combinedArchRule = exampleClassesShouldBeInExampleLayer.And(
                forbiddenInterfacesShouldBeInForbiddenLayer
            );
            combinedArchRule.Check(Architecture);
        }

        [Fact(Skip = "This is just a syntax example, it does not actually work")]
        public void ExampleLayerShouldNotAccessForbiddenLayer()
        {
            //you can give your rules a custom reason, which is displayed when it fails (together with the types that failed the rule)
            IArchRule exampleLayerShouldNotAccessForbiddenLayer = Types()
                .That()
                .Are(ExampleLayer)
                .Should()
                .NotDependOnAny(ForbiddenLayer)
                .Because("it's forbidden");
            exampleLayerShouldNotAccessForbiddenLayer.Check(Architecture);
        }

        [Fact(Skip = "This is just a syntax example, it does not actually work")]
        public void TypoLayerShouldViolateByDefault()
        {
            // test the new default case
            IArchRule typoLayerShouldFailByDefault = Types()
                .That()
                .Are(TypoLayer)
                .Should()
                .NotDependOnAny(ForbiddenLayer)
                .Because("typo layer does not exist");

            Assert.False(typoLayerShouldFailByDefault.HasNoViolations(Architecture));

            // test the active assertion (with implicit exemption)
            IArchRule typoLayerShouldDefinitelyNotExist = Types()
                .That()
                .Are(TypoLayer)
                .Should()
                .NotExist()
                .Because("typo layer simply is not there");

            Assert.True(typoLayerShouldDefinitelyNotExist.HasNoViolations(Architecture));

            // test the explicit exemption
            IArchRule typoLayerCanCauseNoViolations = Types()
                .That()
                .Are(TypoLayer)
                .Should()
                .NotDependOnAny(ForbiddenLayer)
                .Because("typo layer does not exist and causes no violation")
                .WithoutRequiringPositiveResults();

            Assert.True(typoLayerCanCauseNoViolations.HasNoViolations(Architecture));
        }

        [Fact(Skip = "This is just a syntax example, it does not actually work")]
        public void ForbiddenClassesShouldHaveCorrectName()
        {
            Classes()
                .That()
                .AreAssignableTo(ForbiddenInterfaces)
                .Should()
                .HaveNameContaining("forbidden")
                .Check(Architecture);
        }

        [Fact(Skip = "This is just a syntax example, it does not actually work")]
        public void ExampleClassesShouldNotCallForbiddenMethods()
        {
            Classes()
                .That()
                .Are(ExampleClasses)
                .Should()
                .NotCallAny(
                    MethodMembers()
                        .That()
                        .AreDeclaredIn(ForbiddenLayer)
                        .Or()
                        .HaveNameContaining("forbidden")
                )
                .Check(Architecture);
        }
    }
}

internal class ExampleClass { }

internal class ForbiddenClass { }

internal interface IExampleInterface { }
