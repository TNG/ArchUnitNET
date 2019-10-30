//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

// ReSharper disable InconsistentNaming
// ReSharper disable SuggestVarOrType_SimpleTypes


//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;


namespace ExampleTest
{
    public class ExampleArchUnitTest
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(ExampleClass).Assembly, typeof(ForbiddenClass).Assembly).Build();
        // replace <ExampleClass> and <ForbiddenClass> with classes from the assemblies you want to test

        //declare variables you'll use throughout your tests up here
        //use As() to give them a custom description
        private readonly IObjectProvider<IType> ExampleLayer =
            Types().That().ResideInAssembly("ExampleAssembly").As("Example Layer");

        private readonly IObjectProvider<Class> ExampleClasses =
            Classes().That().ImplementInterface("IExampleInterface").As("Example Classes");

        private readonly IObjectProvider<IType> ForbiddenLayer =
            Types().That().ResideInNamespace("ForbiddenNamespace").As("Forbidden Layer");

        private readonly IObjectProvider<Interface> ForbiddenInterfaces =
            Interfaces().That().HaveFullNameContaining("forbidden").As("Forbidden Interfaces");

        [Fact]
        public void ExampleClassesShouldNotCallForbiddenMethods()
        {
            Classes().That().Are(ExampleClasses).Should().NotCallAny(
                    MethodMembers().That().AreDeclaredIn(ForbiddenLayer).Or().HaveNameContaining("forbidden"))
                .Check(Architecture);
        }

        [Fact]
        public void ExampleLayerShouldNotAccessForbiddenLayer()
        {
            //you can give your rules a custom reason, which is displayed when it fails (together with the types that failed the rule)
            IArchRule exampleLayerShouldNotAccessForbiddenLayer = Types().That().Are(ExampleLayer).Should()
                .NotDependOnAny(ForbiddenLayer).Because("it's forbidden");
            exampleLayerShouldNotAccessForbiddenLayer.Check(Architecture);
        }

        [Fact]
        public void ForbiddenClassesShouldHaveCorrectName()
        {
            Classes().That().AreAssignableTo(ForbiddenInterfaces).Should().HaveNameContaining("forbidden")
                .Check(Architecture);
        }


        //write some tests
        [Fact]
        public void TypesShouldBeInCorrectLayer()
        {
            //you can use the fluent API to write your own rules
            IArchRule exampleClassesShouldBeInExampleLayer =
                Classes().That().Are(ExampleClasses).Should().Be(ExampleLayer);
            IArchRule forbiddenInterfacesShouldBeInForbiddenLayer =
                Interfaces().That().Are(ForbiddenInterfaces).Should().Be(ForbiddenLayer);

            //check if your architecture fulfils your rules
            exampleClassesShouldBeInExampleLayer.Check(Architecture);
            forbiddenInterfacesShouldBeInForbiddenLayer.Check(Architecture);

            //you can also combine your rules
            IArchRule combinedArchRule =
                exampleClassesShouldBeInExampleLayer.And(forbiddenInterfacesShouldBeInForbiddenLayer);
            combinedArchRule.Check(Architecture);
        }
    }
}

internal class ExampleClass
{
}

internal class ForbiddenClass
{
}