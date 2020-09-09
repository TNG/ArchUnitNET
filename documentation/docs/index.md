![ArchUnit][logo]
[logo]: ArchUnit-Logo.png
# ArchUnitNET User Guide

## 1. Installation
To use ArchUnitNET, install the ArchUnitNET package from [NuGet](https://www.nuget.org/packages/TngTech.ArchUnitNET/):
```
PS> Install-Package ArchUnitNET
```
If you want to use xUnit or NUnit for your unit tests, you should instead install the corresponding ArchUnit extension:
```
PS> Install-Package ArchUnitNET.xUnit
PS> Install-Package ArchUnitNET.NUnit
```
## 2. Quick Start

After Installation you will want to create a class to start testing. We used xUnit with the ArchUnit extension here, but it works similarly with NUnit or other Unit Test Frameworks.

Find this example code [here](https://github.com/TNG/ArchUnitNET/blob/master/ExampleTest/ExampleArchUnitTest.cs).
#### 2.1. Directives
```cs

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

```

#### 2.2. Load Architecture
Load your architecture once at the start to maximize performance of your tests
replace <ExampleClass\> and <ForbiddenClass\> with classes from the assemblies you want to test
```
private static readonly Architecture Architecture =
    new ArchLoader().LoadAssemblies(typeof(ExampleClass).Assembly, 
    typeof(ForbiddenClass).Assembly).Build();
```
#### 2.3. Declare Layers
Declare variables you'll use throughout your tests up here
```
//use As() to give your variables a custom description
private readonly IObjectProvider<IType> ExampleLayer =
    Types().That().ResideInAssembly("ExampleAssembly").As("Example Layer");

private readonly IObjectProvider<Class> ExampleClasses =
    Classes().That().ImplementInterface("IExampleInterface").As("Example Classes");

private readonly IObjectProvider<IType> ForbiddenLayer =
    Types().That().ResideInNamespace("ForbiddenNamespace").As("Forbidden Layer");

private readonly IObjectProvider<Interface> ForbiddenInterfaces =
    Interfaces().That().HaveFullNameContaining("forbidden").As("Forbidden Interfaces");

```
#### 2.4. Test Cases

Testing if above defined "ExampleClasses" reside in "ExampleLayer"
```
[Fact]
public void TypesShouldBeInCorrectLayer()
{
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
Testing if the types in "ExampleLayer" depend on any object in "ForbiddenLayer"
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
Testing naming of classes implementing "ForbiddenInterfaces"
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
```
## 3. What to Check
The following section illustrates some typical checks you could do with ArchUnitNET.

Find this example code [here](https://github.com/TNG/ArchUnitNET).

### 3.1. Namespace Dependency Rule
![Namespace_Dependency](http://www.plantuml.com/plantuml/png/9OunZW8n34NxFSNk1SRzLhF5YWA92cfadCyaqiI975jeE3s3QFxNUzMRXxDvrFNhfwYiaH2sVcVtFdw9Z1_JKZp7BGPCcZhK9scL3cYs70tnhBmP_PdaYnO__P_f3lOma86JPwGcW_Q92dNsqfG-gl1YN0SfLupEWcj2XoQeR5D_9yqxxVy2)
```
IArchRule rule = Types().That().ResideInNamespace("Model").Should()
                    .NotDependOnAny(Types().That().ResideInNamespace("Controller"));
```
### 3.2. Class Dependency Rule
![Class_Dependency](http://www.plantuml.com/plantuml/png/9OuzhW8n30RxTuh71TOzrL8W2YGgg95np2PIugpy8xISdY2QxzDCDRCvgaUhVxiYL2DnQFtFxglj0HHVqr8ynoECbReuYq_K6vouwt9MZdV3JU6Wb6zI_7AymfFHGncKCNdcrCbWJ3GMHozTuASABehDW2gQtHElB8c5jcdzvio3ndy0)
````
IArchRule rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
                    .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));
````
### 3.3. Inheritance Naming Rule
![Inheritance_Naming](http://www.plantuml.com/plantuml/png/9Own2a8n34Ltlq9_u6LtUWxY83ZfBB6DRU0bVQG9ebzlnULUE73WpDtHF6JPx5ZJ4fd2bcrrN_iUmF0r5VOQZ-XagQcJ-fIQm8cQbRDHV0JV1WTBzQRib-CLdeGUHc24sIlbbZgC2HakEZU5Fs8iXjA1jPAneoQwAmfhcTPVq4oQnty1)
````
IArchRule rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
                    .HaveNameContaining("Car");
````
### 3.4. Class Namespace Containment Rule
![Class_Namespace_Containment](http://www.plantuml.com/plantuml/png/9Swz2i9040JWtgVG5yZsLh4G2y5Awx3UhRc3sxsmFuW-lGasCp3DDwCvgWUttQ5AQf4fjEsB-s970CNtC5SlyGmZJLsSn8VK6IxKTRugnPVnet3IL1vI_NQ-mulGGmcKC7kXt9F16i4aZbwxm7-gE2koGMfeDWaosOA9fet1AhnHo_Pi9Cbh_m00)
````
IArchRule rule = Classes().That().HaveNameContaining("Canvas").Should()
                     .ResideInNamespace(typeof(ICanvas).Namespace);
````
### 3.5. Attribute Access Rule
![Attribute_Access](http://www.plantuml.com/plantuml/png/9Ownpi8m30Rt_ob-Ng3UcSg0692OO4PkOff8ZLFPduNWwIbYkPDkkj71XcZBtwvgMY9BsF9VWDNtHCRFOQueCOMBfQuGnP1wevDqcyycyMxyCZfOAZUjk1olz116c909PnJ9FJbjY2OAHkrAlrHvDcvE35YT0dBdbCHzMByxesZU_040)
````
IArchRule rule = Classes().That().DoNotHaveAnyAttributes(typeof(Display)).Should()
                    .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));
````

### 3.6. Cycle Rule
![Cycle](http://www.plantuml.com/plantuml/png/9Own2a8n34Ltlq9_eDcTdeCu24xw0J6DRQ5D7sc2FB_UYikzSE30MOQXchUwEnIXwf5TwpwRhUC8eFWEgLZsfmzMwc8i5gWtk73NT5NsJyDDuQIK7rBiThx3Yz5S6PGnMkROQS0SRzD5ArtWZmecnJQ0DgeSrb_3mXBdVm40)
````
IArchRule rule = Slices().Matching("Cycle.(*)").Should()
                    .BeFreeOfCycles();
````

## 4. How to check

To get a meaningful error message we recommend using 
the xUnit or nUnit extension.

### 4.1 ArchUnitNET xUnit/nUnit extension

````
IArchRule someRule = ...;
someRule.check(Architecture);
````

### 4.2 ArchUnitNET no extension
```
IArchRule someRule = ...;
bool checkedRule = someRule.HasNoViolations(Architecture);
Assert.True(checkedRule);
```

## 5. Further Reading and Examples
A complete overview of all available methods can be found [here](api.md).

Check out example code on github
[ArchUnitNET Examples](https://github.com/TNG/ArchUnitNET/tree/master/ExampleTest "ExampleTests").

