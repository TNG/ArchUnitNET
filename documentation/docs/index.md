![ArchUnit][logo]
[logo]: ArchUnit-Logo.png
# ArchUnitNET [![Build Status](https://travis-ci.com/TNG/ArchUnitNET.svg?branch=master)](https://travis-ci.com/TNG/ArchUnitNET) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/TNG/ArchUnitNET/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/TngTech.ArchUnitNET)](https://www.nuget.org/packages/TngTech.ArchUnitNET/)

## Installation
To use ArchUnitNET, install the ArchUnitNET package from NuGet:
```
PS> Install-Package ArchUnitNET
```
If you want to use xUnit or NUnit for your unit tests, you should instead install the corresponding ArchUnit extension:
```
PS> Install-Package ArchUnitNET.xUnit
PS> Install-Package ArchUnitNET.NUnit
```
## Quick Start

After Installation you will want to create a class to start testing. We used xUnit with the ArchUnit extension here, but it works similarly with NUnit or other Unit Test Frameworks.

#### Directives
```cs

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

```

#### Load Architecture
```
namespace ExampleTest
{
    public class ExampleArchUnitTest
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(ExampleClass).Assembly, typeof(ForbiddenClass).Assembly)
            .Build();
        // replace <ExampleClass> and <ForbiddenClass> with classes from the assemblies you want to test
```
#### Declare Layers
```
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

```
#### Test Cases

Testing if above defined "ExampleClasses" reside in "ExampleLayer"
```
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
                exampleClassesShouldBeInExampleLayer
				.And(forbiddenInterfacesShouldBeInForbiddenLayer);
			
            combinedArchRule.Check(Architecture);
        }
```
Testing if the Types in "ExampleLayer" depend on any Type in "ForbiddenLayer"
```
        [Fact]
        public void ExampleLayerShouldNotAccessForbiddenLayer()
        {
            //you can give your rules a custom reason, which is displayed when it fails
            //(together with the types that failed the rule)
            IArchRule exampleLayerShouldNotAccessForbiddenLayer = Types().That()
				.Are(ExampleLayer).Should().NotDependOnAny(ForbiddenLayer)
				.Because("it's forbidden");
            exampleLayerShouldNotAccessForbiddenLayer.Check(Architecture);
        }
```
Testing naming of Classes implementing "ForbiddenInterfaces"
```
        [Fact]
        public void ForbiddenClassesShouldHaveCorrectName()
        {
            Classes().That().AreAssignableTo(ForbiddenInterfaces)
				.Should().HaveNameContaining("forbidden").Check(Architecture);
        }
```
Testing if "ExampleClasses" call any Method declared in "ForbiddenLayer" or with name containing "forbidden"
```
        [Fact]
        public void ExampleClassesShouldNotCallForbiddenMethods()
        {
            Classes().That().Are(ExampleClasses).Should()
				.NotCallAny(MethodMembers().That()
				.AreDeclaredIn(ForbiddenLayer).Or().HaveNameContaining("forbidden"))
                .Check(Architecture);
        }
    }
}
```


## Further Examples
Check out further examples for current release at
[ArchUnitNET Examples](https://github.com/TNG/ArchUnitNET/tree/master/ExampleTest "ExampleTests").

