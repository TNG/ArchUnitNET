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
```
IArchRule rule = Types().That().ResideInNamespace("FooNamespace").Should()
                     .NotDependOnAny(Types().That().ResideInNamespace("BarNamespace"));
```
### 3.2. Class Dependency Rule
````
IArchRule rule = Classes().That().Are(typeof(BazClass)).Should()
                    .NotDependOnAny(Classes().That().Are(typeof(FooClass)));
````
### 3.3. Class and Namespace Containment Rule
````
IArchRule rule = Classes().That().HaveNameContaining("Foo").Should()
                    .ResideInNamespace("FooNamespace");
````

### 3.4. Inheritance Rule
````
IArchRule rule = Classes().That().AreAssignableTo(typeof(IConnection)).Should()
                    .HaveNameContaining("Connection");
````

### 3.5. Attribute Rule
````
IArchRule rule = Classes().That().DoNotHaveAnyAttributes(typeof(Transactional)).Should()
                    .NotDependOnAny(Classes().That().AreAssignableTo(typeof(IEntityManager)));
````

### 3.6. Cycle Rule
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

